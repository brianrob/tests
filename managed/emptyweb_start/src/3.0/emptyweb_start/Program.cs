using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
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
            IHost webHost = CreateHostBuilder(args).Build();
            Startup.WebHost = webHost;
            webHost.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
