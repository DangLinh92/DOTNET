using CarMNS.Data.EF.Extensions;
using CarMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarMNS.Data.EF.Configurations
{
    public class PermissionConfiguration : DbEntityConfiguration<PERMISSION>
    {
        public override void Configure(EntityTypeBuilder<PERMISSION> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
