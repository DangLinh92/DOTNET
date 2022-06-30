using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.ViewModels.HR
{
    public class TongHopNhanSuDailyViewModel
    {
        public TongHopNhanSuDailyViewModel()
        {
            CaLamViec_Value = new List<CaLamViecSL>();
        }
        public string BoPhan { get; set; }
        public double TongNV { get; set; }
        public double NghiTS { get; set; }
        public string NgayBaoCao { get; set; }
        public List<CaLamViecSL> CaLamViec_Value { get; set; }
    }

    public class CaLamViecSL
    {
        public CaLamViecSL()
        {
            ThongTins = new List<TrucTiepGianTiepSL>();
        }

        public string CalamViec { get; set; }
        public List<TrucTiepGianTiepSL> ThongTins { get; set; }
    }

    public class TrucTiepGianTiepSL
    {
        public string TrucTiepGianTiep { get; set; }
        public string ChucVu { get; set; }
        public double TongSoNguoi { get; set; }
        public double SoNguoiLamViec { get; set; }
        public double DiMuon { get; set; }
        public double VeSom { get; set; }
        public double NghiPhep { get; set; }
        public double NghiKhongLuong { get; set; }
        public double NghiKhongThongBao { get; set; }
        public double NghiHuongLuong70 { get; set; }
        public double NghiDacBiet { get; set; }
        public double DiCongTac { get; set; }
        public double NghiViec { get; set; }
        public double NghiOm { get; set; }
        public double NghiLe { get; set; }
        public string Note { get; set; }
        public int order { get; set; }
    }
}
