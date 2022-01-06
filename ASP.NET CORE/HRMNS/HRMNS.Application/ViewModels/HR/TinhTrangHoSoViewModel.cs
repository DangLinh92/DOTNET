using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.HR
{
    public class TinhTrangHoSoViewModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(50)]
        public string SoYeuLyLich { get; set; }

        [StringLength(50)]
        public string CMTND { get; set; }

        [StringLength(50)]
        public string SoHoKhau { get; set; }

        [StringLength(50)]
        public string GiayKhaiSinh { get; set; }

        [StringLength(50)]
        public string BangTotNghiep { get; set; }

        [StringLength(50)]
        public string XacNhanDanSu { get; set; }

        [StringLength(50)]
        public string AnhThe { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public NhanVienViewModel HR_NHANVIEN { get; set; }
    }
}
