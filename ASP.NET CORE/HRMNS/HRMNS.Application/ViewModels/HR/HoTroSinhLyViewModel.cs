using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.HR
{
    public class HoTroSinhLyViewModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string MaNV { get; set; }

        public string TenNV { get; set; }

        public string BoPhan { get; set; }

        public double ThoiGianChuaNghi { get; set; }

        [StringLength(50)]
        public string Month { get; set; }

        public double SoTien { get; set; }
    }
}
