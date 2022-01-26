using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class BoPhanDetailConfiguration : DbEntityConfiguration<HR_BO_PHAN_DETAIL>
    {
        public override void Configure(EntityTypeBuilder<HR_BO_PHAN_DETAIL> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }
}
