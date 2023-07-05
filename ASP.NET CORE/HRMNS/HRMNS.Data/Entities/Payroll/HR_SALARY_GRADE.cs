using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities.Payroll
{
    [Table("HR_SALARY_GRADE")]
    public class HR_SALARY_GRADE : DomainEntity<string>, IDateTracking
    {
        public double BasicSalaryStandard { get; set; }
        public double IncentiveLanguage { get; set; }

        public double BasicSalary { get; set; }
        public double LivingAllowance { get; set; } // phu cap đời sống
        public double IncentiveStandard { get; set; }
        public double AttendanceAllowance { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }
    }
}
