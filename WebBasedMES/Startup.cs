using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using WebBasedMES.Data.Models;
using WebBasedMES.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using WebBasedMES.Services.JwtAuth;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using WebBasedMES.Services.Repositories.SystemManage;
using WebBasedMES.Services.Repositories.BaseInfo;
using WebBasedMES.Services.Repositories.MoldManage;
using WebBasedMES.Services.Repositories.Bom;
using WebBasedMES.Services.Repositories.InAndOut;
using WebBasedMES.Services.Repositories.Lots;
using WebBasedMES.Services.Repositories.ProducePlanManage;
using WebBasedMES.Services.Repositories.Quality;
using WebBasedMES.Services.Repositories.ProcessManage;
using WebBasedMES.Services.Repositories.ProduceStatus;
using WebBasedMES.Services.Repositories.InspectionRepairManage;
using WebBasedMES.Services.Repositories.BarcodeManage;
using WebBasedMES.Services.Repositories.Monitor;
using WebBasedMES.Services.Repositories.FacilityManage;

namespace WebBasedMES
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            // In production, the React files will be served from this directory

            services.AddDbContext<Data.ApiDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("2022_TAEGEUK"));
            });

            services.AddTransient<ISystemManageRepository, SystemManageRepository>();
            services.AddTransient<IBaseInfoRepository, BaseInfoRepository>();
            services.AddTransient<IMoldRepository, MoldRepository>();

            //Lot
            services.AddTransient<ILotMngRepository, LotMngRepository>();
            services.AddTransient<ILotCountMngRepository, LotCountMngRepository>();
            //bom
            services.AddTransient<IBomRepository, BomRepository>();
            ////????????
            services.AddTransient<IOrderMngRepository, OrderMngRepository>();
            ////????????
            services.AddTransient<IOutStoreMngRepository, OutStoreMngRepository>();
            ////????????
            services.AddTransient<IStoreMngRepository, StoreMngRepository>();
            ////????????
            services.AddTransient<IOutOrderMngRepository, OutOrderMngRepository>();

            //services.AddTransient<IOrderMngRepository, OrderMngRepository>();
            //품질관리
            services.AddTransient<IFaultyMngRepository, FaultyMngRepository>();
            services.AddTransient<IImportCheckMngRepository, ImportCheckMngRepository>();
            services.AddTransient<IProcessCheckMngRepository, ProcessCheckMngRepository>();
            services.AddTransient<IOutOrderCheckMngRepository, OutOrderCheckMngRepository>();
            services.AddTransient<IStoreCheckDefectiveMngRepository, StoreCheckDefectiveMngRepository>();
            services.AddTransient<IProcessCheckDefectiveMngRepository, ProcessCheckDefectiveMngRepository>();
            services.AddTransient<IOutOrderCheckDefectiveMngRepository, OutOrderCheckDefectiveMngRepository>();
            services.AddTransient<IProductDefectiveMngRepository, ProductDefectiveMngRepository>();
            services.AddTransient<IVoltageCheckMngRepository, VoltageCheckMngRepository>();
            //품질관리

            //생산현황 모니터링
            services.AddTransient<IProcessProgressStatusMngRepository, ProcessProgressStatusMngRepository>();
            services.AddTransient<IFacilityWorkStatusMngRepository, FacilityWorkStatusMngRepository>();
            services.AddTransient<IOrderProduceStatusMngRepository, OrderProduceStatusMngRepository>();
            services.AddTransient<IProducePlanProduceStatusMngRepository, ProducePlanProduceStatusMngRepository>();
            services.AddTransient<IMoldStatusMngRepository, MoldStatusMngRepository>();
            services.AddTransient<IFacilityStatusMngRepository, FacilityStatusMngRepository>();
            //생산현황 모니터링


            services.AddTransient<IProducePlanRepository, ProducePlanRepository>();
            services.AddTransient<IWorkOrderRepository, WorkOrderRepository>();

            services.AddTransient<IProcessRepository, ProcessRepository>();
            services.AddTransient<IProcessStatusRepository, ProcessStatusRepository>();
            services.AddTransient<IInvenMngRepository, InvenMngRepository>();

            //점검 수리
            services.AddTransient<IInspectionManageRepository, InspectionManageRepository>();
            services.AddTransient<IRepairManageRepository, RepairManageRepository>();

            services.AddTransient<IPreventiveMaintenanceRepository, PreventiveMaintenanceRepository>();

            //설비관리
            services.AddTransient<IFacilityManageRepository, FacilityManageRepository>();


            //바코드 관리
            services.AddTransient<IBarcodeRepository, BarcodeRepository>();
            services.AddTransient<IMonitorManageRepository, MonitorManageRepository>();


            services.AddLogging();
            services.AddIdentity<ApplicationUser, IdentityRole>(opts =>
            {
                opts.Password.RequiredLength = 6;
                opts.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<Data.ApiDbContext>()
            .AddErrorDescriber<IdentityErrorDescriberkr>()
            .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider);


            //JWT LOGGING
            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));
            var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = false,

                ClockSkew = TimeSpan.Zero
            };

            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt => {
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = tokenValidationParameters;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web Based MES", Version = "v1" });
                c.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme.ToLowerInvariant(),
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.OperationFilter<AuthResponsesOperationFilter>();
            });

            

            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin", opt => opt.AllowAnyOrigin());

                options.AddPolicy("AllowAll",
                    builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
            });

            services.AddControllers().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                opt.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            });
            services.AddControllers().AddXmlSerializerFormatters().AddXmlDataContractSerializerFormatters();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("api", new OpenApiInfo
                {
                    Title = "Swagger Test",
                    Description = "SU-MES API Docs",
                    Contact = new OpenApiContact
                    {
                        Name = "SU-MES Api docs",
                        Email = string.Empty,
                        Url = new Uri("https://su-tech.co.kr")
                    }
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(b =>
            {
                b.AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(o => true)
                .AllowCredentials();
            });


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }


            app.UseFileServer();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            



            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllers();
            });

            //swagger test
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/api/swagger.json", "Api Docs");
                c.RoutePrefix = string.Empty;
            });

            //app.UseEndpoints(ep =>
            //{
            //    ep.MapControllerRoute(name: "default", pattern: "{controller=api}/{action=api}/{id?}");
            //    ep.MapRazorPages();
            //});

        }
    }

    internal class AuthResponsesOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var attributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                                .Union(context.MethodInfo.GetCustomAttributes(true));

            if (attributes.OfType<IAllowAnonymous>().Any())
            {
                return;
            }

            var authAttributes = attributes.OfType<IAuthorizeData>();

            if (authAttributes.Any())
            {
                operation.Responses["401"] = new OpenApiResponse { Description = "Unauthorized" };

                if (authAttributes.Any(att => !String.IsNullOrWhiteSpace(att.Roles) || !String.IsNullOrWhiteSpace(att.Policy)))
                {
                    operation.Responses["403"] = new OpenApiResponse { Description = "Forbidden" };
                }

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "BearerAuth",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            Array.Empty<string>()
                        }
                    }
                };
            }
        }


    }

}
