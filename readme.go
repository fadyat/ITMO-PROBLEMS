package main

import (
	"flag"
	"fmt"
	"log"
	"os"
	"os/exec"
	"path/filepath"
	"strings"
)

type readmeConfig struct {
	startDir string
}

func parseReadmeArgs() readmeConfig {
	var startDir string
	flag.StringVar(&startDir, "startDir", "", "The directory to start generating the README from")

	flag.Parse()

	return readmeConfig{startDir}
}

func isDirNameStart(r rune) bool {
	return isLetter(r) || r == '.' || r == '_'
}

func asLink(link, val string) string {
	return fmt.Sprintf("<a href=\"%s\">%s</a>", link, val)
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
		"README.md",
		"pdf",
		"latex",
		"README.txt",
		"readme.txt",
		"OopLabs.sln",
	}
)

func buildIgnore() []string {
	var result = make([]string, 0, 2*len(ignoreFiles))
	for _, s := range ignoreFiles {
		result = append(result, "-I", s)
	}

	return result
}

// treeStyleReadME generates a README.md file from the current directory
// in the style of the tree command with the specified header name and shortlinks
// to each directory.
func treeStyleReadME(startDir string) error {
	args := append([]string{
		startDir, "--dirsfirst",
		"-L", "1",
	}, buildIgnore()...)
	cmd := exec.Command("tree", args...)
	out, err := cmd.Output()
	if err != nil {
		return err
	}

	readme, err := os.Create(filepath.Join(startDir, "README.md"))
	if err != nil {
		return err
	}
	defer func() { _ = readme.Close() }()

	var split = strings.Split(string(out), "\n")
	insideCodeBlock(readme, func() {
		_, _ = readme.WriteString(buildLinkWithParents(split[1 : len(split)-3]))
	})

	return nil
}

func buildLinkWithParents(split []string) string {
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

	asMarkdownLink := func(s string) string {
		return takeWhile(
			s,
			func(r rune) bool { return !isDirNameStart(r) },
			func(representation string) string {
				var link = strings.Join(parents, "/")
				return asLink(link, representation)
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

	for _, line := range split {
		currentIndent, storedIndent := calculateIndent(line), top(indents)
		cleaned := cleanWord(line)

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
		sb.WriteString(asMarkdownLink(line) + "\n")
	}

	return sb.String()
}

func top[T any](a []T) T {
	if len(a) == 0 {
		return a[0]
	}

	return a[len(a)-1]
}

func pop[T any](a []T) []T {
	if len(a) == 0 {
		return a
	}

	return a[:len(a)-1]
}

func replaceTop[T any](a []T, on T) []T {
	if len(a) == 0 {
		return append(a, on)
	}

	a[len(a)-1] = on
	return a
}

func main() {
	var c = parseReadmeArgs()
	if err := treeStyleReadME(c.startDir); err != nil {
		log.Fatal(err)
	}
}
