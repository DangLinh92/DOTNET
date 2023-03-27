using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class DMDieuChinhChamCongConfiguration : DbEntityConfiguration<DM_DIEUCHINH_CHAMCONG>
    {
        public override void Configure(EntityTypeBuilder<DM_DIEUCHINH_CHAMCONG> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }
}
