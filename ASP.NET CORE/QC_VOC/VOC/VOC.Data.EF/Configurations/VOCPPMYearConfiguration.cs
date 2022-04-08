using VOC.Data.EF.Extensions;
using VOC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace VOC.Data.EF.Configurations
{
    public class VOCPPMYearConfiguration : DbEntityConfiguration<VOC_PPM_YEAR>
    {
        public override void Configure(EntityTypeBuilder<VOC_PPM_YEAR> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        }
    }
}
