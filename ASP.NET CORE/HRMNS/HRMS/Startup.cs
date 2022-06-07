using AutoMapper;
using HRMNS.Application.AutoMapper;
using HRMNS.Application.Implementation;
using HRMNS.Application.Interfaces;
using HRMNS.Data.EF;
using HRMNS.Data.EF.Repositories;
using HRMNS.Data.Entities;
using HRMNS.Data.IRepositories;
using HRMS.Authorization;
using HRMS.Helpers;
using HRMS.HostedService;
using HRMS.Infrastructure.Interfaces;
using HRMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS
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
                    Configuration.GetConnectionString("DefaultConnection"), o => o.MigrationsAssembly("HRMNS.Data.EF")));

            services.AddDbContext<BioStarDBContext>(option => option.UseSqlServer(@"Data Source = 10.70.22.240;Initial Catalog = BioStar;User Id = sa;Password = qwe123!@#;Connect Timeout=3"));

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

            services.AddSingleton(AutoMapperConfig.RegisterMappings().CreateMapper());
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<UserManager<APP_USER>, UserManager<APP_USER>>();
            services.AddScoped<RoleManager<APP_ROLE>, RoleManager<APP_ROLE>>();

            //services.AddSingleton(Mapper.Configuration);
            //services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));
            services.AddTransient<Services.IEmailSender, EmailSender>();

            services.AddTransient<DBInitializer>();

            services.AddScoped<IUserClaimsPrincipalFactory<APP_USER>, CustomClaimsPrincipalFactory>();

            // Unit of work and repository
            services.AddTransient(typeof(IUnitOfWork), typeof(EFUnitOfWork));
            services.AddTransient(typeof(IRespository<,>), typeof(EFRepository<,>));

            // Service
            services.AddTransient<INhanVienService, NhanVienService>();
            services.AddTransient<IFunctionService, FunctionService>();
            services.AddTransient<IBoPhanService, BoPhanService>();
            services.AddTransient<IChucDanhService, ChucDanhService>();
            services.AddTransient<IBoPhanDetailService, BoPhanDetailService>();
            services.AddTransient<IBHXHService, BHXHService>();
            services.AddTransient<IPhepNamService, PhepNamService>();
            services.AddTransient<ITinhTrangHoSoService, TinhTrangHoSoService>();
            services.AddTransient<IHRLoaiHopDongService, HRLoaiHopDongService>();
            services.AddTransient<IHopDongService, HopDongService>();
            services.AddTransient<IQuatrinhLamViecService, QuatrinhLamViecService>();
            services.AddTransient<ICheDoBHService, CheDoBHService>();
            services.AddTransient<IKeKhaiBaoHiemService, KeKhaiBaoHiemService>();
            services.AddTransient<IChamCongService, ChamCongService>();
            services.AddTransient<INhanVien_CalamviecService, NhanVien_CalamviecService>();
            services.AddTransient<IDMucCalamviecService, DMucCalamviecService>();
            services.AddTransient<ISettingTimeCalamviecService, SettingTimeCalamviecService>();
            services.AddTransient<IOvertimeService, OvertimeService>();
            services.AddTransient<INgayLeNamService, NgayLeNamService>();
            services.AddTransient<IDMucNgaylamviecService, DMucNgaylamviecService>();

            services.AddTransient<IDangKyChamCongChiTietService, DangKyChamCongChiTietService>();
            services.AddTransient<IDangKyChamCongDacBietService, DangKyChamCongDacBietService>();
            services.AddTransient<IBangCongService, BangCongService>();

            services.AddTransient<IDM_DCChamCongService, DM_DCChamCongService>();
            services.AddTransient<IDCChamCongService, DCChamCongService>();
            services.AddTransient<IRoleAndPermisstionService, RoleAndPermisstionService>();
            services.AddTransient<IAuthorizationHandler, BaseResourceAuthorizationHandler>();
            services.AddTransient<IBackgroundService, HmrsBackgroundService>();
            services.AddMvc().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();// not change format json
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.Configure<FormOptions>(x =>
            {
                x.MultipartBodyLengthLimit = 209715200;
            });

            services.AddHostedService<MyBackgroundService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/hrms-{Date}.txt");
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(routes =>
            {

                routes.MapControllerRoute(
                    "default",
                    "{area:exists}/{controller=Login}/{action=Index}/{id?}");

                //routes.MapControllerRoute(
                //    "Deptdefault",
                //    "{controller=Account}/{action=Login}/{id?}");

            });
        }
    }
}
