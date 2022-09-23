using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class EhsDMKeHoachConfiguration : DbEntityConfiguration<EHS_DM_KEHOACH>
    {
        public override void Configure(EntityTypeBuilder<EHS_DM_KEHOACH> entity)
        {
            entity.HasKey(x => x.Id);
        }
    }
}
