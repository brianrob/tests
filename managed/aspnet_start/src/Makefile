all: core_2.2 core_3.0 mono

core_2.2:
	dotnet build -c Release -f netcoreapp2.2
	mv bin bin_core_2.2

core_3.0:
	dotnet build -c Release -f netcoreapp3.0
	mv bin bin_core_3.0

mono:
	dotnet publish -c Release -f net471 -r linux-x64
	mv bin bin_mono

clean:
	rm -rf bin_core_2.2 bin_core_3.0 bin_mono bin obj
