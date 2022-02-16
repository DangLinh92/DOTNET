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
        }
    }
}
