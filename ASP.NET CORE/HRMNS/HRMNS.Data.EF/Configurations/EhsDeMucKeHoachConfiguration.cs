using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore;
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

    public class FileManagementConfiguration : DbEntityConfiguration<FILE_MANAGER>
    {
        public override void Configure(EntityTypeBuilder<FILE_MANAGER> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(x => x.Id).HasColumnName("ItemID");
        }
    }
}
