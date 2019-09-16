using System;
using System.Diagnostics;
using System.IO;

namespace AppZapper
{
    public sealed class Config
    {
        public const string AppDirectory = @"C:\src\link-a-thon\src\ApiTemplate\bin\Release\netcoreapp3.0\win-x64\publish";
        public const string AppExeName = "ApiTemplate.exe";
        public static readonly string AppPath = Path.Combine(AppDirectory, AppExeName);

        public const string TempDirectoryRoot = @"C:\work\ZapperTemp";
        public static readonly string LatestSuccessfulBits = Path.Combine(TempDirectoryRoot, "LatestSuccessful");

        public static readonly TimeSpan OperationTimeout = TimeSpan.FromSeconds(1000); // TODO
        public const string AppStartupSuccessStartString = "      Content root path:";
        public static readonly TimeSpan PollingInterval = TimeSpan.FromMilliseconds(10);

        public const string ServerUrl = "http://localhost:5000/WeatherForecast";
        public const int NumRequests = 2;

        public const ulong BlockSize = 8 * 1024;
        public const int NumPermutations = 10;
    }
}
