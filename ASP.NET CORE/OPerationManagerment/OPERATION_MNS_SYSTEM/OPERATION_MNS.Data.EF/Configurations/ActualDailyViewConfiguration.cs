using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Data.EF.Configurations
{
    public class ActualDailyViewConfiguration : DbEntityConfiguration<ACTUAL_DAILY_VIEW>
    {
        public override void Configure(EntityTypeBuilder<ACTUAL_DAILY_VIEW> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
