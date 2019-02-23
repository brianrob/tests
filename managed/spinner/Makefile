all: coreclr mono

coreclr:
	dotnet build -c Release -f netcoreapp2.2
	mv bin bin_dotnet

mono:
	mcs Program.cs
	mkdir bin_mono
	mv Program.exe bin_mono

clean:
	rm -rf bin_dotnet bin_mono bin obj
