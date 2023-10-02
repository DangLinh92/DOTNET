﻿using System;
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
        public string ThangSuDung { get; set; }
    }

    public class TongHopBoPhan
    {
        public string BoPhan { get; set; }
        public string RowTital { get; set; }
        public string ColumnTital { get; set; }
        public double SoTien_SD { get; set; }
        public string Month { get; set; }
    }

    public class ItemValue
    {
        public ItemValue(string key,string value)
        {
            Key = key; Value = value;
        }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
