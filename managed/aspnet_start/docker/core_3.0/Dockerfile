FROM debian:stretch

RUN apt-get update && \
    apt-get install -y apt-transport-https dirmngr gpg wget make git gcc curl tar

WORKDIR /dotnet
RUN curl -OL https://download.visualstudio.microsoft.com/download/pr/35c9c95a-535e-4f00-ace0-4e1686e33c6e/b9787e68747a7e8a2cf8cc530f4b2f88/dotnet-sdk-3.0.100-preview3-010431-linux-x64.tar.gz && \
    tar -xzvf dotnet-sdk-3.0.100-preview3-010431-linux-x64.tar.gz

RUN apt-get update && \
    apt-get install -y libunwind8 icu-devtools

ENV PATH=/dotnet:$PATH
WORKDIR /src
RUN git clone https://github.com/brianrob/tests && \
    cd tests/managed/aspnet_start/src && \
    make core_3.0

ENV ASPNETCORE_URLS http://+:8080
WORKDIR /src/tests/managed/aspnet_start/src/
ENTRYPOINT ["./run_core_3.0.sh"]
