using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.HR
{
    public class CheckPointViewModel
    {
        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(50)]
        public string TenNV { get; set; }

        [StringLength(50)]
        public string ChucDanh { get; set; }

        [StringLength(50)]
        public string Dept2 { get; set; }

        [StringLength(50)]
        public string Sex { get; set; }

        [StringLength(50)]
        public string NgaySinh { get; set; }

        public string NgayVao { get; set; }

        public int TimeWorking { get; set; }

        [StringLength(50)]
        public string GradeCHE { get; set; }

        [StringLength(50)]
        public string TeamCHE { get; set; }

        // result : kết quả
        [StringLength(50)]
        public string RSCHE { get; set; }

        [StringLength(50)]
        public string Year { get; set; }
        public int CheNumber { get; set; }
    }
}
