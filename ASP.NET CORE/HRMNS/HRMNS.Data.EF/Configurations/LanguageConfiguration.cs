using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
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
