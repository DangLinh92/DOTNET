using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    public class HR_THANHTOAN_NGHIVIEC : DomainEntity<Guid>, IDateTracking
    {
        [StringLength(50)]
        public string MaNV { get; set; }

        // đc xác nhận chi trả
        public bool IsPay { get; set; }

        [StringLength(50)]
        public string Month { get; set; }

        // Đã chi trả hay chưa
        public bool IsPayed { get; set; }

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
