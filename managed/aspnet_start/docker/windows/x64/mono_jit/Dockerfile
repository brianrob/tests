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

# Download and install Mono nightly build.
ENV MONO_VERSION 6.3.0.340
ENV MONO_INSTALLER mono-${MONO_VERSION}-x64-0.msi
ENV MONO_DOWNLOAD_URL https://download.mono-project.com/archive/nightly/windows-installer/${MONO_INSTALLER}
RUN Write-Host ('Downloading {0} ...' -f $env:MONO_DOWNLOAD_URL); \
    Invoke-WebRequest -Uri $env:MONO_DOWNLOAD_URL -OutFile $env:MONO_INSTALLER; \
    \
    Write-Host 'Installing ...'; \
	Write-Host ('  msiexec.exe /i {0} /qn' -f $env:MONO_INSTALLER); Start-Process -FilePath "msiexec.exe" -ArgumentList '/i',$env:MONO_INSTALLER,'/qn' -Wait; \
	\
	Write-Host 'Removing ...'; \
	Remove-Item $env:MONO_INSTALLER -Force; \
	\
	Write-Host 'Updating PATH ...'; \
	$env:PATH = 'C:\Program Files\Mono\bin;' + $env:PATH; \
	[Environment]::SetEnvironmentVariable('PATH', $env:PATH, [EnvironmentVariableTarget]::Machine); \
	\
	Write-Host 'Verifying install ...'; \
	Write-Host '  mono --version'; mono --version; \
	\
	Write-Host 'Complete.'

# Install the dotnet SDK.
ENV DOTNET_SDK_DOWNLOAD_URL https://dotnetcli.blob.core.windows.net/dotnet/Sdk/master/dotnet-sdk-latest-win-x64.zip
ENV DOTNET_SDK_ZIP dotnet-sdk-latest-win-x64.zip
RUN Write-Host ('Downloading {0} ...' -f $env:DOTNET_SDK_DOWNLOAD_URL); \
	[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; \
	Invoke-WebRequest -Uri $env:DOTNET_SDK_DOWNLOAD_URL -OutFile $env:DOTNET_SDK_ZIP; \
	\
	Write-Host 'Expanding ...'; \
	Expand-Archive -Path $env:DOTNET_SDK_ZIP -DestinationPath C:\dotnet\.; \
	\
	Write-Host 'Removing ...'; \
	Remove-Item $env:DOTNET_SDK_ZIP -Force; \
	\
	Write-Host 'Updating PATH ...'; \
	$env:PATH = 'C:\dotnet;' + $env:PATH; \
	[Environment]::SetEnvironmentVariable('PATH', $env:PATH, [EnvironmentVariableTarget]::Machine); \
	\
	Write-Host 'Verifying install ...'; \
	Write-Host '  dotnet --info'; dotnet --info; \
	\
	Write-Host 'Complete.';

# Download and install the .NET 4.7.1 SDK.
ENV 471_SDK_INSTALLER NDP471-DevPack-ENU.exe
ENV 471_SDK_DOWNLOAD_URL https://download.microsoft.com/download/9/0/1/901B684B-659E-4CBD-BEC8-B3F06967C2E7/NDP471-DevPack-ENU.exe
RUN Write-Host ('Downloading {0} ...' -f $env:471_SDK_DOWNLOAD_URL); \
    Invoke-WebRequest -Uri $env:471_SDK_DOWNLOAD_URL -OutFile $env:471_SDK_INSTALLER; \
    \
    Write-Host 'Installing ...'; \
	Write-Host (' {0} /install /quiet /norestart' -f $env:471_SDK_INSTALLER); Start-Process -FilePath $env:471_SDK_INSTALLER -ArgumentList '/install','/quiet','/norestart' -Wait; \
	\
	Write-Host 'Removing ...'; \
	Remove-Item $env:471_SDK_INSTALLER -Force; \
	\
	Write-Host 'Complete.'

# Clone the test repo.
WORKDIR C:\\src
ENV TESTS_GIT_URL https://github.com/brianrob/tests
ENV TESTS_SUBDIR tests
ENV TESTS_COMMIT_HASH 3a7b78783eaa18049deae98afd93932d1b48856e
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
RUN dotnet publish -c Release -f net471 --self-contained -r win-x64

# Run the test.
ENTRYPOINT powershell -Command ".\run.ps1" {Start-Process -FilePath "mono.exe" -ArgumentList "bin\Release\net471\win-x64\aspnet_start.dll" -NoNewWindow -Wait}
