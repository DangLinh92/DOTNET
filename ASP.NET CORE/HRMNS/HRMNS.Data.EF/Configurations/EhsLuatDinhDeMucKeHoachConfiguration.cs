using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class EhsLuatDinhDeMucKeHoachConfiguration : DbEntityConfiguration<EHS_LUATDINH_DEMUC_KEHOACH>
    {
        public override void Configure(EntityTypeBuilder<EHS_LUATDINH_DEMUC_KEHOACH> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
