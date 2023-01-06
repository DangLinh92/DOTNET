using OPERATION_MNS.Data.EF.Configurations;
using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Data.Interfaces;
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

namespace OPERATION_MNS.Data.EF
{
    public class AppDBContext : IdentityDbContext<APP_USER, APP_ROLE, Guid>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }

        public virtual DbSet<APP_USER> AppUsers { get; set; }
        public virtual DbSet<APP_ROLE> AppRoles { get; set; }
        public virtual DbSet<FUNCTION> Functions { get; set; }
        public virtual DbSet<PERMISSION> Permissions { get; set; }
        public virtual DbSet<LANGUAGE> Languages { get; set; }
        public virtual DbSet<ACTUAL_DAILY_VIEW> ACTUAL_DAILY_VIEW { get; set; }
        public virtual DbSet<GOC_PLAN> GOC_PLAN { get; set; }
        public virtual DbSet<INVENTORY_ACTUAL> INVENTORY_ACTUAL { get; set; }
        public virtual DbSet<GOC_STANDAR_QTY> GOC_STANDAR_QTY { get; set; }
        public virtual DbSet<MATERIAL_TO_SAP> MATERIAL_TO_SAP { get; set; }
        public virtual DbSet<SETTING_ITEMS> SETTING_ITEMS { get; set; }
        public virtual DbSet<DATE_OFF_LINE> DATE_OFF_LINE { get; set; }
        public virtual DbSet<STAY_LOT_LIST> STAY_LOT_LIST { get; set; }
        public virtual DbSet<LEAD_TIME_WLP> LEAD_TIME_WLP { get; set; }
        public virtual DbSet<STAY_LOT_LIST_HISTORY> STAY_LOT_LIST_HISTORY { get; set; }
        public virtual DbSet<FAB_PLAN> FAB_PLAN { get; set; }
        public virtual DbSet<VIEW_CONTROL_CHART_MODEL> VIEW_CONTROL_CHART_MODEL { get; set; }
        public virtual DbSet<CTQ_SETTING> CTQ_SETTING { get; set; }
        public virtual DbSet<CTQ_EMAIL_RECEIV> CTQ_EMAIL_RECEIV { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("APP_USER_CLAIM").HasKey(x => x.Id);
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("APP_ROLE_CLAIM").HasKey(x => x.Id);
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("APP_USER_LOGIN").HasKey(x => x.UserId);
            builder.Entity<IdentityUserRole<Guid>>().ToTable("APP_USER_ROLE").HasKey(x => new { x.RoleId, x.UserId });
            builder.Entity<IdentityUserToken<Guid>>().ToTable("APP_USER_TOKEN").HasKey(x => x.UserId);
            #endregion

            builder.AddConfiguration(new FunctionConfiguration());
            builder.AddConfiguration(new PermissionConfiguration());
            builder.AddConfiguration(new ActualDailyViewConfiguration());
            builder.AddConfiguration(new GocPlanConfiguration());
            builder.AddConfiguration(new InventoryActualConfiguration());
            builder.AddConfiguration(new GocStandarQtyConfiguration());
            builder.AddConfiguration(new MaterialToSapConfiguration());
            builder.AddConfiguration(new SettingItemsConfiguration());
            builder.AddConfiguration(new DateOffLineConfiguration());
            builder.AddConfiguration(new YieldOfModelConfiguration());
            builder.AddConfiguration(new StayLotListConfiguration());
            builder.AddConfiguration(new LeadTimeConfiguration());
            builder.AddConfiguration(new StayLotListHistoryConfiguration());
            builder.AddConfiguration(new FABPLANConfiguration());
            builder.AddConfiguration(new CTQConfiguration());
            builder.AddConfiguration(new CTQSettingConfiguration());
            builder.AddConfiguration(new CtqEmailConfiguration());
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
                    }
                    changeOrAddedItem.DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            return base.SaveChanges();
        }
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDBContext>
    {
        public AppDBContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<AppDBContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            return new AppDBContext(builder.Options);
        }
    }
}
