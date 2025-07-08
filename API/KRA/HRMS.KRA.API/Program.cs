using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace HRMS.KRA.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                            .AddJsonFile("appsettings.qa.json", optional: true, reloadOnChange: true)
                            .AddJsonFile("appsettings.uat.json", optional: true, reloadOnChange: true)
                            .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                Log.Information("Starting KRA Microservice....... ");
                var host = CreateHostBuilder(args).Build();
                host.Run();
                Log.Information("Started KRA Microservice....... ");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "KRA Microservice not started correctly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
