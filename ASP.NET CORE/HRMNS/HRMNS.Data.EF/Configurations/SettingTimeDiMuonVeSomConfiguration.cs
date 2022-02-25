using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class SettingTimeDiMuonVeSomConfiguration : DbEntityConfiguration<SETTING_TIME_DIMUON_VESOM>
    {
        public override void Configure(EntityTypeBuilder<SETTING_TIME_DIMUON_VESOM> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }
}
