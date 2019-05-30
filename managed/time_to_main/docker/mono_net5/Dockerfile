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
    cd tests/managed/time_to_main/src && \
    git checkout f25673ede303c1ea6e610d8a48ec514fddf3a0aa && \
    gcc -shared -o libnative.so -fPIC native.c && \
    dotnet publish -c Release -f netcoreapp3.0 --self-contained -r linux-x64 && \
    cp libnative.so bin/Release/netcoreapp3.0/linux-x64/publish

ENV MONO_PKG_VERSION 6.3.0.621
WORKDIR /src/tests/managed/restore_net5
RUN dotnet restore && \
    cp ~/.nuget/packages/runtime.linux-x64.microsoft.netcore.runtime.mono/${MONO_PKG_VERSION}/runtimes/linux-x64/native/* \
    /src/tests/managed/time_to_main/src/bin/Release/netcoreapp3.0/linux-x64/publish

WORKDIR /src/tests/managed/time_to_main/src/bin/Release/netcoreapp3.0/linux-x64/publish
RUN mv libmonosgen-2.0.so libcoreclr.so

WORKDIR /src/tests/managed/time_to_main/src/
ENTRYPOINT ["./run_net5.sh"]
