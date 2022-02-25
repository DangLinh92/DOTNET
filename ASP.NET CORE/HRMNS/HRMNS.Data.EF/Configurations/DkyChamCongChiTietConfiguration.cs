using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class DkyChamCongChiTietConfiguration : DbEntityConfiguration<DANGKY_CHAMCONG_CHITIET>
    {
        public override void Configure(EntityTypeBuilder<DANGKY_CHAMCONG_CHITIET> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();

            entity.HasMany(x => x.DC_CHAM_CONG).WithOne(x => x.DANGKY_CHAMCONG_CHITIET).OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.SetNull);
            entity.HasMany(x => x.DANGKY_CHAMCONG_DACBIET).WithOne(x => x.DANGKY_CHAMCONG_CHITIET).OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.SetNull);
        }
    }
}
