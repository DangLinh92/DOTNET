using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class EhsNoiDungKeHoachConfiguration : DbEntityConfiguration<EHS_NOIDUNG_KEHOACH>
    {
        public override void Configure(EntityTypeBuilder<EHS_NOIDUNG_KEHOACH> entity)
        {
            entity.HasKey(x => x.Id);
        }
    }

    public class EhsNoiDungConfiguration : DbEntityConfiguration<EHS_NOIDUNG>
    {
        public override void Configure(EntityTypeBuilder<EHS_NOIDUNG> entity)
        {
            entity.HasKey(x => x.Id);
        }
    }
}
