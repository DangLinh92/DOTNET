using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Data.EF.Configurations
{
    public class PostOperationShippingConfiguration : DbEntityConfiguration<POST_OPERATION_SHIPPING>
    {
        public override void Configure(EntityTypeBuilder<POST_OPERATION_SHIPPING> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
