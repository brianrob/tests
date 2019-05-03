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
ENV DOTNET_SDK_DOWNLOAD_URL https://download.visualstudio.microsoft.com/download/pr/a3fe6da8-1ec8-48e5-b93d-cbf6cbe4e1ad/b4c6e212cda446fcc6c305296130e76b/dotnet-sdk-3.0.100-preview4-011223-win-x64.zip
ENV DOTNET_SDK_ZIP dotnet-sdk-3.0.100-preview4-011223-win-x64.zip
ENV DOTNET_SDK_SHA512 2E208318A513315D7431F756AF8689B7E7915F9AE1C6DEC7D182BA14D874D7ACD006C1066D0CCDC41B1E9136A08B7B21B43AD3916960C970F31E503646DF516D
RUN Write-Host ('Downloading {0} ...' -f $env:DOTNET_SDK_DOWNLOAD_URL); \
	[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; \
	Invoke-WebRequest -Uri $env:DOTNET_SDK_DOWNLOAD_URL -OutFile $env:DOTNET_SDK_ZIP; \
	\
	Write-Host ('Verifying sha512 ({0}) ...' -f $env:DOTNET_SDK_SHA512); \
	if ((Get-FileHash $env:DOTNET_SDK_ZIP -Algorithm sha512).Hash -ne $env:DOTNET_SDK_SHA512) { \
		Write-Host 'FAILED!'; \
		exit 1; \
	}; \
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
