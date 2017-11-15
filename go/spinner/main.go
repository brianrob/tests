package main

func main() {
    for {
        spin()
    }
}

func spin() {
    sum := 0
    for i:=0;i<9999999;i++ {
        sum += i
        sum -= i
    }
}
