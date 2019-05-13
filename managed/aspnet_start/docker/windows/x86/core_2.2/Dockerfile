FROM mcr.microsoft.com/windows:1809

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Install required packages.
WORKDIR C:\\Downloads

# Install Git for Windows.
ENV GIT_VERSION 2.11.1
ENV GIT_TAG v${GIT_VERSION}.windows.1
ENV GIT_ZIP MinGit-${GIT_VERSION}-64-bit.zip
ENV GIT_DOWNLOAD_URL https://github.com/git-for-windows/git/releases/download/${GIT_TAG}/${GIT_ZIP}
ENV GIT_DOWNLOAD_SHA256 668d16a799dd721ed126cc91bed49eb2c072ba1b25b50048280a4e2c5ed56e59
RUN Write-Host ('Downloading {0} ...' -f $env:GIT_DOWNLOAD_URL); \
	[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; \
	Invoke-WebRequest -Uri $env:GIT_DOWNLOAD_URL -OutFile $env:GIT_ZIP; \
	\
	Write-Host ('Verifying sha256 ({0}) ...' -f $env:GIT_DOWNLOAD_SHA256); \
	if ((Get-FileHash $env:GIT_ZIP -Algorithm sha256).Hash -ne $env:GIT_DOWNLOAD_SHA256) { \
		Write-Host 'FAILED!'; \
		exit 1; \
	}; \
	\
	Write-Host 'Expanding ...'; \
	Expand-Archive -Path $env:GIT_ZIP -DestinationPath C:\git\.; \
	\
	Write-Host 'Removing ...'; \
	Remove-Item $env:GIT_ZIP -Force; \
	\
	Write-Host 'Updating PATH ...'; \
	$env:PATH = 'C:\git\cmd;C:\git\mingw64\bin;C:\git\usr\bin;' + $env:PATH; \
	[Environment]::SetEnvironmentVariable('PATH', $env:PATH, [EnvironmentVariableTarget]::Machine); \
	\
	Write-Host 'Verifying install ...'; \
	Write-Host '  git --version'; git --version; \
	\
	Write-Host 'Complete.';

# Install the dotnet SDK.
ENV DOTNET_SDK_DOWNLOAD_URL https://dotnetcli.blob.core.windows.net/dotnet/Sdk/master/dotnet-sdk-latest-win-x86.zip
ENV DOTNET_SDK_ZIP dotnet-sdk-latest-win-x86.zip
RUN Write-Host ('Downloading {0} ...' -f $env:DOTNET_SDK_DOWNLOAD_URL); \
	[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; \
	Invoke-WebRequest -Uri $env:DOTNET_SDK_DOWNLOAD_URL -OutFile $env:DOTNET_SDK_ZIP; \
	\
	Write-Host 'Expanding ...'; \
	Expand-Archive -Path $env:DOTNET_SDK_ZIP -DestinationPath C:\dotnet-sdk\.; \
	\
	Write-Host 'Removing ...'; \
	Remove-Item $env:DOTNET_SDK_ZIP -Force; \
	\
	Write-Host 'Updating PATH ...'; \
	$env:PATH = 'C:\dotnet-sdk;' + $env:PATH; \
	[Environment]::SetEnvironmentVariable('PATH', $env:PATH, [EnvironmentVariableTarget]::Machine); \
	\
	Write-Host 'Verifying install ...'; \
	Write-Host '  dotnet --info'; dotnet --info; \
	\
	Write-Host 'Complete.';

# Clone the test repo.
WORKDIR C:\\src
ENV TESTS_GIT_URL https://github.com/brianrob/tests
ENV TESTS_SUBDIR tests
ENV TESTS_COMMIT_HASH b6286cd423bfa53e179cae23873fa72d2ba207d9
RUN Write-Host ('Cloning {0} ...' -f $env:TESTS_GIT_URL); \
    git clone $env:TESTS_GIT_URL $env:TESTS_SUBDIR; \
    \
    cd $env:TESTS_SUBDIR; \
    \
    Write-Host ('Checking out {0} ...' -f $env:TESTS_COMMIT_HASH); \
    git checkout $env:TESTS_COMMIT_HASH; \
    \
    Write-Host 'Complete.'

# Build the test.
WORKDIR C:\\src\\tests\\managed\\aspnet_start\\src
RUN dotnet publish -c Release -f netcoreapp2.2 --self-contained -r win-x86

# Allow PowerShell scripts to be run.
RUN Set-ExecutionPolicy Unrestricted

# Run the test.
ENTRYPOINT .\run.ps1 {Start-Process -FilePath "c:\src\tests\managed\aspnet_start\src\bin\Release\netcoreapp2.2\win-x86\publish\aspnet_start.exe" -NoNewWindow -Wait}
