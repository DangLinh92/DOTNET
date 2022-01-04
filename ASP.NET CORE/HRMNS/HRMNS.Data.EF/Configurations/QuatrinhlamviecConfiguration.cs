using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class QuatrinhlamviecConfiguration : DbEntityConfiguration<HR_QUATRINHLAMVIEC>
    {
        public override void Configure(EntityTypeBuilder<HR_QUATRINHLAMVIEC> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        }
    }
}
