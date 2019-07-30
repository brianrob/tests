FROM debian:stretch-20181226 AS build

RUN apt-get update && \
    apt-get install -y libicu57 zip libkrb5-3 zlib1g git && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /dotnet
ADD https://download.visualstudio.microsoft.com/download/pr/c624c5d6-0e9c-4dd9-9506-6b197ef44dc8/ad61b332f3abcc7dec3a49434e4766e1/dotnet-sdk-3.0.100-preview7-012821-linux-x64.tar.gz /dotnet
RUN tar -xzvf dotnet-sdk-3.0.100-preview7-012821-linux-x64.tar.gz
ENV PATH ${PATH}:/dotnet

ENV BenchmarksTargetFramework netcoreapp3.0
ENV MicrosoftAspNetCoreAppPackageVersion 3.0.0-preview7.19365.7
ENV MicrosoftNETCoreAppPackageVersion 3.0.0-preview7-27912-14

WORKDIR /src
ADD https://github.com/aspnet/aspnetcore/archive/master.zip /src/aspnetcore.zip
RUN unzip aspnetcore.zip && \
    cd AspNetCore-master/src/Servers/Kestrel/perf/PlatformBenchmarks && \
    dotnet publish -c Release -f netcoreapp3.0 --self-contained -r linux-x64

ADD https://github.com/brianrob/tests/archive/master.zip /src/tests.zip
ENV MONO_PKG_VERSION 6.5.0.163
RUN unzip tests.zip && \
    cd /src/tests-master/managed/restore_net5 && \
    dotnet restore && \
    cp ~/.nuget/packages/runtime.linux-x64.microsoft.netcore.runtime.mono/${MONO_PKG_VERSION}/runtimes/linux-x64/native/* \
    /src/AspNetCore-master/src/Servers/Kestrel/perf/PlatformBenchmarks/bin/Release/netcoreapp3.0/linux-x64/publish && \
    mv /src/AspNetCore-master/src/Servers/Kestrel/perf/PlatformBenchmarks/bin/Release/netcoreapp3.0/linux-x64/publish/libmonosgen-2.0.so /src/AspNetCore-master/src/Servers/Kestrel/perf/PlatformBenchmarks/bin/Release/netcoreapp3.0/linux-x64/publish/libcoreclr.so

FROM debian:stretch-20181226 AS runtime

# Install tools and dependencies.
RUN apt-get update && \
    apt-get install -y libicu57 && \
    rm -rf /var/lib/apt/lists/*

# Download and install the .NET Core SDK.
WORKDIR /dotnet
ADD https://download.visualstudio.microsoft.com/download/pr/c624c5d6-0e9c-4dd9-9506-6b197ef44dc8/ad61b332f3abcc7dec3a49434e4766e1/dotnet-sdk-3.0.100-preview7-012821-linux-x64.tar.gz /dotnet
RUN tar -xzvf dotnet-sdk-3.0.100-preview7-012821-linux-x64.tar.gz
ENV PATH ${PATH}:/dotnet

# Copy the app.
COPY --from=0 /src/AspNetCore-master/src/Servers/Kestrel/perf/PlatformBenchmarks/bin/Release/netcoreapp3.0/linux-x64/publish /app

# Run the test.
WORKDIR /app
ENV ASPNETCORE_URLS http://+:8080
ENTRYPOINT ["./PlatformBenchmarks"]
