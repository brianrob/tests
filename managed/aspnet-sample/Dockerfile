FROM mcr.microsoft.com/dotnet/sdk:5.0

# Download dependencies.
RUN apt-get update && \
    apt-get install -y unzip

# Download the repo.
ADD https://github.com/brianrob/tests/archive/master.zip /repo-root/master.zip

# Extract the repo.
WORKDIR /repo-root
RUN unzip master.zip

# Build the app.
WORKDIR /repo-root/tests-master/managed/aspnet-sample
RUN dotnet build -c Release

# Run the app.
WORKDIR /repo-root/tests-master/managed/aspnet-sample/bin/Release/net5.0
CMD ./aspnet-sample
