using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.ViewModels.Time_Attendance
{
    public class DeNghiLamThemGioModel
    {
        public string MaNV { get; set; }
        public string TenNV { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        private string _duration;
        public string Duration 
        {
            get
            {
                if(!string.IsNullOrEmpty(_duration) && double.TryParse(_duration,out _))
                {
                    return Math.Round(decimal.Parse(_duration), 1).ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                _duration = value;
            }
        }
        public string NgayDangKy { get; set; }
        public string BoPhan { get; set; }
        public string Note { get; set; }
    }
}
