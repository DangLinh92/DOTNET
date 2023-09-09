using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Data.EF.Configurations
{
    public class LeadTimeConfiguration : DbEntityConfiguration<LEAD_TIME_WLP>
    {
        public override void Configure(EntityTypeBuilder<LEAD_TIME_WLP> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class LeadTimeLfemConfiguration : DbEntityConfiguration<LEAD_TIME_LFEM>
    {
        public override void Configure(EntityTypeBuilder<LEAD_TIME_LFEM> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
