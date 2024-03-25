using CarMNS.Data.EF.Extensions;
using CarMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarMNS.Data.EF.Configurations
{
    public class CarConfiguration : DbEntityConfiguration<CAR>
    {
        public override void Configure(EntityTypeBuilder<CAR> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class LaixeConfiguration : DbEntityConfiguration<LAI_XE>
    {
        public override void Configure(EntityTypeBuilder<LAI_XE> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class LaixeCarConfiguration : DbEntityConfiguration<LAI_XE_CAR>
    {
        public override void Configure(EntityTypeBuilder<LAI_XE_CAR> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class DangKyXeConfiguration : DbEntityConfiguration<DANG_KY_XE>
    {
        public override void Configure(EntityTypeBuilder<DANG_KY_XE> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class DieuXeDangKyXeConfiguration : DbEntityConfiguration<DIEUXE_DANGKY>
    {
        public override void Configure(EntityTypeBuilder<DIEUXE_DANGKY> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class BoPhanConfiguration : DbEntityConfiguration<BOPHAN>
    {
        public override void Configure(EntityTypeBuilder<BOPHAN> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasMaxLength(50).IsRequired();
        }
    }

    public class DangKyXeTaxiConfiguration : DbEntityConfiguration<DANG_KY_XE_TAXI>
    {
        public override void Configure(EntityTypeBuilder<DANG_KY_XE_TAXI> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class TaxiCardInfoConfiguration : DbEntityConfiguration<TAXI_CARD_INFO>
    {
        public override void Configure(EntityTypeBuilder<TAXI_CARD_INFO> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class MucDichSDConfiguration : DbEntityConfiguration<MUCDICHSD_XE>
    {
        public override void Configure(EntityTypeBuilder<MUCDICHSD_XE> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
    

    public class BoPhanDuyetConfiguration : DbEntityConfiguration<BOPHAN_DUYET>
    {
        public override void Configure(EntityTypeBuilder<BOPHAN_DUYET> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
