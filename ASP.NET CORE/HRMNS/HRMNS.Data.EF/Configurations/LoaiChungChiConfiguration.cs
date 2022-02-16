using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class LoaiChungChiConfiguration : DbEntityConfiguration<LOAICHUNGCHI>
    {
        public override void Configure(EntityTypeBuilder<LOAICHUNGCHI> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();

            entity.HasMany(x => x.CHUNG_CHI).WithOne(x => x.LOAICHUNGCHI1).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
