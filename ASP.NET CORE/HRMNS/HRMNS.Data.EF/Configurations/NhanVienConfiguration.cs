using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class NhanVienConfiguration : DbEntityConfiguration<HR_NHANVIEN>
    {
        public override void Configure(EntityTypeBuilder<HR_NHANVIEN> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasMaxLength(50).IsRequired();

            entity.HasMany(x => x.HR_HOPDONG).WithOne(x => x.HR_NHANVIEN).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(x => x.HR_BHXH).WithOne(x => x.HR_NHANVIEN).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(x => x.HR_CHUNGCHI_NHANVIEN).WithOne(x => x.HR_NHANVIEN).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(x => x.HR_KEKHAIBAOHIEM).WithOne(x => x.HR_NHANVIEN).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(x => x.HR_QUATRINHLAMVIEC).WithOne(x => x.HR_NHANVIEN).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(x => x.HR_TINHTRANGHOSO).WithOne(x => x.HR_NHANVIEN).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(x => x.HR_PHEP_NAM).WithOne(x => x.HR_NHANVIEN).OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(x => x.DANGKY_CHAMCONG_DACBIET).WithOne(x => x.HR_NHANVIEN).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(x => x.DANGKY_OT_NHANVIEN).WithOne(x => x.HR_NHANVIEN).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(x => x.DC_CHAM_CONG).WithOne(x => x.HR_NHANVIEN).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(x => x.NHANVIEN_CALAMVIEC).WithOne(x => x.HR_NHANVIEN).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(x => x.ATTENDANCE_RECORD).WithOne(x => x.HR_NHANVIEN).OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class NhanVienExConfiguration : DbEntityConfiguration<NHANVIEN_INFOR_EX>
    {
        public override void Configure(EntityTypeBuilder<NHANVIEN_INFOR_EX> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }

    public class NhanVien2Configuration : DbEntityConfiguration<HR_NHANVIEN_2>
    {
        public override void Configure(EntityTypeBuilder<HR_NHANVIEN_2> entity)
        {
            entity.HasKey(c => c.Id);
        }
    }

    public class DailyTimeWorkingConfiguration : DbEntityConfiguration<DAILY_TIME_WORKING>
    {
        public override void Configure(EntityTypeBuilder<DAILY_TIME_WORKING> entity)
        {
            entity.HasKey(c => c.Id);
        }
    }

    public class KhenThuongKyLuatConfiguration : DbEntityConfiguration<HR_KY_LUAT_KHENTHUONG>
    {
        public override void Configure(EntityTypeBuilder<HR_KY_LUAT_KHENTHUONG> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }

    public class UserConfiguration : DbEntityConfiguration<APP_USER>
    {
        public override void Configure(EntityTypeBuilder<APP_USER> entity)
        {
            entity.HasMany(e => e.APP_USER_TOKEN)
            .WithOne(e => e.APP_USER)
            .HasForeignKey(ut => ut.UserId)
            .IsRequired();
        }
    }
}
