package main

func isLetter(r rune) bool {
	return (r >= 'A' && r <= 'Z') || (r >= 'a' && r <= 'z')
}

func isDirNameStart(r rune) bool {
	return isLetter(r) || r == '.' || r == '_'
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
