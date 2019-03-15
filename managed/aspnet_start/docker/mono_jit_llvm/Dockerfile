FROM debian:stretch-20181226

RUN apt-get update && \
    apt install -y apt-transport-https dirmngr && \
    apt-key adv --no-tty --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF  && \
    echo "deb https://download.mono-project.com/repo/debian stable-stretch main" | tee /etc/apt/sources.list.d/mono-official-stable.list && \
    apt-get update && \
    apt-get install -y \
        mono-devel=5.18.0.268-0xamarin9+debian9b1 \
        make \
        git \
        gcc && \
    rm -rf /var/lib/apt/lists/*

RUN apt-get update && \
    apt-get install -y gpg wget && \
    wget -qO- https://packages.microsoft.com/keys/microsoft.asc | gpg --no-tty --dearmor > microsoft.asc.gpg && \
    mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/ && \
    wget -q https://packages.microsoft.com/config/debian/9/prod.list && \
    mv prod.list /etc/apt/sources.list.d/microsoft-prod.list && \
    chown root:root /etc/apt/trusted.gpg.d/microsoft.asc.gpg && \
    chown root:root /etc/apt/sources.list.d/microsoft-prod.list && \
    apt-get update && \
    apt-get -y install \
        dotnet-sdk-2.2

WORKDIR /src
RUN git clone https://github.com/brianrob/tests

ENV MONOCMD='mono --llvm'
WORKDIR /src/tests/managed/aspnet_start/src
ENTRYPOINT ["./run_mono.sh"]
