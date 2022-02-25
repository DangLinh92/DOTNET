using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class DkyChamCongDacBietConfiguration : DbEntityConfiguration<DANGKY_CHAMCONG_DACBIET>
    {
        public override void Configure(EntityTypeBuilder<DANGKY_CHAMCONG_DACBIET> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }
}
