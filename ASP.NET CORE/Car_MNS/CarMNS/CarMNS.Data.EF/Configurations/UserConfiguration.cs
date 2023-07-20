using CarMNS.Data.EF.Extensions;
using CarMNS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarMNS.Data.EF.Configurations
{
    public class UserConfiguration : DbEntityConfiguration<APP_USER>
    {
        public override void Configure(EntityTypeBuilder<APP_USER> entity)
        {
            entity.HasMany(e => e.APP_USER_TOKEN)
            .WithOne(e => e.APP_USER)
            .HasForeignKey(ut => ut.UserId)
            .IsRequired();
        }
    }

    public class UserRoleConfiguration : DbEntityConfiguration<APP_USER_ROLE>
    {
        public override void Configure(EntityTypeBuilder<APP_USER_ROLE> entity)
        {
            entity.HasKey(x => new { x.UserId, x.RoleId });
        }
    }
}
