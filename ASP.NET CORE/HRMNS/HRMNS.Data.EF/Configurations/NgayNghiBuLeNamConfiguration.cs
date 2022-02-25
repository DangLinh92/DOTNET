using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class NgayNghiBuLeNamConfiguration : DbEntityConfiguration<NGAY_NGHI_BU_LE_NAM>
    {
        public override void Configure(EntityTypeBuilder<NGAY_NGHI_BU_LE_NAM> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }
}
