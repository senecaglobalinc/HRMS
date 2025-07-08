using HRMS.Report.Infrastructure;
using HRMS.Report.Service;
using HRMS.Report.Service.External;
using HRMS.Report.Types;
using HRMS.Report.Types.External;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Authorization;
using HRMS.Report.API.Handlers;
using System.Reflection;
using System.IO;

namespace HRMS.Report.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            // Get the environment (Development, Uat, Production, etc.)
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                            // Load environment-specific appsettings files based on the environment
                            //.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();
        }

        public bool AuthenticationRequired { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<APIEndPoints>(Configuration.GetSection("APIEndPoints"));
            services.Configure<MiscellaneousSettings>(Configuration.GetSection("MiscellaneousSettings"));

            services.AddHeaderPropagation(o =>
            {
                o.Headers.Add("Authorization");
                o.Headers.Add("x-NightJob");
            });

            services.AddHttpClient("AdminClient").AddHeaderPropagation();
            services.AddHttpClient("ProjectClient").AddHeaderPropagation();
            services.AddHttpClient("EmployeeClient").AddHeaderPropagation();

            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                        options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();

                    });

            ConfigureAuthentication(services);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowMyOrigin",
                builder => builder.WithOrigins("*")
                                    .AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });

            //services.AddAutoMapper(typeof(Startup));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IOrganizationService, OrganizationService>();
            services.AddTransient<IProjectService, ProjectService>();

            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc(
                   "v2",
                    new OpenApiInfo
                    {
                        Title = "HRMS Report Microservice V2",
                        Description = "A Microservice to perform Reports related operations",
                        Version = "v2",
                        Contact = new OpenApiContact
                        {
                            Name = "Kalyan Penumutchu",
                            Email = "kalyan.penumutchu@senecaglobal.com"
                        }
                    });

                setup.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,

                      },
                      new List<string>()
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                setup.IncludeXmlComments(xmlPath);
            });

            bool authRequired = Configuration.GetValue<bool>("AuthenticationServer:EnableAuthentication");
            string nightJobs = Configuration.GetSection("NightJobClients")?.Value;

            services.AddAuthorization(options =>
            {
                if (!authRequired)
                {
                    options.DefaultPolicy = new AuthorizationPolicyBuilder()
                        .RequireAssertion(_ => true)
                        .Build();
                }

                options.AddPolicy("NightJobHeaderAuthPolicy",
                    policy => policy.Requirements.Add(new NightJobHeaderAuthRequirement("x-NightJob")));
            });

            services.AddScoped<IAuthorizationHandler, NighlyJobHeaderAuthHandler>();

            //var mappingConfig = new MapperConfiguration(mc =>
            //{
            //    mc.AddProfile(new AutoMapperProfile());
            //});
            //IMapper mapper = mappingConfig.CreateMapper();
            //services.AddSingleton(mapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            string currentEnv = Configuration.GetValue<string>("MiscellaneousSettings:Environment");

            if (!currentEnv.Equals("PROD"))
            {
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v2/swagger.json", "HRMS Report Microservice V2");
                    options.RoutePrefix = string.Empty;
                });
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            //app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseHeaderPropagation();

            app.UseCors("AllowMyOrigin");

            if (AuthenticationRequired)
            {
                app.UseAuthentication();
            }

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        //Private Method
        private void ConfigureAuthentication(IServiceCollection services)
        {
            string authority = Configuration.GetSection("AuthenticationServer").GetSection("Authority").Value;
            string audience = Configuration.GetSection("AuthenticationServer").GetSection("Audience").Value;
            var requireHttpsMetadata = false;

            if (!string.IsNullOrWhiteSpace(Configuration.GetSection("AuthenticationServer")
                                                            .GetSection("RequireHttpsMetadata").Value))
            {
                requireHttpsMetadata = Convert.ToBoolean(Configuration.GetSection("AuthenticationServer")
                                                            .GetSection("RequireHttpsMetadata").Value);
            }

            if (!string.IsNullOrWhiteSpace(authority) && !string.IsNullOrWhiteSpace(audience))
            {
                AuthenticationRequired = true;

                services.AddAuthentication("Bearer")
                        .AddJwtBearer("Bearer", options =>
                        {
                            options.Authority = authority;
                            options.RequireHttpsMetadata = requireHttpsMetadata;
                            options.Audience = audience;
                        });
            }
        }
    }
}
