using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class KeKhaiBaoHiemConfiguration : DbEntityConfiguration<HR_KEKHAIBAOHIEM>
    {
        public override void Configure(EntityTypeBuilder<HR_KEKHAIBAOHIEM> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
