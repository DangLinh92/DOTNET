using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("NHANVIEN_CALAMVIEC")]
    public class NHANVIEN_CALAMVIEC : DomainEntity<int>, IDateTracking
    {
        [StringLength(50)]
        public string MaNV { get; set; }

         [StringLength(50)]
        public string Danhmuc_CaLviec { get; set; }

        [StringLength(50)]
        public string BatDau_TheoCa { get; set; }

        [StringLength(50)]
        public string KetThuc_TheoCa { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [ForeignKey("Danhmuc_CaLviec")]
        public virtual DM_CA_LVIEC DM_CA_LVIEC { get; set; }

        [ForeignKey("MaNV")]
        public virtual HR_NHANVIEN HR_NHANVIEN { get; set; }
    }
}
