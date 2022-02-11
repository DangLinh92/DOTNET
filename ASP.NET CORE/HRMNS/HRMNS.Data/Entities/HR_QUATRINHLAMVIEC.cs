using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("HR_QUATRINHLAMVIEC")]
    public class HR_QUATRINHLAMVIEC : DomainEntity<int>, IDateTracking
    {
        public HR_QUATRINHLAMVIEC()
        {

        }

        public HR_QUATRINHLAMVIEC(string maNV,string tieuDe,string note,string thoigianBatDau,string thoigianKetThuc)
        {
            MaNV = maNV;
            TieuDe = tieuDe;
            Note = note;
            ThơiGianBatDau = thoigianBatDau;
            ThoiGianKetThuc = thoigianKetThuc;
        }

        [Required]
        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(500)]
        public string TieuDe { get; set; }

        public string Note { get; set; }

        [StringLength(50)]
        public string ThơiGianBatDau { get; set; }

        [StringLength(50)]
        public string ThoiGianKetThuc { get; set; }

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
