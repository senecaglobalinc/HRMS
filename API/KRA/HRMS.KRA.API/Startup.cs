using AutoMapper;
using HRMS.KRA.Database;
using HRMS.KRA.Infrastructure;
using HRMS.KRA.Service;
using HRMS.KRA.Service.External;
using HRMS.KRA.Types;
using HRMS.KRA.Types.External;
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HRMS.KRA.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                             .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                             .AddJsonFile("appsettings.qa.json", optional: true, reloadOnChange: true)
                             .AddJsonFile("appsettings.uat.json", optional: true, reloadOnChange: true)
                             .AddJsonFile("MailSubjects.json", optional: false, reloadOnChange: false);
            Configuration = builder.Build();
        }
        public bool AuthenticationRequired { get; set; }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
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
            services.AddHttpClient("OrgClient").AddHeaderPropagation();

            //Mail Subjects
            services.Configure<KRAMailSubjects>(options => Configuration.GetSection("KRAMailSubjects").Bind(options));

            services.AddHttpClient();

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

            services.AddAutoMapper(typeof(Startup));

            services.AddDbContext<KRAContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("Default"));
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IAspectService, AspectService>();
            services.AddTransient<IScaleService, ScaleService>();
            services.AddTransient<IMeasurementTypeService, MeasurementTypeService>();
            services.AddTransient<IKRAService, KRAService>();
            services.AddTransient<IOrganizationService, OrganizationService>();
            services.AddTransient<IRoleTypeService, RoleTypeService>();
            services.AddTransient<IStatusService, StatusService>();
            services.AddTransient<KRAContext>();
            services.AddTransient<IDefinitionService, DefinitionService>();
            services.AddTransient<IKRAWorkFlowService, KRAWorkFlowService>();
            services.AddTransient<IEmployeeService, EmployeeService>();

            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc(
                   "v2",
                    new OpenApiInfo
                    {
                        Title = "HRMS KRA Microservice",
                        Version = "v2",
                        Description = "A Microservice to perform KRA related operations",
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

            services.AddAuthorization(options =>
            {
                //Bypassing token authorization of APIs in non-prod environments
                if (!authRequired)
                {
                    options.DefaultPolicy = new AuthorizationPolicyBuilder()
                        .RequireAssertion(_ => true)
                        .Build();
                }
            });
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
                    options.SwaggerEndpoint("/swagger/v2/swagger.json", "HRMS KRA Microservice V2");
                    options.RoutePrefix = string.Empty;
                });
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
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
            List<string> valid_audiences = Configuration.GetSection("AuthenticationServer:ValidAudiences")?.GetChildren()?.Where(x => !string.IsNullOrEmpty(x.Value.Trim())).Select(x => x.Value.Trim())?.ToList();
            var requireHttpsMetadata = false;

            if (!string.IsNullOrWhiteSpace(Configuration.GetSection("AuthenticationServer")
                                                            .GetSection("RequireHttpsMetadata").Value))
            {
                requireHttpsMetadata = Convert.ToBoolean(Configuration.GetSection("AuthenticationServer")
                                                            .GetSection("RequireHttpsMetadata").Value);
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
