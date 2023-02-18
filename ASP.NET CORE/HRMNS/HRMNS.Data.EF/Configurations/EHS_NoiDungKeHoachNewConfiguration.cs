using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class EHS_NoiDungKeHoachNewConfiguration : DbEntityConfiguration<EHS_KEHOACH_QUANTRAC>
    {
        public override void Configure(EntityTypeBuilder<EHS_KEHOACH_QUANTRAC> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class EHS_NgayThucHienChiTietConfiguration : DbEntityConfiguration<EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC>
    {
        public override void Configure(EntityTypeBuilder<EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class EHS_NhanVienKhamSKConfiguration : DbEntityConfiguration<EHS_NHANVIEN_KHAM_SK>
    {
        public override void Configure(EntityTypeBuilder<EHS_NHANVIEN_KHAM_SK> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class EHS_KeHoachKhamSKConfiguration : DbEntityConfiguration<EHS_KE_HOACH_KHAM_SK>
    {
        public override void Configure(EntityTypeBuilder<EHS_KE_HOACH_KHAM_SK> entity)
        {
            entity.HasKey(x => x.Id);
        }
    }

    public class EHS_NgayThucHienKhamSKConfiguration : DbEntityConfiguration<EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK>
    {
        public override void Configure(EntityTypeBuilder<EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class EHS_KeHoachDaoTaoAnToanVSLDConfiguration : DbEntityConfiguration<EHS_KEHOACH_DAOTAO_ANTOAN_VSLD>
    {
        public override void Configure(EntityTypeBuilder<EHS_KEHOACH_DAOTAO_ANTOAN_VSLD> entity)
        {
            entity.HasKey(x => x.Id);
        }
    }

    public class EHS_ThoiGianThucHienAnToanVSLDConfiguration : DbEntityConfiguration<EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD>
    {
        public override void Configure(EntityTypeBuilder<EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class EHS_KeHoachPCCCConfiguration : DbEntityConfiguration<EHS_KEHOACH_PCCC>
    {
        public override void Configure(EntityTypeBuilder<EHS_KEHOACH_PCCC> entity)
        {
            entity.HasKey(x => x.Id);
        }
    }

    public class EHS_ThoiGianThucHienPCCCConfiguration : DbEntityConfiguration<EHS_THOIGIAN_THUC_HIEN_PCCC>
    {
        public override void Configure(EntityTypeBuilder<EHS_THOIGIAN_THUC_HIEN_PCCC> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class EHS_KeHoachAntoanBucXaConfiguration : DbEntityConfiguration<EHS_KEHOACH_ANTOAN_BUCXA>
    {
        public override void Configure(EntityTypeBuilder<EHS_KEHOACH_ANTOAN_BUCXA> entity)
        {
            entity.HasKey(x => x.Id);
        }
    }

    public class EHS_ThoiGianThucHienAntoanBucXaConfiguration : DbEntityConfiguration<EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA>
    {
        public override void Configure(EntityTypeBuilder<EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class EHS_KeHoachKiemDinhMayMocConfiguration : DbEntityConfiguration<EHS_KEHOACH_KIEMDINH_MAYMOC>
    {
        public override void Configure(EntityTypeBuilder<EHS_KEHOACH_KIEMDINH_MAYMOC> entity)
        {
            entity.HasKey(x => x.Id);
        }
    }

    public class EHS_ThoiGianKiemDinhMayMocConfiguration : DbEntityConfiguration<EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM>
    {
        public override void Configure(EntityTypeBuilder<EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class EHS_EmailNotifyConfiguration : DbEntityConfiguration<EHS_MAIL_NOTIFY>
    {
        public override void Configure(EntityTypeBuilder<EHS_MAIL_NOTIFY> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
