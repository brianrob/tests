all: coreclr mono

native:
	gcc -shared -o libnative.so -fPIC native.c

coreclr: native
	dotnet build -c Release -f netcoreapp2.2
	mv bin bin_dotnet
	cp libnative.so bin_dotnet/Release/netcoreapp2.2

mono: native
	mcs Program.cs
	mkdir bin_mono
	mv Program.exe bin_mono
	cp libnative.so bin_mono

clean:
	rm -rf bin_dotnet bin_mono bin obj libnative.so
