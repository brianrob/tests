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
        public static readonly string LatestSuccessfulList = Path.Combine(TempDirectoryRoot, "LatestSuccessful.txt");

        public static readonly TimeSpan OperationTimeout = TimeSpan.FromSeconds(1000); // TODO
        public static readonly int ShutdownTimeOutMilliseconds = 5000;
        public const string AppStartupSuccessStartString = "      Content root path:";
        public static readonly TimeSpan PollingInterval = TimeSpan.FromMilliseconds(10);

        public const string ServerUrl = "http://localhost:5000/WeatherForecast";
        public const string ShutdownUrl = "http://localhost:5000/shutdown";
        public const int NumRequests = 2;

        public const ulong BlockSize = 8 * 1024;
        public const int NumPermutations = 10;
    }
}
