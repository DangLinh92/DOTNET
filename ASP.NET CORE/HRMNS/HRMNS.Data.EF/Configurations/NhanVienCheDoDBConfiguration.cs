using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
namespace HRMNS.Data.EF.Configurations
{
    public class NhanVienCheDoDBConfiguration : DbEntityConfiguration<HR_NHANVIEN_CHEDO_DB>
    {
        public override void Configure(EntityTypeBuilder<HR_NHANVIEN_CHEDO_DB> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }
}
