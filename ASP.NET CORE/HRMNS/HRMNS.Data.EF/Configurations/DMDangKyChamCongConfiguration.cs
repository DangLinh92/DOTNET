using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class DMDangKyChamCongConfiguration : DbEntityConfiguration<DM_DANGKY_CHAMCONG>
    {
        public override void Configure(EntityTypeBuilder<DM_DANGKY_CHAMCONG> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();

            entity.HasMany(x => x.DANGKY_CHAMCONG_CHITIET).WithOne(x => x.DM_DANGKY_CHAMCONG).OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);
        }
    }
}
