using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class CheDoBHConfiguration : DbEntityConfiguration<HR_CHEDOBH>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<HR_CHEDOBH> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasMaxLength(50).IsRequired();

            entity.HasMany(x => x.HR_KEKHAIBAOHIEM).WithOne(x => x.HR_CHEDOBH).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
