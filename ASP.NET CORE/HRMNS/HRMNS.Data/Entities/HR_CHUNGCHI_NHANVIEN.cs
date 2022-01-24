using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("HR_CHUNGCHI_NHANVIEN")]
    public class HR_CHUNGCHI_NHANVIEN : DomainEntity<int>, IDateTracking
    {
        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(50)]
        public string MaChungChi { get; set; }

        public string NoiCap { get; set; }

        [StringLength(50)]
        public string NgayCap { get; set; }

        [StringLength(50)]
        public string NgayHetHan { get; set; }

        [StringLength(250)]
        public string ChuyenMon { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [ForeignKey("MaChungChi")]
        public virtual CHUNG_CHI CHUNG_CHI { get; set; }

        [ForeignKey("MaNV")]
        public virtual HR_NHANVIEN HR_NHANVIEN { get; set; }

    }
}
