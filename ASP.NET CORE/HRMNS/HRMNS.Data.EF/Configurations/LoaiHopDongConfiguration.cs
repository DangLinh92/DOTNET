using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class LoaiHopDongConfiguration : DbEntityConfiguration<HR_LOAIHOPDONG>
    {
        public override void Configure(EntityTypeBuilder<HR_LOAIHOPDONG> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();

            entity.HasMany(x => x.HR_HOPDONG).WithOne(x => x.HR_LOAIHOPDONG).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
