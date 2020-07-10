FROM debian:buster

ADD https://download.visualstudio.microsoft.com/download/pr/0c795076-b679-457e-8267-f9dd20a8ca28/02446ea777b6f5a5478cd3244d8ed65b/dotnet-sdk-3.1.300-linux-x64.tar.gz /sdk/

WORKDIR /sdk/
RUN tar -xzvf dotnet-sdk-3.1.300-linux-x64.tar.gz
ENV PATH=${PATH}:/sdk

WORKDIR /perf/
ADD https://aka.ms/perfcollect /perf/
RUN apt-get update && \
    chmod +x perfcollect && \
    ./perfcollect install -force

WORKDIR /app/
RUN apt-get install -y git && \
    git clone https://github.com/timheuer/buildtest

ENV COMPlus_PerfMapEnabled=1
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
ENV DOTNET_NOLOGO=true
