﻿using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class ChungChiConfiguration : DbEntityConfiguration<CHUNG_CHI>
    {
        public override void Configure(EntityTypeBuilder<CHUNG_CHI> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasMaxLength(50).IsRequired();
        }
    }
}
