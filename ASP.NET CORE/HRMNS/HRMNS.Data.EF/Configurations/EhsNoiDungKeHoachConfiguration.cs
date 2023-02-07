using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class EventParentConfiguration : DbEntityConfiguration<EVENT_SHEDULE_PARENT>
    {
        public override void Configure(EntityTypeBuilder<EVENT_SHEDULE_PARENT> entity)
        {
            entity.HasKey(x => x.Id);
        }
    }
}
