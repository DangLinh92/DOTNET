using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class AttendenceRecordConfiguration : DbEntityConfiguration<ATTENDANCE_RECORD>
    {
        public override void Configure(EntityTypeBuilder<ATTENDANCE_RECORD> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
            entity.HasMany(x => x.ATTENDANCE_OVERTIME).WithOne(x => x.ATTENDANCE_RECORD).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
