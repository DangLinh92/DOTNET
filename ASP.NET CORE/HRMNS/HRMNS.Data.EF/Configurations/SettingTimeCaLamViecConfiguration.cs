﻿using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class SettingTimeCaLamViecConfiguration : DbEntityConfiguration<SETTING_TIME_CA_LVIEC>
    {
        public override void Configure(EntityTypeBuilder<SETTING_TIME_CA_LVIEC> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }
}
