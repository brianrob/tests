build:
	gcc -I. -c tp.c
	gcc -c program.c
	gcc -o program program.o tp.o -llttng-ust -ldl -lpthread

clean:
	rm program program.o tp.o
