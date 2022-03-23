using VOC.Data.EF.Extensions;
using VOC.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace VOC.Data.EF.Configurations
{
    public class LanguageConfiguration : DbEntityConfiguration<LANGUAGE>
    {
        public override void Configure(EntityTypeBuilder<LANGUAGE> entity)
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasMaxLength(50).IsRequired();
        }
    }
}
