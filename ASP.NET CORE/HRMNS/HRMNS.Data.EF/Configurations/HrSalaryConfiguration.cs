using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMNS.Data.Entities.Payroll;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    class HrSalaryPRConfiguration : DbEntityConfiguration<HR_SALARY_PR>
    {
        public override void Configure(EntityTypeBuilder<HR_SALARY_PR> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }

    class HrSalaryConfiguration : DbEntityConfiguration<HR_SALARY>
    {
        public override void Configure(EntityTypeBuilder<HR_SALARY> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }

    class HrSalaryPhatSinhConfiguration : DbEntityConfiguration<HR_SALARY_PHATSINH>
    {
        public override void Configure(EntityTypeBuilder<HR_SALARY_PHATSINH> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }

    class HrSalaryDanhMucPhatSinhConfiguration : DbEntityConfiguration<HR_SALARY_DANHMUC_PHATSINH>
    {
        public override void Configure(EntityTypeBuilder<HR_SALARY_DANHMUC_PHATSINH> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }

    class HrBangLuongExConfiguration : DbEntityConfiguration<BANG_CONG_EXTENTION>
    {
        public override void Configure(EntityTypeBuilder<BANG_CONG_EXTENTION> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }

    class HrSalaryHistoryConfiguration : DbEntityConfiguration<HR_SALARY_HISTORY>
    {
        public override void Configure(EntityTypeBuilder<HR_SALARY_HISTORY> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }

    class HrNgayChotCongConfiguration : DbEntityConfiguration<HR_NGAY_CHOT_CONG>
    {
        public override void Configure(EntityTypeBuilder<HR_NGAY_CHOT_CONG> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }

    class HrBangCongHistoryConfiguration : DbEntityConfiguration<BANGLUONGCHITIET_HISTORY>
    {
        public override void Configure(EntityTypeBuilder<BANGLUONGCHITIET_HISTORY> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }

    class HrBangCongHistoryPrConfiguration : DbEntityConfiguration<BANGLUONGCHITIET_HISTORY_PR>
    {
        public override void Configure(EntityTypeBuilder<BANGLUONGCHITIET_HISTORY_PR> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }

    class HrSalaryPhatSinhPRConfiguration : DbEntityConfiguration<HR_SALARY_PHATSINH_PR>
    {
        public override void Configure(EntityTypeBuilder<HR_SALARY_PHATSINH_PR> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }
}
