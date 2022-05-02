using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Debugging;
using System.IO;

namespace EskomApiBufferServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureLogging((hostingContext, logging) =>
                {
                    //var configuration = hostingContext.Configuration.GetSection("Logging");

                    //logging.AddConfiguration(configuration);
                    //logging.AddSerilog();

                    //logging.ClearProviders();
                    //logging.AddConsole();
                    //logging.AddDebug();
                    //logging.AddFile(configuration);
                    //logging.AddSerilog();
                }).UseSerilog((hostingContext, logging) =>
                {
                    logging.ReadFrom.Configuration(hostingContext.Configuration);
                });
    }
}
