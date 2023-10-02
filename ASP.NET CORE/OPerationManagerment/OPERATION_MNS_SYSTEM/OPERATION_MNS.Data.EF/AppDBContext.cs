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
        public virtual DbSet<CTQ_EMAIL_RECEIV_WLP2> CTQ_EMAIL_RECEIV_WLP2 { get; set; }
        public virtual DbSet<POST_OPERATION_SHIPPING> POST_OPERATION_SHIPPING { get; set; }
        public virtual DbSet<VIEW_WIP_POST_WLP> VIEW_WIP_POST_WLP { get; set; }
        public virtual DbSet<SMT_RETURN_WLP2> SMT_RETURN_WLP2 { get; set; }
        public virtual DbSet<STAY_LOT_LIST_WLP2> STAY_LOT_LIST_WLP2 { get; set; }
        public virtual DbSet<STAY_LOT_LIST_HISTORY_WLP2> STAY_LOT_LIST_HISTORY_WLP2 { get; set; }
        public virtual DbSet<BOPHAN_DE_NGHI_XUAT_NLIEU> BOPHAN_DE_NGHI_XUAT_NLIEU { get; set; }
        public virtual DbSet<OUTGOING_RECEIPT_WLP2> OUTGOING_RECEIPT_WLP2 { get; set; }
        public virtual DbSet<KHUNG_THOI_GIAN_XUAT_HANG_WLP2> KHUNG_THOI_GIAN_XUAT_HANG_WLP2 { get; set; }
        public virtual DbSet<STAY_LOT_LIST_PRIORY_WLP2> STAY_LOT_LIST_PRIORY_WLP2 { get; set; }
        public virtual DbSet<DAILY_PLAN_WLP2> DAILY_PLAN_WLP2 { get; set; }
        public virtual DbSet<VIEW_CONTROL_CHART_MODEL_WLP2> VIEW_CONTROL_CHART_MODEL_WLP2 { get; set; }
        public virtual DbSet<CTQ_SETTING_WLP2> CTQ_SETTING_WLP2 { get; set; }
        public virtual DbSet<THICKNET_MODEL_WLP2> THICKNET_MODEL_WLP2 { get; set; }

        // sample
        public virtual DbSet<TINH_HINH_SAN_XUAT_SAMPLE> TINH_HINH_SAN_XUAT_SAMPLE { get; set; }
        public virtual DbSet<PHAN_LOAI_HANG_SAMPLE> PHAN_LOAI_HANG_SAMPLE { get; set; }
        public virtual DbSet<PHAN_LOAI_MODEL_SAMPLE> PHAN_LOAI_MODEL_SAMPLE { get; set; }
        public virtual DbSet<TCARD_SAMPLE> TCARD_SAMPLE { get; set; }
        public virtual DbSet<STAY_LOT_LIST_SAMPLE> STAY_LOT_LIST_SAMPLE { get; set; }
        public virtual DbSet<STAY_LOT_LIST_HISTORY_SAMPLE> STAY_LOT_LIST_HISTORY_SAMPLE { get; set; }
        public virtual DbSet<DATE_OFF_LINE_SAMPLE> DATE_OFF_LINE_SAMPLE { get; set; }
        public virtual DbSet<GOC_PLAN_LFEM> GOC_PLAN_LFEM { get; set; }
        public virtual DbSet<DATE_OFF_LINE_LFEM> DATE_OFF_LINE_LFEM { get; set; }
        public virtual DbSet<OUT_PUT_SHIPPING_LOT_LFEM> OUT_PUT_SHIPPING_LOT_LFEM { get; set; }
        public virtual DbSet<OUT_PUT_BY_LOT_LFEM> OUT_PUT_BY_LOT_LFEM { get; set; }
        public virtual DbSet<KHSX_ACTUAL_LFEM> KHSX_ACTUAL_LFEM { get; set; }
        public virtual DbSet<DAILY_PLAN_DATA_LFEM> DAILY_PLAN_DATA_LFEM { get; set; }
        public virtual DbSet<STAY_LOT_LIST_PRIORY_LFEM> STAY_LOT_LIST_PRIORY_LFEM { get; set; }
        public virtual DbSet<LOT_TEST_HISTOTY_LFEM> LOT_TEST_HISTOTY_LFEM { get; set; }
        public virtual DbSet<STAY_LOT_LIST_LFEM> STAY_LOT_LIST_LFEM { get; set; }
        public virtual DbSet<STAY_LOT_LIST_HISTORY_LFEM> STAY_LOT_LIST_HISTORY_LFEM { get; set; }
        public virtual DbSet<ACTUAL_PLAN_SAMPLE> ACTUAL_PLAN_SAMPLE { get; set; }
        public virtual DbSet<WARNING_LOT_RUN_LFEM> WARNING_LOT_RUN_LFEM { get; set; }
        public virtual DbSet<WARNING_LOT_RUNTIME_LFEM> WARNING_LOT_RUNTIME_LFEM { get; set; }
        public virtual DbSet<CAPA_LFEM_DATA> CAPA_LFEM_DATA { get; set; }

        // SMT
        public virtual DbSet<GOC_PLAN_SMT> GOC_PLAN_SMT { get; set; }
        public virtual DbSet<OUT_PUT_BY_LOT_SMT> OUT_PUT_BY_LOT_SMT { get; set; }
        public virtual DbSet<DATE_OFF_LINE_SMT> DATE_OFF_LINE_SMT { get; set; }
        public virtual DbSet<LEAD_TIME_LFEM> LEAD_TIME_LFEM { get; set; }
        public virtual DbSet<VIEW_WIP_LOT_LIST_LFEM> VIEW_WIP_LOT_LIST_LFEM { get; set; }

        // GOC
        public virtual DbSet<SALES_APPROVE_MANUFATURE> SALES_APPROVE_MANUFATURE { get; set; }
        public virtual DbSet<OPERATION_STANDARD_INFO> OPERATION_STANDARD_INFO { get; set; }
        public virtual DbSet<PRODUCT_MIX_CAPA> PRODUCT_MIX_CAPA { get; set; }
        public virtual DbSet<SCP_PLAN_BOM> SCP_PLAN_BOM { get; set; }
        public virtual DbSet<OPERATION_FLOW> OPERATION_FLOW { get; set; }
        public virtual DbSet<SITE_CALENDAR> SITE_CALENDAR { get; set; }
        public virtual DbSet<MATERIALS_PLAN> MATERIALS_PLAN { get; set; }
        public virtual DbSet<PRODUCTION_PLAN> PRODUCTION_PLAN { get; set; }
        public virtual DbSet<GOC_PRODUCTION_PLAN_LFEM> GOC_PRODUCTION_PLAN_LFEM { get; set; }
        public virtual DbSet<GOC_PRODUCTION_PLAN_LFEM_UPDATE> GOC_PRODUCTION_PLAN_LFEM_UPDATE { get; set; }
        public virtual DbSet<PLAN_RANGE_TIME> PLAN_RANGE_TIME { get; set; }

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
            builder.AddConfiguration(new PostOperationShippingConfiguration());
            builder.AddConfiguration(new GocPlanWLP2Configuration());
            builder.AddConfiguration(new ViewWipPostConfiguration());
            builder.AddConfiguration(new SmtReturnConfiguration());
            builder.AddConfiguration(new StayLotListWlp2Configuration());
            builder.AddConfiguration(new StayLotListHistoryWlp2Configuration());
            builder.AddConfiguration(new BoPhanDeNghiXuatConfiguration());
            builder.AddConfiguration(new OutGoingReceipConfiguration());
            builder.AddConfiguration(new KhungThoiGianXuatConfiguration());
            builder.AddConfiguration(new StayLotListPrioryConfiguration());
            builder.AddConfiguration(new DailyPlanWlp2Configuration());
            builder.AddConfiguration(new CTQWLP2Configuration());
            builder.AddConfiguration(new CTQSettingWlp2Configuration());
            builder.AddConfiguration(new ThickNetModelWlp2Configuration());
            builder.AddConfiguration(new CtqEmailwlp2Configuration());

            // sample
            builder.AddConfiguration(new TinhHinhSanXuatSampleConfiguration());
            builder.AddConfiguration(new PhanLoaiHangSampleConfiguration());
            builder.AddConfiguration(new PhanLoaiModelSampleConfiguration());
            builder.AddConfiguration(new TCardSampleConfiguration());
            builder.AddConfiguration(new StayLotListSampleConfiguration());
            builder.AddConfiguration(new StayLotListHistorySample2Configuration());
            builder.AddConfiguration(new DateOffLineSampleConfiguration());
            builder.AddConfiguration(new DelayCommentSampleConfiguration());
            builder.AddConfiguration(new ActualPlanSampleConfiguration());

            // lfem
            builder.AddConfiguration(new GocPlanLfemConfiguration());
            builder.AddConfiguration(new DateOffLineLFEMConfiguration());
            builder.AddConfiguration(new OutPutByLotLFEMConfiguration());
            builder.AddConfiguration(new OutPutShippingLFEMConfiguration());
            builder.AddConfiguration(new KHSXActualLFEMConfiguration());
            builder.AddConfiguration(new DailyPlanDataLFEMConfiguration());
            builder.AddConfiguration(new StayLotListPrioryLfemConfiguration());
            builder.AddConfiguration(new LotTestHistoryLfemConfiguration());
            builder.AddConfiguration(new StayLotListLfemConfiguration());
            builder.AddConfiguration(new StayLotListHistoryLfemConfiguration());
            builder.AddConfiguration(new WarningLotRunLfemConfiguration());
            builder.AddConfiguration(new WarningLotRunTimeLfemConfiguration());
            builder.AddConfiguration(new LeadTimeLfemConfiguration());
            builder.AddConfiguration(new VIEWWIPLotListLfemConfiguration());
            builder.AddConfiguration(new CapaLfemConfiguration());

            // smt
            builder.AddConfiguration(new GocPlanSMTConfiguration());
            builder.AddConfiguration(new OutputByLotSMTConfiguration());
            builder.AddConfiguration(new DateOffLineSMTConfiguration());

            // GOC
            builder.AddConfiguration(new SaleApproveManufatureConfiguration());
            builder.AddConfiguration(new OperationStandardConfiguration());
            builder.AddConfiguration(new ProductMixCapaConfiguration());
            builder.AddConfiguration(new SCPPLanBomConfiguration());
            builder.AddConfiguration(new OperationFlowConfiguration());
            builder.AddConfiguration(new SizeCalendarConfiguration());
            builder.AddConfiguration(new MaterialPlanConfiguration());
            builder.AddConfiguration(new ProductionPlanConfiguration());
            builder.AddConfiguration(new GocProductionPlanLfemConfiguration());
            builder.AddConfiguration(new GocProductionPlanUpdateLfemConfiguration());
            builder.AddConfiguration(new PlanRangeTimeConfiguration());
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
