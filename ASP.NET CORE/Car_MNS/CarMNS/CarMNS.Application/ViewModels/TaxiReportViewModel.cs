using System;
using System.Collections.Generic;
using System.Text;

namespace CarMNS.Application.ViewModels
{
    public class TaxiReportViewModel
    {
        public string BoPhan { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string TenNguoi { get; set; }
        public double SoTien { get; set; }
        public double SoNguoiDung { get; set; }
        public double SoLanSuDung { get; set; }
    }

    public class NguoiDungTaxi
    {
        public string MaNV { get; set; }
        public string TenNV { get; set; }
        public string BoPhan { get; set;}
        public double SoTien { get; set; }
        public double SoLanSuDung { get; set; }
    }

    public class TongHopBoPhan
    {
        public string BoPhan { get; set; }
        public string RowTital { get; set; }
        public double SoTien_SD { get; set; }
    }
}
