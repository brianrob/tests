using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace emptyweb_start
{
    public class Program
    {
        public static Stopwatch _startupTimer = new Stopwatch();

        public static void Main(string[] args)
        {
            _startupTimer.Start();
            IWebHost webHost = CreateWebHostBuilder(args).Build();
            Startup.WebHost = webHost;
            webHost.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
