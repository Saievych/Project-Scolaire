package main

import (
	"fmt"
	"io/ioutil"
    "strings"
	"sync"
	"os"
	"time"
)
var wg sync.WaitGroup


func check(e error) {
    if e != nil {
        panic(e)
    }
}
	
func psearch(text string, i int) {	
	f, err := os.Open("input.txt")
	fi, err := f.Stat()
	if err != nil {
	}
	startp := (fi.Size()/12)*int64(i)
	if(i!=0){
			startp=startp-int64(len(text))
		}
	length:=(fi.Size()/12)
	if(i==11){
			length=length+int64(len(text))
		}
	move, err := f.Seek(startp, 0)
    check(err)
    buf := make([]byte,length)
    str, err := f.Read(buf)
    check(err)
    
	if strings.Contains(string(buf[:str]), text){
		fmt.Println("Its alive")
	}else {
		fmt.Println("-")
	}
	fmt.Println("Thread",move)
	defer wg.Done()
	}
	
	
func search(text string) {

    b, err := ioutil.ReadFile("input.txt")
    if err != nil {
        panic(err)
		fmt.Printf("file doesnt exist")
    }
    s := string(b)
    if strings.Contains(s, text){
		fmt.Println("substring exist")
	}else {
		fmt.Println("substring dont exist")
	}	
	
}

func main() {
	strt:=time.Now()
	wg.Add(12)
	for i := 0; i < 12; i++ {		
		go psearch("aFpvBlJ69qSnNUhu6ilZBE OiISH9RbCUBN1bHHzf pGcIoqsdfsdgifdgj qisdhg uqdhguqhdkf hkqusdhf uhqsdkuhfksqd",i)
	}
	wg.Wait()
	fmt.Println("Time with threads: ", time.Since(strt))
	strt=time.Now()
	search("sdsfsdfsdgdsf")
	
	fmt.Println("Time: without threads ", time.Since(strt))
	
}



//aFpvBlJ69qSnNUhu6ilZBE OiISH9RbCUBN1bHHzf pGcIo