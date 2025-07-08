using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace HRMS.Report.API
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
                Log.Information("Starting Reports Microservice....... ");
                var host = CreateHostBuilder(args).Build();
                host.Run();
                Log.Information("Started Reports Microservice....... ");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Reports Microservice not started correctly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                        .UseSerilog()
                        .ConfigureWebHostDefaults(webBuilder =>
                        {
                            webBuilder.UseStartup<Startup>();
                        });
        }
    }
}
