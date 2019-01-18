all: native managed

native:
	gcc -shared -o libnative.so -fPIC native.c

managed:
	mcs Program.cs

clean:
	rm Program.exe libnative.so
