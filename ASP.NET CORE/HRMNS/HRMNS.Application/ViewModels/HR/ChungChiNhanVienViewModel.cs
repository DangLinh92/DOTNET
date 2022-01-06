using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.HR
{
    public class ChungChiNhanVienViewModel
    {
        public int Id { get; set; }

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

        public ChungChiViewModel CHUNG_CHI { get; set; }

        public NhanVienViewModel HR_NHANVIEN { get; set; }
    }
}
