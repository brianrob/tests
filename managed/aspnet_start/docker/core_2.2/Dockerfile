FROM debian:stretch-20181226

RUN apt-get update && \
    apt-get install -y apt-transport-https dirmngr gpg wget && \
    wget -qO- https://packages.microsoft.com/keys/microsoft.asc | gpg --no-tty --dearmor > microsoft.asc.gpg && \
    mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/ && \
    wget -q https://packages.microsoft.com/config/debian/9/prod.list && \
    mv prod.list /etc/apt/sources.list.d/microsoft-prod.list && \
    chown root:root /etc/apt/trusted.gpg.d/microsoft.asc.gpg && \
    chown root:root /etc/apt/sources.list.d/microsoft-prod.list && \
    apt-get update && \
    apt-get -y install \
        dotnet-sdk-2.2 \
        make \
        git \
        gcc

RUN apt-get install -y curl

WORKDIR /dotnet-sdk
RUN curl -OL https://download.visualstudio.microsoft.com/download/pr/35c9c95a-535e-4f00-ace0-4e1686e33c6e/b9787e68747a7e8a2cf8cc530f4b2f88/dotnet-sdk-3.0.100-preview3-010431-linux-x64.tar.gz && \
    tar -xzvf dotnet-sdk-3.0.100-preview3-010431-linux-x64.tar.gz
ENV SAVEPATH=${PATH}
ENV PATH=/dotnet-sdk:${PATH}

WORKDIR /src
RUN git clone https://github.com/brianrob/tests && \
    cd tests/managed/aspnet_start/src && \
    make core_2.2 

ENV PATH=${SAVEPATH}
ENV ASPNETCORE_URLS http://+:8080
WORKDIR /src/tests/managed/aspnet_start/src/
ENTRYPOINT ["./run_core_2.2.sh"]
