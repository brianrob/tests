FROM mcr.microsoft.com/dotnet/framework/sdk:4.8

# Download the repo.
ADD https://github.com/brianrob/tests/archive/master.zip c:\repo-root\master.zip

# Extract the repo.
WORKDIR c:\repo-root
RUN unzip master.zip

# Build the app.
WORKDIR c:\repo-root\tests-master\managed\system-resources
RUN dotnet build -c Release

# Run the app.
WORKDIR c:\repo-root\tests-master\managed\system-resources\bin\Release\net5.0
CMD system-resources.exe
