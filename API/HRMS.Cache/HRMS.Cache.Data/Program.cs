using HRMS.Cache.Database.Entities;
using HRMS.Cache.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using StackExchange.Redis;
using HRMS.Cache.Database;
using Newtonsoft.Json;
using Serilog;
using HRMS.Cache.Linq;
using System.Diagnostics;
using AutoMapper;

namespace HRMS.Cache.DataService
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = LoadConfiguration();

            Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(config)
                        .CreateLogger();
            try
            {
                Log.Information("  Starting Caching the data....... ");
                var services = ConfigureServices(config);

                var serviceProvider = services.BuildServiceProvider();

                Log.Information("Started Caching the data....... ");
                serviceProvider.GetService<DataCacheService>().Run();
                Log.Information("Completed Caching the data....... ");
                // Create new stopwatch.
                Stopwatch stopwatch = new Stopwatch();

                // Begin timing.
                //stopwatch.Start();

                //serviceProvider.GetService<SpToLinq>().GetEmployeesBySkillUsingCache();

                //// Stop timing.
                //stopwatch.Stop();

                //Log.Information("  Time to execute GetEmployeesBySkillUsingCache:" + stopwatch.Elapsed);

                // Begin timing.
                //stopwatch.Start();

                //serviceProvider.GetService<SpToLinq>().GetProjectDetailUsingCache("Program Manager", 212, "ProjectDashboard");

                //// Stop timing.
                //stopwatch.Stop();

                //Log.Information("Time to execute GetProjectDetailUsingCache for Parameter \"Program Manager\", 212, \"ProjectDashboard\" :" + stopwatch.Elapsed);

                //stopwatch.Start();

                //serviceProvider.GetService<SpToLinq>().GetProjectDetailUsingCache("Department Head", 213, "ProjectDashboard");

                //// Stop timing.
                //stopwatch.Stop();

                //Log.Information("Time to execute GetProjectDetailUsingCache for Parameter \"Department Head\", 213, \"ProjectDashboard\":" + stopwatch.Elapsed);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "  Data caching service not started correctly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }            
        }

        private static IServiceCollection ConfigureServices(IConfiguration config)
        {
            IServiceCollection services = new ServiceCollection();

            services.AddLogging(configure => configure.AddSerilog())
                    .AddTransient<ICacheService, CacheService>()
                    .AddTransient<DataCacheService>()
                    .AddTransient<SpToLinq>();
            services.AddAutoMapper(typeof(Program));
            services.AddDbContext<OrgCoreDBContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("OrgCoreDB"));
            });

            services.AddDbContext<ProjectDBContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("ProjectDB"));
            });

            services.AddDbContext<EmployeeDBContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("EmployeeDB"));
            });

            services.AddSingleton<IConnectionMultiplexer>
                (ConnectionMultiplexer.Connect(
                    config.GetValue<string>("redis:connection")));

            return services;
        }

        public static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()               
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);            
           
            return builder.Build();
        }
    }
}
