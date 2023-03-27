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

    public class GocPlanWLP2Configuration : DbEntityConfiguration<GOC_PLAN_WLP2>
    {
        public override void Configure(EntityTypeBuilder<GOC_PLAN_WLP2> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class ViewWipPostConfiguration : DbEntityConfiguration<VIEW_WIP_POST_WLP>
    {
        public override void Configure(EntityTypeBuilder<VIEW_WIP_POST_WLP> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class SmtReturnConfiguration : DbEntityConfiguration<SMT_RETURN_WLP2>
    {
        public override void Configure(EntityTypeBuilder<SMT_RETURN_WLP2> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class BoPhanDeNghiXuatConfiguration : DbEntityConfiguration<BOPHAN_DE_NGHI_XUAT_NLIEU>
    {
        public override void Configure(EntityTypeBuilder<BOPHAN_DE_NGHI_XUAT_NLIEU> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class OutGoingReceipConfiguration : DbEntityConfiguration<OUTGOING_RECEIPT_WLP2>
    {
        public override void Configure(EntityTypeBuilder<OUTGOING_RECEIPT_WLP2> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class KhungThoiGianXuatConfiguration : DbEntityConfiguration<KHUNG_THOI_GIAN_XUAT_HANG_WLP2>
    {
        public override void Configure(EntityTypeBuilder<KHUNG_THOI_GIAN_XUAT_HANG_WLP2> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class StayLotListPrioryConfiguration : DbEntityConfiguration<STAY_LOT_LIST_PRIORY_WLP2>
    {
        public override void Configure(EntityTypeBuilder<STAY_LOT_LIST_PRIORY_WLP2> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
