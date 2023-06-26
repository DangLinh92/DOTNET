using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Data.EF.Configurations
{
    public class TinhHinhSanXuatSampleConfiguration : DbEntityConfiguration<TINH_HINH_SAN_XUAT_SAMPLE>
    {
        public override void Configure(EntityTypeBuilder<TINH_HINH_SAN_XUAT_SAMPLE> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class PhanLoaiHangSampleConfiguration : DbEntityConfiguration<PHAN_LOAI_HANG_SAMPLE>
    {
        public override void Configure(EntityTypeBuilder<PHAN_LOAI_HANG_SAMPLE> entity)
        {
            entity.HasKey(x => x.Id); 
            entity.Property(x => x.Id).HasMaxLength(50);
        }
    }

    public class PhanLoaiModelSampleConfiguration : DbEntityConfiguration<PHAN_LOAI_MODEL_SAMPLE>
    {
        public override void Configure(EntityTypeBuilder<PHAN_LOAI_MODEL_SAMPLE> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasMaxLength(50);
        }
    }

    public class TCardSampleConfiguration : DbEntityConfiguration<TCARD_SAMPLE>
    {
        public override void Configure(EntityTypeBuilder<TCARD_SAMPLE> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
