using AutoMapper;
using CarMNS.Application.AutoMapper;
using CarMNS.Application.Implementation;
using CarMNS.Application.Interfaces;
using CarMNS.Data.EF;
using CarMNS.Data.EF.Repositories;
using CarMNS.Data.Entities;
using CarMNS.Data.IRepositories;
using CarMNS.Authorization;
using CarMNS.Extensions;
using CarMNS.Helpers;
using CarMNS.HostedService;
using CarMNS.Infrastructure.Interfaces;
using CarMNS.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IEmailSender = CarMNS.Services.IEmailSender;

namespace CarMNS
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
            services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"), o => o.MigrationsAssembly("CarMNS.Data.EF")));

            services.AddIdentity<APP_USER, APP_ROLE>().AddEntityFrameworkStores<AppDBContext>().AddDefaultTokenProviders();

            // Configure Identity
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            // ASP.NET Core hosting
            //services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            services.AddSingleton(AutoMapperConfig.RegisterMappings().CreateMapper());
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<UserManager<APP_USER>, UserManager<APP_USER>>();
            services.AddScoped<RoleManager<APP_ROLE>, RoleManager<APP_ROLE>>();

            var emailConfig = Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddTransient<DBInitializer>();

            services.AddScoped<IUserClaimsPrincipalFactory<APP_USER>, CustomClaimsPrincipalFactory>();

            // Unit of work and repository
            services.AddTransient(typeof(IUnitOfWork), typeof(EFUnitOfWork));
            services.AddTransient(typeof(IRespository<,>), typeof(EFRepository<,>));

            // Service
            services.AddTransient<IFunctionService, FunctionService>();
            services.AddTransient<IRoleAndPermisstionService, RoleAndPermisstionService>();
            services.AddTransient<IAuthorizationHandler, BaseResourceAuthorizationHandler>();
            services.AddTransient<IDriverAndCarService, DriverAndCarService>();
            services.AddTransient<IDangKyXeService, DangKyXeService>();
            //services.AddAntiforgery(o => o.SuppressXFrameOptionsHeader = true);
            services.AddMvc().AddJsonOptions(options => {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();// not change format json
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            // using for schedule syncfusion
            services.AddMvc()
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .AddNewtonsoftJson(opt => opt.SerializerSettings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat)
                .AddNewtonsoftJson(opt => opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local);

            //services.AddMvc().AddRazorPagesOptions(o => {
            //    o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
            //});

            services.Configure<FormOptions>(x =>
            {
                x.MultipartBodyLengthLimit = 209715200;
            });

            //services.AddHostedService<MyBackgroundService>();

            services.AddMemoryCache();

            services.AddRazorPages().AddSessionStateTempDataProvider();
            services.AddControllersWithViews().AddSessionStateTempDataProvider();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(8);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddMinResponse();

            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
                options.MaxRequestBodySize = 209715200;
            });

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix,opts=> { opts.ResourcesPath = "Resources"; })
                .AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var cultures = new List<CultureInfo> {new CultureInfo("vi-VN"),new CultureInfo("ko-KR")
                };
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("vi-VN");
                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
            });

            //string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            //services.AddCors(options =>
            //{
            //    options.AddPolicy(MyAllowSpecificOrigins,
            //                      policy =>
            //                      {
            //                          policy.WithOrigins(".")
            //                                              .AllowAnyHeader()
            //                                              .AllowAnyMethod();
            //                      });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/hrms-{Date}.txt");
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt/QHNqVVhkW1pFdEBBXHxAd1p/VWJYdVt5flBPcDwsT3RfQF9iSXxQdERgWntccXdRRA==;Mgo+DSMBPh8sVXJ0S0V+XE9AcVRDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS3xSdkdgW35dc3BXR2VfUw==;ORg4AjUWIQA/Gnt2VVhjQlFaclhJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxRd0VhWn1fc3dRRGFaWUY=;NzQ2MjA0QDMyMzAyZTMzMmUzMEVXeUJ5MzVHOUNrRVZBK3lDUWtsV3VzVFQrTzg3eUh3VkcxeVRlZlA4M289;NzQ2MjA1QDMyMzAyZTMzMmUzMGZqcnVLY2dnRDFjY3JGcU54Q3lNbDNuenJ0NFQ0SE0wNnI0QU9qM01WRDg9;NRAiBiAaIQQuGjN/V0Z+X09EaFtFVmJLYVB3WmpQdldgdVRMZVVbQX9PIiBoS35RdERjW3xccXdWQmJeUkN0;NzQ2MjA3QDMyMzAyZTMzMmUzMGVUMHVkYUs0blBNME9lUEEwakVaaGlyeDNCVWV4RzhlcS8yZm9xMS9QaU09;NzQ2MjA4QDMyMzAyZTMzMmUzMEsrd3lNRGxlRUZ3TmZDNUI1dlBlK1FyWnhycFA2bFpuYkRzR3lkR1RFYkk9;Mgo+DSMBMAY9C3t2VVhjQlFaclhJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxRd0VhWn1fc3dRRGJVWUY=;NzQ2MjEwQDMyMzAyZTMzMmUzMGFWY0NaTlhGSUNuWGlFYUlteUsrd3F0SlJqSUV5Z2VQUUwwMWtJS3NNTmc9;NzQ2MjExQDMyMzAyZTMzMmUzMFUrMUhmZnJFRUdRbTgvQkdxMDhVTTdrZnFudzZhS0NWN083dGxYbm1UNDA9;NzQ2MjEyQDMyMzAyZTMzMmUzMGVUMHVkYUs0blBNME9lUEEwakVaaGlyeDNCVWV4RzhlcS8yZm9xMS9QaU09");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMinResponse();
            app.UseRouting();

            // app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            app.UseEndpoints(routes =>
            {
                routes.MapControllerRoute(
                    "default",
                    "{area:exists}/{controller=Login}/{action=Index}/{id?}");

                //routes.MapControllerRoute(
                //    "Deptdefault",
                //      "{area:exists}/{controller=Login}/{action=Index}/{id?}");

            });
        }
    }
}
