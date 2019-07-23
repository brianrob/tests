FROM mcr.microsoft.com/dotnet/core-nightly/sdk:3.0.100-preview7 AS build

RUN apt-get update && \
    apt-get install -y zip && \
    rm -rf /var/lib/apt/lists/*

# Download and build the application.
WORKDIR /src
ADD https://github.com/brianrob/tests/archive/master.zip /src/tests.zip
RUN unzip tests.zip && \
    cd tests-master/managed/aspnet_start/src && \
    dotnet publish -c Release -f net471 --self-contained -r linux-x64

FROM debian:stretch-20181226 AS runtime

# Install Mono.
RUN apt-get update && \
    apt-get install -y apt-transport-https gnupg && \
    apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF && \
    echo "deb https://download.mono-project.com/repo/debian nightly-stretch main" | tee /etc/apt/sources.list.d/mono-official-nightly.list && \
    echo "deb https://download.mono-project.com/repo/debian preview-stretch main" | tee /etc/apt/sources.list.d/mono-official-preview.list && \
    apt-get update && \
#    apt-get install -y mono-runtime=6.5.0.625-0nightly1+debian9b1 && \
    apt-get install -y mono-devel=6.5.0.625-0nightly1+debian9b1 && \
    apt-get remove -y gnupg && \
    apt-get autoremove -y && \
    rm -rf /var/lib/apt/lists/*

# AOT the framework.
RUN for i in /usr/lib/mono/gac/*/*/*.dll; do echo "=====" && echo "Starting AOT: $i" && echo "=====" && mono --aot=llvm $i && echo ""; done

# Copy the app.
COPY --from=0 /src/tests-master/managed/aspnet_start/src/bin/Release/net471/linux-x64/publish /app
COPY --from=0 /src/tests-master/managed/aspnet_start/src/run.sh /app/run.sh

# Switch to the app directory to AOT the app binaries.
WORKDIR /app

# Remove desktop assemblies blocked by mono.  These will be pulled from the previously AOT'd mono implementation.
RUN rm System.Globalization.Extensions.dll System.IO.Compression.dll System.Net.Http.dll System.Threading.Overlapped.dll

# Run the test.
ENV ASPNETCORE_URLS http://+:8080
ENV RUNCMD 'mono /app/aspnet_start.exe'
CMD ./run.sh
