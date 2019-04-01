FROM debian:stretch-20181226

RUN apt-get update && \
    apt-get install -y \
        make \
        git \
        gcc \
        g++ \
        autoconf \
        libtool \
        automake \
        cmake \
        gettext \
        python && \
    rm -rf /var/lib/apt/lists/*

RUN apt-get update && \
    apt-get install -y gpg wget apt-transport-https dirmngr && \
    wget -qO- https://packages.microsoft.com/keys/microsoft.asc | gpg --no-tty --dearmor > microsoft.asc.gpg && \
    mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/ && \
    wget -q https://packages.microsoft.com/config/debian/9/prod.list && \
    mv prod.list /etc/apt/sources.list.d/microsoft-prod.list && \
    chown root:root /etc/apt/trusted.gpg.d/microsoft.asc.gpg && \
    chown root:root /etc/apt/sources.list.d/microsoft-prod.list && \
    apt-get update && \
    apt-get -y install \
        dotnet-sdk-2.2 && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /src
RUN git clone https://github.com/mono/mono -b 2019-02

WORKDIR /src/mono
RUN ./autogen.sh --disable-boehm --enable-llvm=yes && \
    make -j8 && \
    make install
ENV MONOCMD='mono'

WORKDIR /src
RUN git clone https://github.com/brianrob/tests && \
    cd tests/managed/time_to_main/src && \
    make mono

WORKDIR /src/tests/managed/time_to_main/src/
ENTRYPOINT ["./run_mono.sh"]
