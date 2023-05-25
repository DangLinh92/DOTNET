using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("NHANVIEN_INFOR_EX")]
    public class NHANVIEN_INFOR_EX : DomainEntity<int>, IDateTracking
    {
        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(50)]
        public string MaBoPhanEx { get; set; }

        [StringLength(50)]
        public string Grade { get; set; }

        public int Year { get; set; }

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
