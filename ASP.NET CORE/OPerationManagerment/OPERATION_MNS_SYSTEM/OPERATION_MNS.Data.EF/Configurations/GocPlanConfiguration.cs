using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Data.EF.Configurations
{
    public class GocPlanConfiguration : DbEntityConfiguration<GOC_PLAN>
    {
        public override void Configure(EntityTypeBuilder<GOC_PLAN> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class FABPLANConfiguration : DbEntityConfiguration<FAB_PLAN>
    {
        public override void Configure(EntityTypeBuilder<FAB_PLAN> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class CTQConfiguration : DbEntityConfiguration<VIEW_CONTROL_CHART_MODEL>
    {
        public override void Configure(EntityTypeBuilder<VIEW_CONTROL_CHART_MODEL> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
