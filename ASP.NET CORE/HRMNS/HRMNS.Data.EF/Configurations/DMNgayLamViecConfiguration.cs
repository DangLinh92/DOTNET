using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class DMNgayLamViecConfiguration : DbEntityConfiguration<DM_NGAY_LAMVIEC>
    {
        public override void Configure(EntityTypeBuilder<DM_NGAY_LAMVIEC> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();

            entity.HasMany(x => x.CA_LVIEC).WithOne(x => x.DM_NGAY_LAMVIEC).OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.SetNull);
            entity.HasMany(x => x.DANGKY_OT_NHANVIEN).WithOne(x => x.DM_NGAY_LAMVIEC).OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.SetNull);
            entity.HasMany(x => x.HE_SO_OVERTIME).WithOne(x => x.DM_NGAY_LAMVIEC).OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.SetNull);
        }
    }
}
