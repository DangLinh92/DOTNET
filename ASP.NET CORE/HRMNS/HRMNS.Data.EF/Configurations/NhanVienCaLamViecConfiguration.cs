using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class NhanVienCaLamViecConfiguration : DbEntityConfiguration<NHANVIEN_CALAMVIEC>
    {
        public override void Configure(EntityTypeBuilder<NHANVIEN_CALAMVIEC> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }
}
