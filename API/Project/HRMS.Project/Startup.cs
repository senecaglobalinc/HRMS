using AutoMapper;
using HRMS.Project.API.Handlers;
using HRMS.Project.Database;
using HRMS.Project.Infrastructure;
using HRMS.Project.Infrastructure.Mappers;
using HRMS.Project.Service;
using HRMS.Project.Service.External;
using HRMS.Project.Types;
using HRMS.Project.Types.External;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HRMS.Project
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            // Get the environment (Development, Uat, Production, etc.)
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
                            // Load environment-specific appsettings files based on the environment
                            //.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();
        }
        public bool AuthenticationRequired { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {            
            services.Configure<APIEndPoints>(options => Configuration.GetSection("APIEndPoints").Bind(options));
            services.Configure<APIEndPoints>(options => Configuration.GetSection("MiscellaneousSettings").Bind(options));
            services.Configure<APIEndPoints>(options => Configuration.GetSection("EmailConfigurations").Bind(options));
            
            services.AddHeaderPropagation(o =>
            {
                o.Headers.Add("Authorization");
                o.Headers.Add("UserName");
            });

            services.AddHttpClient("EmployeeClient").AddHeaderPropagation();
            services.AddHttpClient("AdminClient").AddHeaderPropagation();

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

            services.AddDbContext<ProjectDBContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("ProjectDB"));
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IAllocationPercentageService, AllocationPercentageService>();
            services.AddTransient<IAssociateAllocationService, AssociateAllocationService>();
            services.AddTransient<IClientBillingRoleService, ClientBillingRolesService>();
            services.AddTransient<IClientBillingRoleHistoryService, ClientBillingRoleHistoryService>();
            services.AddTransient<IProjectManagerService, ProjectManagerService>();
            services.AddTransient<IProjectRoleDetailService, ProjectRoleDetailsService>();
            services.AddTransient<IProjectRolesService, ProjectRolesService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<ITalentPoolService, TalentPoolService>();
            services.AddTransient<ITalentRequisitionService, TalentRequisitionService>();
            services.AddTransient<ISOWService, SOWService>();
            services.AddTransient<IAddendumService, AddendumService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IOrganizationService, OrganizationService>();
            services.AddTransient<IReportsService, ReportsService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IProjectClosureReportService, ProjectClosureReportService>();
            services.AddTransient<IProjectClosureActivityService, ProjectClosureActivityService>();
            services.AddTransient<IProjectClosureService, ProjectClosureService>();
            services.AddTransient<IHRMSExternalService, HRMSExternalService>();
            services.AddTransient<IAssociateSkillAssessmentService, AssociateSkillAssessmentService>();

            services.AddTransient<ProjectDBContext>();

            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc(
                "v2",
                new OpenApiInfo
                {
                    Title = "HRMS Projects Microservice",
                    Version = "v2",
                    Description = "A Microservice to perform Project related operations",
                    Contact = new OpenApiContact
                    {
                        Name = "Kalyan Penumutchu",
                        Email = "kalyan.penumutchu@senecaglobal.com"
                    }
                });

                setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
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
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //setup.IncludeXmlComments(xmlPath);
            });

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            bool authRequired = Configuration.GetValue<bool>("AuthenticationServer:EnableAuthentication");
            string nightJobs = Configuration.GetValue<string>("AuthenticationServer:NightJobClients");

            services.AddAuthorization(options =>
            {
                //Bypassing token authorization of APIs in non-prod environments
                if (!authRequired)
                {
                    options.DefaultPolicy = new AuthorizationPolicyBuilder()
                        .RequireAssertion(_ => true)
                        .Build();
                }

                options.AddPolicy("NightJobHeaderAuthPolicy",
                    policy => policy.Requirements.Add(new NightlyjobAuthRequirement("x-Nightjob", nightJobs)));
            });

            services.AddScoped<IAuthorizationHandler, NighlyJobHeaderAuthHandler>();
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
                    options.SwaggerEndpoint("/swagger/v2/swagger.json", "HRMS Projects Microservice V2");
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
            string authority = Configuration.GetSection("AuthenticationServer:Authority")?.Value;
            List<string> valid_audiences = Configuration.GetSection("AuthenticationServer:ValidAudiences")?.GetChildren()?.Where(x => !string.IsNullOrEmpty(x.Value.Trim())).Select(x => x.Value.Trim())?.ToList();
            var requireHttpsMetadata = false;

            if (!string.IsNullOrWhiteSpace(Configuration.GetSection("AuthenticationServer:RequireHttpsMetadata")?.Value))
            {
                requireHttpsMetadata = Convert.ToBoolean(Configuration.GetSection("AuthenticationServer:RequireHttpsMetadata")?.Value);
            }

            if (!string.IsNullOrWhiteSpace(authority) && valid_audiences?.Count() > 0)
            {
                AuthenticationRequired = true;

                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                        {
                            options.Authority = authority;
                            options.RequireHttpsMetadata = requireHttpsMetadata;
                            options.TokenValidationParameters = new TokenValidationParameters { ValidateIssuer = true, ValidAudiences = valid_audiences };
                        });
            }
        }
    }
}
