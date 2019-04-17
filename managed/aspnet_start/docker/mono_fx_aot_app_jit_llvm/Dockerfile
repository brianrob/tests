FROM debian:stretch-20181226

# Install tools and dependencies.
RUN apt-get update && \
    apt-get install -y \
        apt-transport-https \
        dirmngr \
        gnupg \
        ca-certificates \
        make \
        git \
        gcc \
        g++ \
        gdb \
        autoconf \
        libtool \
        automake \
        cmake \
        gettext \
        python

# Install dotnet 2.2 SDK.
RUN apt-get install -y gpg wget apt-transport-https dirmngr && \
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

# Install Mono.
RUN apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF && \
    echo "deb https://download.mono-project.com/repo/debian nightly-stretch main" | tee /etc/apt/sources.list.d/mono-official-nightly.list && \
    echo "deb https://download.mono-project.com/repo/debian preview-stretch main" | tee /etc/apt/sources.list.d/mono-official-preview.list && \
    apt-get update && \
    apt-cache madison mono-devel && \
    apt-get install -y mono-devel=6.3.0.202-0nightly4+debian9b1 \
        mono-dbg=6.3.0.202-0nightly4+debian9b1 \
        mono-runtime-dbg=6.3.0.202-0nightly4+debian9b1

# AOT the framework.
RUN for i in /usr/lib/mono/gac/*/*/*.dll; do echo "=====" && echo "Starting AOT: $i" && echo "=====" && mono --aot=llvm $i && echo ""; done

# Set the MONO command line.
ENV MONOCMD='mono --llvm'

# Clone and build the test.
WORKDIR /src
RUN git clone https://github.com/brianrob/tests
WORKDIR /src/tests/managed/aspnet_start/src
RUN make mono

# Run the test.
WORKDIR /src/tests/managed/aspnet_start/src
ENV ASPNETCORE_URLS http://+:8080
ENTRYPOINT ["./run_mono.sh"]
