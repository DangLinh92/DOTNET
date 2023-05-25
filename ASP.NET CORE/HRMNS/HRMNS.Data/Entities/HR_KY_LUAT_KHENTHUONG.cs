using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("HR_KY_LUAT_KHENTHUONG")]
    public class HR_KY_LUAT_KHENTHUONG : DomainEntity<int>, IDateTracking
    {
        public HR_KY_LUAT_KHENTHUONG()
        {
        }

        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(500)]
        public string LoiViPham { get; set; }

        [StringLength(500)]
        public string HinhThucKyLuat { get; set; }

        public DateTime ThoiGianViPham { get; set; }

        [StringLength(500)]
        public string PhuongThucXuLy { get; set; }

        [StringLength(50)]
        public string PhanLoai { get; set; } // ky luat.khen thuong

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
