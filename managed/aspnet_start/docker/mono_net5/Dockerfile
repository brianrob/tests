FROM debian:stretch

RUN apt-get update && \
    apt-get install -y apt-transport-https dirmngr gpg wget make git gcc curl tar

WORKDIR /dotnet
RUN curl -OL https://dotnetcli.blob.core.windows.net/dotnet/Sdk/master/dotnet-sdk-latest-linux-x64.tar.gz && \
    tar -xzvf dotnet-sdk-latest-linux-x64.tar.gz

RUN apt-get update && \
    apt-get install -y libunwind8 icu-devtools

ENV PATH=/dotnet:$PATH
WORKDIR /src
RUN git clone https://github.com/brianrob/tests && \
    cd tests/managed/aspnet_start/src && \
    git checkout ea86d6575b826aa5a97327184a7997078c16e09d && \
    dotnet publish -c Release -f netcoreapp3.0 --self-contained -r linux-x64

ENV MONO_PKG_VERSION 6.3.0.621
WORKDIR /src/tests/managed/restore_net5
RUN dotnet restore && \
    cp ~/.nuget/packages/runtime.linux-x64.microsoft.netcore.runtime.mono/${MONO_PKG_VERSION}/runtimes/linux-x64/native/* \
    /src/tests/managed/aspnet_start/src/bin/Release/netcoreapp3.0/linux-x64/publish

WORKDIR /src/tests/managed/aspnet_start/src/bin/Release/netcoreapp3.0/linux-x64/publish
RUN mv libmonosgen-2.0.so libcoreclr.so

ENV ASPNETCORE_URLS http://+:8080
WORKDIR /src/tests/managed/aspnet_start/src/
ENTRYPOINT ["./run_net5.sh"]
