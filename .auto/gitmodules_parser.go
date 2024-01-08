package main

import (
	"fmt"
	"os"
	"strings"
)

type gitModule struct {
	Name string
	Path string
	URL  string
}

func (g gitModule) String() string {
	return fmt.Sprintf("%s %s %s", g.Name, g.Path, g.URL)
}

func joinPath(path, lookingFor string) string {
	if strings.HasSuffix(path, lookingFor) {
		return path
	}

	if strings.HasSuffix(path, "/") {
		return path + lookingFor
	}

	return strings.Join([]string{path, lookingFor}, "/")
}

const (
	gitModulesFile = ".gitmodules"

	subModuleStart = "[submodule"
	subModuleEnd   = "]"
	pathStart      = "path ="
	urlStart       = "url ="
)

// when building tree for hole, we don't want to go dip into the git submodule
// directories, so we need to parse the .gitmodules file and ignore them.
//
// replacing path with a redirect url to the GitHub repository.
func parseGitModules(path string) ([]gitModule, error) {
	// idea: is really simple, we're reading the file and trimming whitespaces.
	// for simplicity, we're assuming 3 lines as an atomic unit of data.

	content, err := os.ReadFile(joinPath(path, gitModulesFile))
	if err != nil {
		return nil, fmt.Errorf("error reading %s: %v", gitModulesFile, err)
	}

	var lines = strings.Split(string(content), "\n")
	fmt.Println(len(lines))
	if len(lines)%3 != 0 {
		return nil, fmt.Errorf("fix .gitmodules file, it's not a multiple of 3")
	}

	return actualParsing(lines), nil
}

func trimmingChain(s string, fn ...func(string) string) string {
	for _, f := range fn {
		s = f(s)
	}

	return s
}

func actualParsing(lines []string) []gitModule {
	modules := make([]gitModule, 0, len(lines)/3)
	for i := 0; i < len(lines); i += 3 {
		moduleName := trimmingChain(
			lines[i],
			strings.TrimSpace,
			func(s string) string { return strings.TrimPrefix(s, subModuleStart) },
			func(s string) string { return strings.TrimSuffix(s, subModuleEnd) },
			strings.TrimSpace,
		)

		modulePath := trimmingChain(
			lines[i+1],
			strings.TrimSpace,
			func(s string) string { return strings.TrimPrefix(s, pathStart) },
			strings.TrimSpace,
		)

		moduleURL := trimmingChain(
			lines[i+2],
			strings.TrimSpace,
			func(s string) string { return strings.TrimPrefix(s, urlStart) },
			strings.TrimSpace,
		)

		modules = append(modules, gitModule{
			Name: moduleName,
			Path: modulePath,
			URL:  moduleURL,
		})
	}

	return modules
}
