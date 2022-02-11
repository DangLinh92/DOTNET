using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class HopDongConfiguration : DbEntityConfiguration<HR_HOPDONG>
    {
        public override void Configure(EntityTypeBuilder<HR_HOPDONG> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(x => x.DayNumberNoti).HasDefaultValue(30);
        }
    }
}
