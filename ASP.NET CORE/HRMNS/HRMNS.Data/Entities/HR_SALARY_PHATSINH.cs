using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("HR_SALARY_PHATSINH")]
    public class HR_SALARY_PHATSINH : DomainEntity<int>, IDateTracking
    {
        public int DanhMucPhatSinh { get; set; }
      
        public double SoTien { get; set; }

        public DateTime? ThoiGianApDung_From { get; set; }

        public DateTime? ThoiGianApDung_To { get; set; }

        [StringLength(50)]
        public string FromTime { get; set; }

        [StringLength(50)]
        public string ToTime { get; set; }

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

        public Guid Key { get; set; }

        [ForeignKey("MaNV")]
        public virtual HR_NHANVIEN HR_NHANVIEN { get; set; }

        [ForeignKey("DanhMucPhatSinh")]
        public virtual HR_SALARY_DANHMUC_PHATSINH HR_SALARY_DANHMUC_PHATSINH { get; set; }
    }
}
