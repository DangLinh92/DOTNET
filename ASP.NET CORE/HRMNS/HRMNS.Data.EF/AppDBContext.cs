using HRMNS.Data.EF.Configurations;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMNS.Data.Interfaces;
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

namespace HRMNS.Data.EF
{
    public class AppDBContext : IdentityDbContext<APP_USER, APP_ROLE, Guid>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }
        public virtual DbSet<APP_USER> AppUsers { get; set; }
        public virtual DbSet<APP_ROLE> AppRoles { get; set; }
        public virtual DbSet<BOPHAN> BoPhans { get; set; }
        public virtual DbSet<HR_BHXH> HrBHXH { get; set; }
        public virtual DbSet<HR_CHEDOBH> HrCheDoBH { get; set; }
        public virtual DbSet<HR_CHUCDANH> HrChucDanh { get; set; }
        public virtual DbSet<CHUNG_CHI> HrChungChi { get; set; }
        public virtual DbSet<HR_HOPDONG> HrHopDong { get; set; }
        public virtual DbSet<HR_KEKHAIBAOHIEM> HrKeKhaiBaoHiem { get; set; }
        public virtual DbSet<HR_LOAIHOPDONG> HrLoaiHopDong { get; set; }
        public virtual DbSet<HR_NHANVIEN> HrNhanVien { get; set; }
        public virtual DbSet<HR_QUATRINHLAMVIEC> HrQuaTrinhLamViec { get; set; }
        public virtual DbSet<HR_TINHTRANGHOSO> HrTinhTrangHoSo { get; set; }
        public virtual DbSet<LOAICHUNGCHI> LoaiChungChis { get; set; }
        public virtual DbSet<FUNCTION> Functions { get; set; }
        public virtual DbSet<HR_CHUNGCHI_NHANVIEN> HrChungChiNhanVien { get; set; }
        public virtual DbSet<PERMISSION> Permissions { get; set; }
        public virtual DbSet<LANGUAGE> Languages { get; set; }
        public virtual DbSet<HR_PHEP_NAM> HrPhepNam { get; set; }
        public virtual DbSet<HR_BO_PHAN_DETAIL> HrBoPhanDetail { get; set; }

        public virtual DbSet<CA_LVIEC> CA_LVIEC { get; set; }
        public virtual DbSet<CHAM_CONG_LOG> CHAM_CONG_LOG { get; set; }
        public virtual DbSet<DANGKY_CHAMCONG_CHITIET> DANGKY_CHAMCONG_CHITIET { get; set; }
        public virtual DbSet<DANGKY_CHAMCONG_DACBIET> DANGKY_CHAMCONG_DACBIET { get; set; }
        public virtual DbSet<DANGKY_OT_NHANVIEN> DANGKY_OT_NHANVIEN { get; set; }
        public virtual DbSet<DC_CHAM_CONG> DC_CHAM_CONG { get; set; }
        public virtual DbSet<DM_CA_LVIEC> DM_CA_LVIEC { get; set; }
        public virtual DbSet<DM_DANGKY_CHAMCONG> DM_DANGKY_CHAMCONG { get; set; }
        public virtual DbSet<DM_NGAY_LAMVIEC> DM_NGAY_LAMVIEC { get; set; }
        public virtual DbSet<KY_HIEU_CHAM_CONG> KY_HIEU_CHAM_CONG { get; set; }
        public virtual DbSet<NGAY_LE_NAM> NGAY_LE_NAM { get; set; }
        public virtual DbSet<NGAY_NGHI_BU_LE_NAM> NGAY_NGHI_BU_LE_NAM { get; set; }
        public virtual DbSet<NHANVIEN_CALAMVIEC> NHANVIEN_CALAMVIEC { get; set; }
        public virtual DbSet<SETTING_TIME_DIMUON_VESOM> SETTING_TIME_DIMUON_VESOM { get; set; }
        public virtual DbSet<TRU_SO_LVIEC> TRU_SO_LVIEC { get; set; }
        public virtual DbSet<ATTENDANCE_RECORD> ATTENDANCE_RECORD { get; set; }
        public virtual DbSet<ATTENDANCE_OVERTIME> ATTENDANCE_OVERTIME { get; set; }
        // public virtual DbSet<SETTING_TIME_CA_LVIEC> SETTING_TIME_CA_LVIEC { get; set; }
        public virtual DbSet<NGAY_DAC_BIET> NGAY_DAC_BIET { get; set; }
        public virtual DbSet<DM_DIEUCHINH_CHAMCONG> DM_DIEUCHINH_CHAMCONG { get; set; }
        public virtual DbSet<HR_THAISAN_CONNHO> HR_THAISAN_CONNHO { get; set; }

        public virtual DbSet<EHS_DM_KEHOACH> EHS_DM_KEHOACH { get; set; }
        //public virtual DbSet<EHS_DEMUC_KEHOACH> EHS_DEMUC_KEHOACH { get; set; }
        //public virtual DbSet<EHS_LUATDINH_DEMUC_KEHOACH> EHS_LUATDINH_DEMUC_KEHOACH { get; set; }
        public virtual DbSet<EHS_LUATDINH_KEHOACH> EHS_LUATDINH_KEHOACH { get; set; }
        //public virtual DbSet<EHS_NOIDUNG_KEHOACH> EHS_NOIDUNG_KEHOACH { get; set; }
        //public virtual DbSet<EHS_NOIDUNG> EHS_NOIDUNG { get; set; }

        //public virtual DbSet<EVENT_SHEDULE> EVENT_SHEDULE { get; set; }
        public virtual DbSet<EVENT_SHEDULE_PARENT> EVENT_SHEDULE_PARENT { get; set; }
        public virtual DbSet<HR_TRAINING> HR_TRAINING { get; set; }
        public virtual DbSet<TRAINING_NHANVIEN> TRAINING_NHANVIEN { get; set; }
        public virtual DbSet<TRAINING_TYPE> TRAINING_TYPE { get; set; }
        public virtual DbSet<HR_NHANVIEN_CHEDO_DB> HR_NHANVIEN_CHEDO_DB { get; set; }
        public virtual DbSet<FILE_MANAGER> FILE_MANAGER { get; set; }
        public virtual DbSet<SAMSUNG_TRAINING> SAMSUNG_TRAINING { get; set; }
        public virtual DbSet<DANGKY_DIMUON_VSOM_NHANVIEN> DANGKY_DIMUON_VSOM_NHANVIEN { get; set; }
        //public virtual DbSet<EHS_CHIPHI_BY_MONTH> EHS_CHIPHI_BY_MONTH { get; set; }
        public virtual DbSet<EHS_KEHOACH_QUANTRAC> EHS_KEHOACH_QUANTRAC { get; set; }
        public virtual DbSet<EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC> EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC { get; set; }
        public virtual DbSet<EHS_KE_HOACH_KHAM_SK> EHS_KE_HOACH_KHAM_SK { get; set; }
        public virtual DbSet<EHS_NHANVIEN_KHAM_SK> EHS_NHANVIEN_KHAM_SK { get; set; }
        public virtual DbSet<EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK> EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK { get; set; }


        public virtual DbSet<EHS_KEHOACH_DAOTAO_ANTOAN_VSLD> EHS_KEHOACH_DAOTAO_ANTOAN_VSLD { get; set; }
        public virtual DbSet<EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD> EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD { get; set; }

        public virtual DbSet<EHS_KEHOACH_PCCC> EHS_KEHOACH_PCCC { get; set; }
        public virtual DbSet<EHS_THOIGIAN_THUC_HIEN_PCCC> EHS_THOIGIAN_THUC_HIEN_PCCC { get; set; }

        public virtual DbSet<EHS_KEHOACH_ANTOAN_BUCXA> EHS_KEHOACH_ANTOAN_BUCXA { get; set; }
        public virtual DbSet<EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA> EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("APP_USER_CLAIM").HasKey(x => x.Id);
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("APP_ROLE_CLAIM").HasKey(x => x.Id);
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("APP_USER_LOGIN").HasKey(x => x.UserId);
            builder.Entity<IdentityUserRole<Guid>>().ToTable("APP_USER_ROLE").HasKey(x => new { x.RoleId, x.UserId });
            builder.Entity<IdentityUserToken<Guid>>().ToTable("APP_USER_TOKEN").HasKey(x => x.UserId);
            #endregion

            builder.AddConfiguration(new CheDoBHConfiguration());
            builder.AddConfiguration(new BaoHiemXHConfiguration());
            builder.AddConfiguration(new LoaiChungChiConfiguration());
            builder.AddConfiguration(new LoaiHopDongConfiguration());
            builder.AddConfiguration(new NhanVienConfiguration());
            builder.AddConfiguration(new BoPhanConfiguration());
            builder.AddConfiguration(new ChucDanhConfiguration());
            builder.AddConfiguration(new ChungChiNhanVienConfiguration());
            builder.AddConfiguration(new ChungChiConfiguration());
            builder.AddConfiguration(new FunctionConfiguration());
            builder.AddConfiguration(new HopDongConfiguration());
            builder.AddConfiguration(new KeKhaiBaoHiemConfiguration());
            builder.AddConfiguration(new LanguageConfiguration());
            builder.AddConfiguration(new PermissionConfiguration());
            builder.AddConfiguration(new QuatrinhlamviecConfiguration());
            builder.AddConfiguration(new TinhTrangHosoConfiguration());
            builder.AddConfiguration(new BoPhanDetailConfiguration());
            builder.AddConfiguration(new PhepNamConfiguration());

            builder.AddConfiguration(new CaLviecConfiguration());
            builder.AddConfiguration(new ChamCongLogConfiguration());
            builder.AddConfiguration(new DkyChamCongChiTietConfiguration());
            builder.AddConfiguration(new DkyChamCongDacBietConfiguration());
            builder.AddConfiguration(new DKyOTNhanVienConfiguration());
            builder.AddConfiguration(new DCChamCongConfiguration());
            builder.AddConfiguration(new DMCaLviecConfiguration());
            builder.AddConfiguration(new DMDangKyChamCongConfiguration());
            builder.AddConfiguration(new DMNgayLamViecConfiguration());
            builder.AddConfiguration(new KyHieuChamCongConfiguration());
            builder.AddConfiguration(new NgayLeNamConfiguration());
            builder.AddConfiguration(new NgayNghiBuLeNamConfiguration());
            builder.AddConfiguration(new NhanVienCaLamViecConfiguration());
            builder.AddConfiguration(new SettingTimeDiMuonVeSomConfiguration());
            builder.AddConfiguration(new TruSoLamViecConfiguration());
            builder.AddConfiguration(new AttendenceRecordConfiguration());
            builder.AddConfiguration(new AttendenceOvertimeConfiguration());
            // builder.AddConfiguration(new SettingTimeCaLamViecConfiguration());
            builder.AddConfiguration(new NgayDacBietConfiguration());
            builder.AddConfiguration(new DMDieuChinhChamCongConfiguration());
            builder.AddConfiguration(new NhanVienThaiSanConfiguration());

            builder.AddConfiguration(new EhsDMKeHoachConfiguration());
            // builder.AddConfiguration(new EhsDMKeHoachByYearConfiguration());
            //builder.AddConfiguration(new EhsDeMucKeHoachConfiguration());
            builder.AddConfiguration(new EhsLuatDinhKeHoachConfiguration());
            //builder.AddConfiguration(new EhsLuatDinhDeMucKeHoachConfiguration());
            //builder.AddConfiguration(new EhsNoiDungKeHoachConfiguration());
            //builder.AddConfiguration(new EhsNoiDungConfiguration());
            builder.AddConfiguration(new EventParentConfiguration());
            //builder.AddConfiguration(new EventConfiguration());

            builder.AddConfiguration(new TrainingConfigurationConfiguration());
            builder.AddConfiguration(new TrainingNhanVienConfiguration());
            builder.AddConfiguration(new TrainingTypeConfiguration());
            builder.AddConfiguration(new NhanVienCheDoDBConfiguration());
            builder.AddConfiguration(new FileManagementConfiguration());
            builder.AddConfiguration(new DKyELNhanVienConfiguration());
            //builder.AddConfiguration(new EhsChiPhiByMonthConfiguration());
            builder.AddConfiguration(new EHS_NoiDungKeHoachNewConfiguration());
            builder.AddConfiguration(new EHS_NgayThucHienChiTietConfiguration());

            builder.AddConfiguration(new EHS_NhanVienKhamSKConfiguration());
            builder.AddConfiguration(new EHS_KeHoachKhamSKConfiguration());
            builder.AddConfiguration(new EHS_NgayThucHienKhamSKConfiguration());

            builder.AddConfiguration(new EHS_KeHoachDaoTaoAnToanVSLDConfiguration());
            builder.AddConfiguration(new EHS_ThoiGianThucHienAnToanVSLDConfiguration());

            builder.AddConfiguration(new EHS_KeHoachPCCCConfiguration());
            builder.AddConfiguration(new EHS_ThoiGianThucHienPCCCConfiguration());
            builder.AddConfiguration(new EHS_KeHoachAntoanBucXaConfiguration());
            builder.AddConfiguration(new EHS_ThoiGianThucHienAntoanBucXaConfiguration());

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
