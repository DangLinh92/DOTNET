﻿using VOC.Data.EF.Extensions;
using VOC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace VOC.Data.EF.Configurations
{
    public class VOCDefectTypeConfiguration : DbEntityConfiguration<VOC_DEFECT_TYPE>
    {
        public override void Configure(EntityTypeBuilder<VOC_DEFECT_TYPE> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        }
    }
}
