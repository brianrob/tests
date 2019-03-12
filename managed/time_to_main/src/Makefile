all: core_2.2 core_3.0 mono

native:
	gcc -shared -o libnative.so -fPIC native.c

core_2.2: native
	dotnet build -c Release -f netcoreapp2.2
	mv bin bin_core_2.2
	cp libnative.so bin_core_2.2/Release/netcoreapp2.2

core_3.0: native
	dotnet build -c Release -f netcoreapp3.0
	mv bin bin_core_3.0
	cp libnative.so bin_core_3.0/Release/netcoreapp3.0

mono: native
	mcs Program.cs
	mkdir bin_mono
	mv Program.exe bin_mono
	cp libnative.so bin_mono

clean:
	rm -rf bin_core_2.2 bin_core_3.0 bin_mono bin obj libnative.so
