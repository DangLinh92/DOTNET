using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    public class TrainingConfigurationConfiguration : DbEntityConfiguration<HR_TRAINING>
    {
        public override void Configure(EntityTypeBuilder<HR_TRAINING> entity)
        {
            entity.HasKey(x => x.Id);
        }
    }

    public class TrainingNhanVienConfiguration : DbEntityConfiguration<TRAINING_NHANVIEN>
    {
        public override void Configure(EntityTypeBuilder<TRAINING_NHANVIEN> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public class TrainingTypeConfiguration : DbEntityConfiguration<TRAINING_TYPE>
    {
        public override void Configure(EntityTypeBuilder<TRAINING_TYPE> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
