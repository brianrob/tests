FROM debian:stretch-20181226

RUN apt-get update && \
    apt install -y apt-transport-https dirmngr && \
    apt-key adv --no-tty --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF  && \
    echo "deb https://download.mono-project.com/repo/debian stable-stretch main" | tee /etc/apt/sources.list.d/mono-official-stable.list && \
    apt-get update && \
    apt-get install -y \
        mono-devel \
        make \
        git \
        gcc && \
    rm -rf /var/lib/apt/lists/*

RUN mono --aot=llvm /usr/lib/mono/4.5/mscorlib.dll
RUN for i in /usr/lib/mono/gac/*/*/*.dll; do mono --aot=llvm $i; done

WORKDIR /src
RUN git clone https://github.com/brianrob/tests && \
    cd tests/managed/time_to_main/src && \
    make mono

WORKDIR /src/tests/managed/time_to_main/src/
ENTRYPOINT ["./run_mono.sh"]
