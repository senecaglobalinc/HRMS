using AutoMapper;
using HRMS.Admin.API.Handlers;
using HRMS.Admin.Database;
using HRMS.Admin.Infrastructure;
using HRMS.Admin.Service;
using HRMS.Admin.Service.External;
using HRMS.Admin.Types;
using HRMS.Admin.Types.External;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HRMS.Admin.API
{
    public class Startup
    {
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
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<APIEndPoints>(Configuration.GetSection("APIEndPoints"));
            services.Configure<EmailConfigurations>(Configuration.GetSection("EmailConfigurations"));
            services.Configure<MiscellaneousSettings>(Configuration.GetSection("MiscellaneousSettings"));
            services.AddHeaderPropagation(o =>
            {
                o.Headers.Add("Authorization");
                o.Headers.Add("UserName");
            });
            services.AddHttpClient("EmployeeClient").AddHeaderPropagation();
            services.AddHttpClient("ProjectClient").AddHeaderPropagation();
            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                        options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();

                    });
            ConfigureAuthentication(services);
            //services.AddControllers()
            //        .AddJsonOptions(option =>
            //        {
            //            option.JsonSerializerOptions.PropertyNamingPolicy = null;
            //            option.JsonSerializerOptions.MaxDepth = 512;
            //        });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowMyOrigin",
                builder => builder.WithOrigins("*")
                                    .AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddDbContext<AdminContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("Default"));
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ICategoryMasterService, CategoryMasterService>();
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<ICompetencyAreaService, CompetencyAreaService>();
            services.AddTransient<IDepartmentService, DepartmentService>();
            services.AddTransient<IDepartmentTypeService, DepartmentTypeService>();
            services.AddTransient<IDesignationService, DesignationService>();
            services.AddTransient<IDomainService, DomainService>();
            services.AddTransient<IGradeService, GradeService>();
            services.AddTransient<IKeyFunctionService, KeyFunctionService>();
            services.AddTransient<INotificationConfigurationService, NotificationConfigurationService>();
            services.AddTransient<INotificationTypeService, NotificationTypeService>();
            services.AddTransient<INotificationManagerService, NotificationManagerService>();
            services.AddTransient<IPracticeAreaService, PracticeAreaService>();
            services.AddTransient<IProficiencyLevelService, ProficiencyLevelService>();
            services.AddTransient<IProjectTypeService, ProjectTypeService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<ISeniorityService, SeniorityService>();
            services.AddTransient<ISkillGroupService, SkillGroupService>();
            services.AddTransient<ISkillService, SkillService>();
            services.AddTransient<ISpecialityService, SpecialityService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRoleService, UserRoleService>();
            services.AddTransient<IStatusService, StatusService>();
            services.AddTransient<IRoleTypeService, RoleTypeService>();
            services.AddTransient<IFinancialYearService, FinancialYearService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IMenuService, MenuService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IActivityService, ActivityService>();
            services.AddTransient<IServiceTypeService, ServiceTypeService>();
            services.AddTransient<IExitTypeService, ExitTypeService>();
            services.AddTransient<IReasonService, ReasonService>();
            services.AddTransient<IGradeRoleTypeService, GradeRoleTypeService>();
            services.AddTransient<IHolidayService, HolidayService>();
            services.AddTransient<AdminContext>();

            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc(
                   "v2",
                    new OpenApiInfo
                    {
                        Title = "HRMS Admin Microservice",
                        Version = "v2",
                        Description = "A Microservice to perform Admin/Master realed operations",
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
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //setup.IncludeXmlComments(xmlPath);
            });

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
                    options.SwaggerEndpoint("/swagger/v2/swagger.json", "HRMS Admin Microservice V2");
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
