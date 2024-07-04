using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities.Payroll
{
    [Table("HR_CHECK_POINT")]
    public class HR_CHECK_POINT : DomainEntity<int>, IDateTracking
    {
        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(50)]
        public string GradeCHE { get; set; }

        [StringLength(50)]
        public string TeamCHE { get; set; }

        [StringLength(50)]
        public string RSCHE { get; set; }

        [StringLength(50)]
        public string Year { get; set; }
        public int CheNumber { get; set; }

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
