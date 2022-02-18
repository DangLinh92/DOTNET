using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace HRMNS.Application.ViewModels.HR
{
    public class QuaTrinhLamViecViewModel
    {
        public int Id { get; set; }

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

        public NhanVienViewModel HR_NHANVIEN { get; set; }
    }
}
