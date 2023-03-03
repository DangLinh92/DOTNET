using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.EHS
{
    public class EhsHangMucNGViewModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string MaNgayChiTiet { get; set; }

        [StringLength(500)]
        public string HangMucNG { get; set; }

        [StringLength(1000)]
        public string NoiDungVanDeNG { get; set; }

        [StringLength(500)]
        public string NguyenNhan { get; set; }

        [StringLength(1000)]
        public string DoiSachCaiTien { get; set; }

        [StringLength(250)]
        public string TinhHinhCaiTien { get; set; }

        [StringLength(250)]
        public string DeMuc { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }
    }
}
