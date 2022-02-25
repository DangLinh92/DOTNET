using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class ChamCongLogConfiguration : DbEntityConfiguration<CHAM_CONG_LOG>
    {
        public override void Configure(EntityTypeBuilder<CHAM_CONG_LOG> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity
                   .Property(e => e.FirstIn)
                   .IsUnicode(false);

            entity
                  .Property(e => e.LastOut)
                  .IsUnicode(false);
        }
    }
}
