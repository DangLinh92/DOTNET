using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Data.EF.Configurations
{
    public class YieldOfModelConfiguration : DbEntityConfiguration<YIELD_OF_MODEL>
    {
        public override void Configure(EntityTypeBuilder<YIELD_OF_MODEL> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class StayLotListConfiguration : DbEntityConfiguration<STAY_LOT_LIST>
    {
        public override void Configure(EntityTypeBuilder<STAY_LOT_LIST> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class StayLotListWlp2Configuration : DbEntityConfiguration<STAY_LOT_LIST_WLP2>
    {
        public override void Configure(EntityTypeBuilder<STAY_LOT_LIST_WLP2> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }


    public class CTQSettingConfiguration : DbEntityConfiguration<CTQ_SETTING>
    {
        public override void Configure(EntityTypeBuilder<CTQ_SETTING> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class CapaLfemConfiguration : DbEntityConfiguration<CAPA_LFEM_DATA>
    {
        public override void Configure(EntityTypeBuilder<CAPA_LFEM_DATA> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class CTQSettingWlp2Configuration : DbEntityConfiguration<CTQ_SETTING_WLP2>
    {
        public override void Configure(EntityTypeBuilder<CTQ_SETTING_WLP2> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class CtqEmailConfiguration : DbEntityConfiguration<CTQ_EMAIL_RECEIV>
    {
        public override void Configure(EntityTypeBuilder<CTQ_EMAIL_RECEIV> entity)
        {
            entity.HasKey(x => x.Id);

        }
    }

    public class CtqEmailwlp2Configuration : DbEntityConfiguration<CTQ_EMAIL_RECEIV_WLP2>
    {
        public override void Configure(EntityTypeBuilder<CTQ_EMAIL_RECEIV_WLP2> entity)
        {
            entity.HasKey(x => x.Id);

        }
    }

    public class ThickNetModelWlp2Configuration : DbEntityConfiguration<THICKNET_MODEL_WLP2>
    {
        public override void Configure(EntityTypeBuilder<THICKNET_MODEL_WLP2> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class StayLotListPrioryLfemConfiguration : DbEntityConfiguration<STAY_LOT_LIST_PRIORY_LFEM>
    {
        public override void Configure(EntityTypeBuilder<STAY_LOT_LIST_PRIORY_LFEM> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class LotTestHistoryLfemConfiguration : DbEntityConfiguration<LOT_TEST_HISTOTY_LFEM>
    {
        public override void Configure(EntityTypeBuilder<LOT_TEST_HISTOTY_LFEM> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class StayLotListLfemConfiguration : DbEntityConfiguration<STAY_LOT_LIST_LFEM>
    {
        public override void Configure(EntityTypeBuilder<STAY_LOT_LIST_LFEM> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class StayLotListHistoryLfemConfiguration : DbEntityConfiguration<STAY_LOT_LIST_HISTORY_LFEM>
    {
        public override void Configure(EntityTypeBuilder<STAY_LOT_LIST_HISTORY_LFEM> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class WarningLotRunLfemConfiguration : DbEntityConfiguration<WARNING_LOT_RUN_LFEM>
    {
        public override void Configure(EntityTypeBuilder<WARNING_LOT_RUN_LFEM> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class WarningLotRunTimeLfemConfiguration : DbEntityConfiguration<WARNING_LOT_RUNTIME_LFEM>
    {
        public override void Configure(EntityTypeBuilder<WARNING_LOT_RUNTIME_LFEM> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
