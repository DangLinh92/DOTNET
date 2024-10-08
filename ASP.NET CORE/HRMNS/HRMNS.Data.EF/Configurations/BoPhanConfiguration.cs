﻿using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class BoPhanConfiguration : DbEntityConfiguration<BOPHAN>
    {
        public override void Configure(EntityTypeBuilder<BOPHAN> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasMaxLength(50).IsRequired();

            entity.HasMany(x => x.HR_NHANVIEN).WithOne(x => x.BOPHAN).OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class HoTroSinhLyConfiguration : DbEntityConfiguration<HOTRO_SINH_LY>
    {
        public override void Configure(EntityTypeBuilder<HOTRO_SINH_LY> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }

    public class ThanhToanNghiViecConfiguration : DbEntityConfiguration<HR_THANHTOAN_NGHIVIEC>
    {
        public override void Configure(EntityTypeBuilder<HR_THANHTOAN_NGHIVIEC> entity)
        {
            entity.HasKey(c => c.Id);
        }
    }

}
