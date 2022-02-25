using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class KyHieuChamCongConfiguration : DbEntityConfiguration<KY_HIEU_CHAM_CONG>
    {
        public override void Configure(EntityTypeBuilder<KY_HIEU_CHAM_CONG> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasMaxLength(20);

            entity.HasMany(x => x.CA_LVIEC).WithOne(x => x.KY_HIEU_CHAM_CONG).OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.SetNull);
            entity.HasMany(x => x.DANGKY_CHAMCONG_CHITIET).WithOne(x => x.KY_HIEU_CHAM_CONG).OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.SetNull);
            entity.HasMany(x => x.NGAY_LE_NAM).WithOne(x => x.KY_HIEU_CHAM_CONG).OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.SetNull);
            entity.HasMany(x => x.NGAY_NGHI_BU_LE_NAM).WithOne(x => x.KY_HIEU_CHAM_CONG).OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.SetNull);
        }
    }
}
