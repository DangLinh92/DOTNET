using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("HR_HOPDONG")]
    public class HR_HOPDONG : DomainEntity<string>
    {
        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(500)]
        public string TenHD { get; set; }

        public int? LoaiHD { get; set; }

        [StringLength(50)]
        public string NgayTao { get; set; }

        [StringLength(50)]
        public string NgayKy { get; set; }

        [StringLength(50)]
        public string NgayHieuLuc { get; set; }

        [StringLength(50)]
        public string NgayHetHieuLuc { get; set; }

        [StringLength(50)]
        public string ChucDanh { get; set; }

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

        [StringLength(10)]
        public string IsDelete { get; set; }

        [ForeignKey("LoaiHD")]
        public virtual HR_LOAIHOPDONG HR_LOAIHOPDONG { get; set; }

        [ForeignKey("MaNV")]
        public virtual HR_NHANVIEN HR_NHANVIEN { get; set; }
    }
}
