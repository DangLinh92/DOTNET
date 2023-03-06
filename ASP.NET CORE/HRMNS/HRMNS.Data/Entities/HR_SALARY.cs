using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("HR_SALARY")]
    public class HR_SALARY : DomainEntity<int>, IDateTracking
    {
        public HR_SALARY()
        {

        }

        public decimal LivingAllowance { get; set; } // phu cap đời sống
        public decimal PositionAllowance { get; set; }
        public decimal AbilityAllowance { get; set; }
        public decimal FullAttendanceSupport { get; set; }
        public decimal SeniorityAllowance { get; set; }
        public decimal HarmfulAllowance { get; set; }
        public decimal IncentiveBase { get; set; }
        public decimal IncentiveLanguage { get; set; }
        public decimal IncentiveTechnical { get; set; }
        public decimal IncentiveOther { get; set; }

        [StringLength(4)]
        public string Year { get; set; }

        public decimal IncentiveSixMonth1 { get; set; }
        public decimal IncentiveSixMonth2 { get; set; }

        public decimal CI_SixMonth1 { get; set; }
        public decimal CI_SixMonth2 { get; set; }

        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [ForeignKey("MaNV")]
        public virtual HR_NHANVIEN HR_NHANVIEN { get; set; }
    }
}
