FROM debian:stretch

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

WORKDIR /src
RUN git clone https://github.com/brianrob/tests && \
    cd tests/managed/time_to_main/src && \
    make coreclr

WORKDIR /src/tests/managed/time_to_main/src/
ENTRYPOINT ["./run_core_2.2.sh"]
