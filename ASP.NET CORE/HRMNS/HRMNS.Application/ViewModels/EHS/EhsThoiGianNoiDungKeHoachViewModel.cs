using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.EHS
{
    public class EhsThoiGianNoiDungKeHoachViewModel
    {
        public int Id { get; set; }

        public Guid MaNoiDungKeHoach { get; set; }

        [StringLength(50)]
        public string Year { get; set; } // 2022

        [StringLength(50)]
        public string NgayThucHien { get; set; } // 2022-01-01

        [StringLength(50)]
        public string ThoiGian_ThucHien { get; set; } // 8h

        [StringLength(250)]
        public string ViTri { get; set; }

        public double SoLuong { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public EhsNoiDungKeHoachViewModel EHS_NOIDUNG_KEHOACH { get; set; }
    }
}
