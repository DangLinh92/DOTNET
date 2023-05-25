using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("HR_SALARY_DANHMUC_PHATSINH")]
    public class HR_SALARY_DANHMUC_PHATSINH : DomainEntity<int>, IDateTracking
    {
        public HR_SALARY_DANHMUC_PHATSINH()
        {
            HR_SALARY_PHATSINH = new HashSet<HR_SALARY_PHATSINH>();
        }

        [StringLength(50)]
        public string DanhMuc { get; set; }

        [StringLength(50)]
        public string KeyDanhMuc { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public ICollection<HR_SALARY_PHATSINH> HR_SALARY_PHATSINH { get; set; }
    }
}
