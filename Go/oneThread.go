package main

import (
	"bufio"
	"fmt"
	"log"
	"os"
	"strings"
	"time"
)

func readLines(path string) ([]string, error) {
	file, err := os.Open(path)
	if err != nil {
		return nil, err
	}
	defer file.Close()

	var lines []string
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		lines = append(lines, scanner.Text())
	}
	return lines, scanner.Err()
}

func main() {
	start := time.Now()
	searchPhrase := "gsgdsdgkiter"
	lines, err := readLines("text.txt")
	if err != nil {
		log.Fatalf("readLines: %s", err)
	}
	for _, line := range lines {
		if strings.ContainsAny(line, searchPhrase) {
			break
		}
	}
	fmt.Println("Your time : %v \n", time.Since(start))
	//fmt.Println(string(content))
}
