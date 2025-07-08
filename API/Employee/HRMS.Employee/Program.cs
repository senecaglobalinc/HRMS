using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace HRMS.Employee
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false)
                                .AddJsonFile("appsettings.qa.json", optional: true, reloadOnChange: false)
                                .AddJsonFile("appsettings.uat.json", optional: true, reloadOnChange: false)
                                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                Log.Information("Starting Employee Microservice....... ");
                var host = CreateHostBuilder(args).Build();
                host.Run();
                Log.Information("Started Employee Microservice....... ");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Employee Microservice not started correctly.");
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
