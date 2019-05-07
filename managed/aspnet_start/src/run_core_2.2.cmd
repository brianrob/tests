powershell -Command ".\run.ps1" {Start-Process -FilePath "dotnet.exe" -ArgumentList "bin\Release\netcoreapp2.2\aspnet_start.dll" -NoNewWindow -Wait}
