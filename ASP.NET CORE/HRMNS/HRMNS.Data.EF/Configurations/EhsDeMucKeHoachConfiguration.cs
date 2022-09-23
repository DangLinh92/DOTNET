using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class EhsDeMucKeHoachConfiguration : DbEntityConfiguration<EHS_DEMUC_KEHOACH>
    {
        public override void Configure(EntityTypeBuilder<EHS_DEMUC_KEHOACH> entity)
        {
            entity.HasKey(x => x.Id);
        }
    }
}
