using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class DkyChamCongOTDacBietConfiguration : DbEntityConfiguration<DANGKY_CHAMCONG_OT_DACBIET>
    {
        public override void Configure(EntityTypeBuilder<DANGKY_CHAMCONG_OT_DACBIET> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }
}
