using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("CONGDOAN_NOT_JOIN")]
   public class CONGDOAN_NOT_JOIN : DomainEntity<int>, IDateTracking
    {
        [StringLength(50)]
        public string MaNV { get; set; }

        public DateTime? NgayBatDau { get; set; }

        [StringLength(250)]
        public string NguyenNhan { get; set; }

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
