using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.HR
{
    public class HopDongViewModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string MaHD { get; set; }

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
        public int DayNumberNoti { get; set; }

        public LoaiHopDongViewModel HR_LOAIHOPDONG { get; set; }

        public NhanVienViewModel HR_NHANVIEN { get; set; }
    }
}
