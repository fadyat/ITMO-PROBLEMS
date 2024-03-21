package main

import (
	"flag"
	"fmt"
	"gopkg.in/yaml.v3"
	"log"
	"os"
	"os/exec"
	"path/filepath"
	"strings"
)

type readmeConfig struct {
	Folders []struct {
		Path     string `yaml:"path"`
		TreeArgs string `yaml:"tree_args"`
		Header   string `yaml:"header"`
		Tail     string `yaml:"tail"`
	} `yaml:"folders"`
}

func parseReadmeArgs() readmeConfig {
	var startDir string
	flag.StringVar(&startDir, "startDir", "", "The directory to start generating the README from")

	flag.Parse()

	var config readmeConfig
	f, _ := os.ReadFile("docs_config.yaml")
	if err := yaml.Unmarshal(f, &config); err != nil {
		log.Fatalf("error parsing readme config: %v", err)
	}

	return config
}

func takeWhile(
	s string,
	acceptable func(rune) bool,
	fn func(string) string,
) string {
	var result strings.Builder

	for i, r := range s {
		if acceptable(r) {
			result.WriteRune(r)
		} else {
			// here is the first character that doesn't match the predicate
			// taking all the characters up to this point and
			result.WriteString(fn(s[i:]))
			break
		}
	}

	return result.String()
}

func insideCodeBlock(f *os.File, fn func()) {
	_, _ = f.WriteString("<pre>\n")
	fn()
	_, _ = f.WriteString("</pre>\n")
}

var (
	ignoreFiles = []string{
		"docs",
		"data",
		"README.md",
		"README_RU.md",
		"pdf",
		"latex",
		"README.txt",
		"readme.txt",
		"OopLabs.sln",
		"_config.yml",
		"LICENSE",
	}
)

func buildIgnore() []string {
	var result = make([]string, 0, 2*len(ignoreFiles))
	for _, s := range ignoreFiles {
		result = append(result, "-I", s)
	}

	return result
}

func getDirName(s string) string {
	if strings.HasSuffix(s, ".md") {
		return filepath.Dir(s)
	}

	return s
}

func getReadmePath(path string) string {
	if strings.HasSuffix(path, ".md") {
		return filepath.Join(path)
	}

	return filepath.Join(path, "README.md")
}

// treeStyleReadME generates a README.md file from the current directory
// in the style of the tree command with the specified header name and shortlinks
// to each directory.
func treeStyleReadME(
	startDir, header, tail string,
	treeArgs []string,
	submodules []gitModule,
) error {
	args := append([]string{getDirName(startDir), "--dirsfirst"}, buildIgnore()...)

	if treeArgs[0] != "" {
		args = append(args, treeArgs...)
	}

	cmd := exec.Command("tree", args...)
	out, err := cmd.Output()
	if err != nil {
		return err
	}

	readme, err := os.Create(getReadmePath(startDir))
	if err != nil {
		return err
	}
	defer func() { _ = readme.Close() }()

	if header != "" {
		if _, err = readme.WriteString(header + "\n"); err != nil {
			log.Printf("error writing header: %v", err)
		}
	}

	var split = strings.Split(string(out), "\n")
	insideCodeBlock(readme, func() {
		_, _ = readme.WriteString(
			buildLinkWithParents(split[1:len(split)-3], submodules),
		)
	})

	if tail != "" {
		if _, err = readme.WriteString(tail + "\n"); err != nil {
			log.Printf("error writing tail: %v", err)
		}
	}

	return nil
}

func buildLinkWithParents(
	split []string,
	submodules []gitModule,
) string {
	// idea: all parents are indented, so we can use the indentation to determine
	// the parent directories.

	var (
		parents = make([]string, 1, 10)
		indents = make([]int, 1, 10)
		sb      strings.Builder
	)
	parents[0] = "."
	indents[0] = -1

	cleanWord := func(s string) string {
		for i, r := range s {
			if isDirNameStart(r) {
				return s[i:]
			}
		}

		return ""
	}

	asMarkdownLink := func(s string, linkBuilder func() string) string {
		return takeWhile(
			s,
			func(r rune) bool { return !isDirNameStart(r) },
			func(representation string) string {
				var link = linkBuilder()
				return fmt.Sprintf("<a href=\"%s\">%s</a>", link, representation)
			},
		)
	}

	calculateIndent := func(s string) int {
		var result int
		for _, r := range s {
			if !isDirNameStart(r) {
				result++
			} else {
				break
			}
		}

		return result
	}

	searchModule := func(s string) string {
		for _, module := range submodules {
			if module.Path == s {
				return module.URL
			}
		}

		return ""
	}

	var (
		isModuleSubDir bool
		linkBuilder    = func() string { return strings.Join(parents, "/") }
	)
	for _, line := range split {
		currentIndent, storedIndent := calculateIndent(line), top(indents)
		cleaned := cleanWord(line)

		if isModuleSubDir && currentIndent > storedIndent {
			log.Printf("skipping %s, it's a submodule part", cleaned)
			continue
		}

		isModuleSubDir = false
		linkBuilder = func() string { return strings.Join(parents, "/") }
		submoduleURL := searchModule(cleaned)

		// submodules are only on top level of the project
		// need not to include directories that are named the
		// same as the submodule
		if submoduleURL != "" {
			log.Printf("found module %s", cleaned)
			isModuleSubDir = true
			linkBuilder = func() string { return submoduleURL }
		}

		if currentIndent > storedIndent {
			parents = append(parents, cleaned)
		}

		for currentIndent < storedIndent {
			parents, indents = pop(parents), pop(indents)
			storedIndent = top(indents)
		}

		if currentIndent == storedIndent {
			parents = replaceTop(parents, cleaned)
			indents = pop(indents)
		}

		indents = append(indents, currentIndent)
		sb.WriteString(asMarkdownLink(line, linkBuilder) + "\n")
	}

	return sb.String()
}

func main() {
	var (
		c               = parseReadmeArgs()
		gitmodules, err = parseGitModules("../.")
	)

	if err != nil {
		log.Fatalf("error parsing .gitmodules: %v", err)
	}

	for _, f := range c.Folders {
		var modules []gitModule

		// todo: fix this, done for not to include subfolders in RU
		//  version of the README
		if f.Path == "../." || strings.HasSuffix(f.Path, ".md") {
			fmt.Println("asodasodoasodoad", f.Path)
			modules = gitmodules
		}

		err = treeStyleReadME(f.Path, f.Header, f.Tail, strings.Split(f.TreeArgs, " "), modules)
		if err != nil {
			log.Printf("error generating README for %s: %v", f.Path, err)
			continue
		}

		log.Printf("generated README for %s", f.Path)
	}
}
