using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class ChucDanhConfiguration : DbEntityConfiguration<HR_CHUCDANH>
    {
        public override void Configure(EntityTypeBuilder<HR_CHUCDANH> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasMaxLength(50).IsRequired();
        }
    }
}
