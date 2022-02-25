using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class TruSoLamViecConfiguration : DbEntityConfiguration<TRU_SO_LVIEC>
    {
        public override void Configure(EntityTypeBuilder<TRU_SO_LVIEC> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasMaxLength(50);

            entity.HasMany(x => x.DM_CA_LVIEC).WithOne(x => x.TRU_SO_LVIEC).OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.SetNull);
        }
    }
}
