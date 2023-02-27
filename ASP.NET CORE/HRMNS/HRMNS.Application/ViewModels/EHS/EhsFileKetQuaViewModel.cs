using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.EHS
{
    public class EhsFileKetQuaViewModel
    {
        public EhsFileKetQuaViewModel()
        {
            
        }

        public string MaNgayChiTiet { get; set; }
        public string TenDeMuc { get; set; }
        public string ThoiGianBatDau { get; set; } 
        public string ThoiGianKetThuc { get; set; } 
        public string NoiDung { get; set; }
        public string NguoiPhuTrach { get; set; }

        public string Status { get; set; }

        public string UrlFile { get; set; }

    }
}
