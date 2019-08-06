FROM debian:stretch AS build

RUN apt-get update && \
    apt-get install -y libunwind8 libicu57 make git gcc zip && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /dotnet
ADD https://download.visualstudio.microsoft.com/download/pr/35c9c95a-535e-4f00-ace0-4e1686e33c6e/b9787e68747a7e8a2cf8cc530f4b2f88/dotnet-sdk-3.0.100-preview3-010431-linux-x64.tar.gz /dotnet
RUN tar -xzvf dotnet-sdk-3.0.100-preview3-010431-linux-x64.tar.gz
ENV PATH ${PATH}:/dotnet

WORKDIR /src
ADD https://github.com/brianrob/tests/archive/master.zip /src/tests.zip
RUN unzip tests.zip && \
    cd tests-master/managed/aspnet_start/src && \
    dotnet publish -c Release -f netcoreapp3.0 --self-contained -r linux-x64

FROM debian:stretch AS runtime

WORKDIR /dotnet
ADD https://download.visualstudio.microsoft.com/download/pr/35c9c95a-535e-4f00-ace0-4e1686e33c6e/b9787e68747a7e8a2cf8cc530f4b2f88/dotnet-sdk-3.0.100-preview3-010431-linux-x64.tar.gz /dotnet
RUN apt-get update && \
    apt-get install -y libunwind8 libicu57 libssl1.1 && \
    rm -rf /var/lib/apt/lists/* && \
    tar -xzvf dotnet-sdk-3.0.100-preview3-010431-linux-x64.tar.gz
ENV PATH=/dotnet:$PATH

COPY --from=0 /src/tests-master/managed/aspnet_start/src/bin/Release/netcoreapp3.0/linux-x64/publish /app
COPY --from=0 /src/tests-master/managed/aspnet_start/src/run.sh /app/run.sh

ENV ASPNETCORE_URLS http://+:8080
ENV RUNCMD /app/aspnet_start
WORKDIR /app
ENTRYPOINT ./run.sh
