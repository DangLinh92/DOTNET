using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Data.EF.Configurations
{
    public class SettingItemsConfiguration : DbEntityConfiguration<SETTING_ITEMS>
    {
        public override void Configure(EntityTypeBuilder<SETTING_ITEMS> entity)
        {
            entity.HasKey(x => x.Id);
        }
    }

    public class DateOffLineConfiguration : DbEntityConfiguration<DATE_OFF_LINE>
    {
        public override void Configure(EntityTypeBuilder<DATE_OFF_LINE> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class DateOffLineLFEMConfiguration : DbEntityConfiguration<DATE_OFF_LINE_LFEM>
    {
        public override void Configure(EntityTypeBuilder<DATE_OFF_LINE_LFEM> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class DateOffLineSampleConfiguration : DbEntityConfiguration<DATE_OFF_LINE_SAMPLE>
    {
        public override void Configure(EntityTypeBuilder<DATE_OFF_LINE_SAMPLE> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class DateOffLineSMTConfiguration : DbEntityConfiguration<DATE_OFF_LINE_SMT>
    {
        public override void Configure(EntityTypeBuilder<DATE_OFF_LINE_SMT> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class DelayCommentSampleConfiguration : DbEntityConfiguration<DELAY_COMMENT_SAMPLE>
    {
        public override void Configure(EntityTypeBuilder<DELAY_COMMENT_SAMPLE> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class OutPutByLotLFEMConfiguration : DbEntityConfiguration<OUT_PUT_BY_LOT_LFEM>
    {
        public override void Configure(EntityTypeBuilder<OUT_PUT_BY_LOT_LFEM> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class OutPutShippingLFEMConfiguration : DbEntityConfiguration<OUT_PUT_SHIPPING_LOT_LFEM>
    {
        public override void Configure(EntityTypeBuilder<OUT_PUT_SHIPPING_LOT_LFEM> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class KHSXActualLFEMConfiguration : DbEntityConfiguration<KHSX_ACTUAL_LFEM>
    {
        public override void Configure(EntityTypeBuilder<KHSX_ACTUAL_LFEM> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class DailyPlanDataLFEMConfiguration : DbEntityConfiguration<DAILY_PLAN_DATA_LFEM>
    {
        public override void Configure(EntityTypeBuilder<DAILY_PLAN_DATA_LFEM> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class ActualPlanSampleConfiguration : DbEntityConfiguration<ACTUAL_PLAN_SAMPLE>
    {
        public override void Configure(EntityTypeBuilder<ACTUAL_PLAN_SAMPLE> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
