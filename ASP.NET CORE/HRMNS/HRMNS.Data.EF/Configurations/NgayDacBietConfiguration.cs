using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class NgayDacBietConfiguration : DbEntityConfiguration<NGAY_DAC_BIET>
    {
        public override void Configure(EntityTypeBuilder<NGAY_DAC_BIET> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasMaxLength(10);
        }
    }
}
