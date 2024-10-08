﻿using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class ChucDanhConfiguration : DbEntityConfiguration<HR_CHUCDANH>
    {
        public override void Configure(EntityTypeBuilder<HR_CHUCDANH> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasMaxLength(50).IsRequired();

            entity.HasMany(x => x.HR_NHANVIEN).WithOne(x => x.HR_CHUCDANH).OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class ChucDanhByYearConfiguration : DbEntityConfiguration<HR_CHUCDANH_BY_YEAR>
    {
        public override void Configure(EntityTypeBuilder<HR_CHUCDANH_BY_YEAR> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }
}
