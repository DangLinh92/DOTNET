using VOC.Data.EF.Extensions;
using VOC.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace VOC.Data.EF.Configurations
{
    public class FunctionConfiguration : DbEntityConfiguration<FUNCTION>
    {
        public override void Configure(EntityTypeBuilder<FUNCTION> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasMaxLength(128).IsRequired();
        }
    }
}
