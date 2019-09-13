dotnet c:\src\aspnet-benchmarks\src\BenchmarksDriver\bin\Release\netcoreapp2.1\BenchmarksDriver.dll ^
    -s http://asp-perflab-lin:5001 ^
    -c http://asp-perflab-load:5002 ^
    --jobs https://raw.githubusercontent.com/aspnet/AspNetCore/master/src/Servers/Kestrel/perf/PlatformBenchmarks/benchmarks.plaintext.json ^
    --scenario PlaintextPlatform ^
    --runtimeversion 3.0.0-preview7-27731-04 ^
    --aspnetcoreversion 3.0.0-preview6-19279-08 ^
    --sdk 3.0.100-preview6-012264 ^
    --hash 9f9c79bbe8cc2628e6f68a9f47f167c1c078deaf
