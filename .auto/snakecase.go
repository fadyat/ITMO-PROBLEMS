package main

import (
	"errors"
	"flag"
	"log"
	"os"
	"path/filepath"
	"slices"
	"strings"
)

var (
	skippedFiles = []string{
		"README.md",
		"README.txt",
		"LICENSE",
		".DS_Store",
		".pytest_cache",
	}
)

type snakeConfig struct {
	startDir  string
	recursive bool
}

func parseSnakecaseArgs() snakeConfig {
	var startDir string
	flag.StringVar(&startDir, "startDir", "", "The directory to start renaming files from")

	var recursive bool
	flag.BoolVar(&recursive, "recursive", true, "Whether to rename files in subdirectories")

	flag.Parse()

	return snakeConfig{startDir, recursive}
}

func validateArgs(c snakeConfig) error {
	if c.startDir == "" {
		return errors.New("startDir must be specified")
	}

	return nil
}

func isUpper(c rune) bool {
	return c >= 'A' && c <= 'Z'
}

func toLower(c rune) rune {
	if isUpper(c) {
		return c + 32
	}

	return c
}

func underscoreToDash(s string, skipFirst bool) string {
	if skipFirst && s[0] == '_' {
		return "_" + strings.ReplaceAll(s[1:], "_", "-")
	}

	return strings.ReplaceAll(s, "_", "-")
}

func toSnakeCase(s string) string {
	var (
		haveSeenLowerCase bool
		snakeCaseBuilder  strings.Builder
	)

	for _, c := range s {
		if isUpper(c) {
			if haveSeenLowerCase {
				snakeCaseBuilder.WriteRune('-')
			}

			snakeCaseBuilder.WriteRune(toLower(c))
			continue
		} else {
			haveSeenLowerCase = isLetter(c)
		}

		snakeCaseBuilder.WriteRune(c)
	}

	return underscoreToDash(snakeCaseBuilder.String(), true)
}

func makeSnakeCaseFilesCopies(startDir string, recursive bool) error {
	return filepath.Walk(startDir, func(path string, info os.FileInfo, err error) error {
		if err != nil {
			return err
		}

		if info.IsDir() && !recursive {
			return filepath.SkipDir
		}

		if slices.Contains(skippedFiles, info.Name()) {
			return nil
		}

		newName := toSnakeCase(info.Name())
		if newName == info.Name() {
			return nil
		}

		return os.Rename(path, filepath.Join(filepath.Dir(path), newName))
	})
}

func main() {
	c := parseSnakecaseArgs()
	if err := validateArgs(c); err != nil {
		log.Fatal(err)
	}

	err := makeSnakeCaseFilesCopies(c.startDir, c.recursive)
	if err == nil || errors.Is(err, filepath.SkipDir) {
		return
	}

	log.Fatal(err)
}
