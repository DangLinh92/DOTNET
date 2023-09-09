using CarMNS.Data.EF.Configurations;
using CarMNS.Data.EF.Extensions;
using CarMNS.Data.Entities;
using CarMNS.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace CarMNS.Data.EF
{
    public class AppDBContext : IdentityDbContext<APP_USER, APP_ROLE, Guid, IdentityUserClaim<Guid>
    , APP_USER_ROLE
    , IdentityUserLogin<Guid>
    , IdentityRoleClaim<Guid>
    , IdentityUserToken<Guid>>
    {
        public IHttpContextAccessor _httpContextAccessor;
        public AppDBContext(DbContextOptions<AppDBContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual DbSet<APP_USER_TOKEN> AppUserTokens { get; set; }
        public virtual DbSet<APP_USER> AppUsers { get; set; }
        public virtual DbSet<APP_ROLE> AppRoles { get; set; }
        public virtual DbSet<APP_USER_ROLE> AppUserRoles { get; set; }

        public virtual DbSet<FUNCTION> Functions { get; set; }
        public virtual DbSet<PERMISSION> Permissions { get; set; }
        public virtual DbSet<CAR> CAR { get; set; }
        public virtual DbSet<LAI_XE> LAI_XE { get; set; }
        public virtual DbSet<LAI_XE_CAR> LAI_XE_CAR { get; set; }
        public virtual DbSet<DIEUXE_DANGKY> DIEUXE_DANGKY { get; set; }
        public virtual DbSet<DANG_KY_XE> DANG_KY_XE { get; set; }
        public virtual DbSet<DANG_KY_XE_TAXI> DANG_KY_XE_TAXI { get; set; }
        public virtual DbSet<BOPHAN_DUYET> BOPHAN_DUYET { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("APP_USER_CLAIM").HasKey(x => x.Id);
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("APP_ROLE_CLAIM").HasKey(x => x.Id);
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("APP_USER_LOGIN").HasKey(x => x.UserId);
            //builder.Entity<IdentityUserRole<Guid>>().ToTable("APP_USER_ROLE").HasKey(x => new { x.RoleId, x.UserId });
            builder.Entity<IdentityUserToken<Guid>>().ToTable("APP_USER_TOKEN").HasKey(x => new { x.UserId, x.LoginProvider, x.Name });
            #endregion

            builder.AddConfiguration(new UserConfiguration());
            builder.AddConfiguration(new UserRoleConfiguration());
            builder.AddConfiguration(new FunctionConfiguration());
            builder.AddConfiguration(new PermissionConfiguration());
            builder.AddConfiguration(new CarConfiguration());
            builder.AddConfiguration(new LaixeConfiguration());
            builder.AddConfiguration(new LaixeCarConfiguration());
            builder.AddConfiguration(new DangKyXeConfiguration());
            builder.AddConfiguration(new DieuXeDangKyXeConfiguration());
            builder.AddConfiguration(new BoPhanConfiguration());
            builder.AddConfiguration(new DangKyXeTaxiConfiguration());
            builder.AddConfiguration(new BoPhanDuyetConfiguration());

            //base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            var modified = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);
            foreach (EntityEntry item in modified)
            {
                var changeOrAddedItem = item.Entity as IDateTracking;
                if (changeOrAddedItem != null)
                {
                    if (item.State == EntityState.Added)
                    {
                        changeOrAddedItem.DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        if (_httpContextAccessor.HttpContext != null && changeOrAddedItem.UserCreated.NullString() == "")
                            changeOrAddedItem.UserCreated = _httpContextAccessor.HttpContext.User?.Identity?.Name;
                    }

                    changeOrAddedItem.DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    if (_httpContextAccessor.HttpContext != null && changeOrAddedItem.UserModified.NullString() == "")
                        changeOrAddedItem.UserModified = _httpContextAccessor.HttpContext.User?.Identity?.Name;
                }
            }
            return base.SaveChanges();
        }
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDBContext>
    {
        public DesignTimeDbContextFactory()
        {

        }

        public IHttpContextAccessor _httpContextAccessor;
        public DesignTimeDbContextFactory(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public AppDBContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<AppDBContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            return new AppDBContext(builder.Options, _httpContextAccessor);
        }
    }
}
