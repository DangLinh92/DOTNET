﻿using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMNS.Data.Entities.Payroll;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Configurations
{
    class PhepNamConfiguration : DbEntityConfiguration<HR_PHEP_NAM>
    {
        public override void Configure(EntityTypeBuilder<HR_PHEP_NAM> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }

    public class CongDoanNotJoinConfiguration : DbEntityConfiguration<CONGDOAN_NOT_JOIN>
    {
        public override void Configure(EntityTypeBuilder<CONGDOAN_NOT_JOIN> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }

    public class PhuCapDocHaiConfiguration : DbEntityConfiguration<PHUCAP_DOC_HAI>
    {
        public override void Configure(EntityTypeBuilder<PHUCAP_DOC_HAI> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
        }
    }
    public class HrGradeConfiguration : DbEntityConfiguration<HR_SALARY_GRADE>
    {
        public override void Configure(EntityTypeBuilder<HR_SALARY_GRADE> entity)
        {
            entity.HasKey(c => c.Id);
        }
    }
}
