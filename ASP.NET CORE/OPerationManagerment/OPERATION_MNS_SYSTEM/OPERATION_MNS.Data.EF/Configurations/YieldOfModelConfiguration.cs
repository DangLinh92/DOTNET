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


    public class CTQSettingConfiguration : DbEntityConfiguration<CTQ_SETTING>
    {
        public override void Configure(EntityTypeBuilder<CTQ_SETTING> entity)
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
}
