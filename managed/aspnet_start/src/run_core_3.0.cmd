powershell -Command ".\run.ps1" {Start-Process -FilePath "dotnet.exe" -ArgumentList "bin\Release\netcoreapp3.0\aspnet_start.dll" -NoNewWindow -Wait}
