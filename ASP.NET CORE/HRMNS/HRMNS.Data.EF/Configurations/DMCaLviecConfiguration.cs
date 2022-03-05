using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class DMCaLviecConfiguration : DbEntityConfiguration<DM_CA_LVIEC>
    {
        public override void Configure(EntityTypeBuilder<DM_CA_LVIEC> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasMaxLength(50);

            entity.HasMany(x => x.CA_LVIEC).WithOne(x => x.DM_CA_LVIEC).OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.SetNull);
            entity.HasMany(x => x.NHANVIEN_CALAMVIEC).WithOne(x => x.DM_CA_LVIEC).OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.SetNull);
            entity.HasMany(x => x.SETTING_TIME_DIMUON_VESOM).WithOne(x => x.DM_CA_LVIEC).OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.SetNull);
        }
    }
}
