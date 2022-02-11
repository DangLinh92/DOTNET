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

        public bool SoYeuLyLich { get; set; }

        public bool CMTND { get; set; }

        public bool SoHoKhau { get; set; }

        public bool GiayKhaiSinh { get; set; }

        public bool BangTotNghiep { get; set; }

        public bool XacNhanDanSu { get; set; }

        public bool AnhThe { get; set; }

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
