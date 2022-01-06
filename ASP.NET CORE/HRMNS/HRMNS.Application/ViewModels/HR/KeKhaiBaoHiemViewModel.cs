using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.HR
{
    public class KeKhaiBaoHiemViewModel
    {
        public int Id { get; set; } 

        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(50)]
        public string CheDoBH { get; set; }

        [StringLength(50)]
        public string NgayBatDau { get; set; }

        [StringLength(50)]
        public string NgayKetThuc { get; set; }

        [StringLength(50)]
        public string NgayThanhToan { get; set; }

        public double? SoTienThanhToan { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public CheDoBaoHiemViewModel HR_CHEDOBH { get; set; }

        public NhanVienViewModel HR_NHANVIEN { get; set; }
    }
}
