using VOC.Data.EF.Extensions;
using VOC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace VOC.Data.EF.Configurations
{
    public class VOCPPMConfiguration : DbEntityConfiguration<VOC_PPM>
    {
        public override void Configure(EntityTypeBuilder<VOC_PPM> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        }
    }
}
