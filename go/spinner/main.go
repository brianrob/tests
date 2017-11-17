package main

import (
    "log"
    "net/http"
    "time"
)

import "expvar"
import _ "net/http/pprof"

var spinCount = expvar.NewInt("SpinCount")

func main() {

    go func() {
        log.Println("Starting HTTP")
        log.Println(http.ListenAndServe("localhost:6060", nil))
    }()

    log.Println("Starting spin")
    for {
        spin()
        spinCount.Add(1)
        time.Sleep(0)
    }
}

func spin() {
    sum := 0
    for i:=0;i<1000;i++ {
        sum += i
        sum -= i
    }
}
