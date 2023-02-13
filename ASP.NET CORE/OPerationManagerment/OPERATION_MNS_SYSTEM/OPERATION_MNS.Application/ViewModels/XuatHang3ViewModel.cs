using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels
{
    public class XuatHang3ViewModel
    {
        public string Key { get => Module + Model; }
        public string Module { get; set; }
        public string Model { get; set; }
        public double WaferQty { get; set; }
        public double? ChipQty { get; set; }
        public string GhiChu { get; set; }
    }

    public class XuatHang2ViewModel
    {
        public string Key { get => DateTime.Parse(Ngay).ToString("yyyyMMddHHmmss") + Module + Model + CasstteID; }
        public int STT { get; set; }
        public string Ngay { get; set; }
        public string Module { get; set; }
        public string Model { get; set; }
        public string CasstteID { get; set; }
        public double WaferQty { get; set; }
        public double? ChipQty { get; set; }
        public string WaferID { get; set; }
        public string NguoiXuat { get; set; }
        public string KetQuaFAKiemTra { get; set; }
        public string NguoiKiemTra { get; set; }
        public string NguoiNhan { get; set; }
        public string GhiChu { get; set; }
    }

    public class XuatHang1ViewModel
    {
        public string Key { get => DateTime.Parse(NgayXuat).ToString("yyyyMMddHHmmss") + LotID; }
        public int STT { get; set; }
        public string NgayXuat { get; set; }
        public string LotID { get; set; }
        public string Module { get; set; }
        public string WaferID { get; set; }
        public string CasstteID { get; set; }
        public string Model { get; set; }
        public double? DefaultChipQty { get; set; }
        public double? ChipMesQty { get; set; }
        public double? ChipMapQty { get; set; }
        public double? DiffMapMes { get; set; }
        public double? Rate { get; set; }
        public string VanDeDacBiet { get; set; }
    }

    public class XuatHangViewModel
    {
        public List<XuatHang3ViewModel> XuatHang3ViewModels { get; set; }
        public List<XuatHang2ViewModel> XuatHang2ViewModels { get; set; }
        public List<XuatHang1ViewModel> XuatHang1ViewModels { get; set; }
        public List<PostOpeationShippingViewModel> DataXH { get; set; }
        public XuatHangViewModel()
        {
            XuatHang3ViewModels = new List<XuatHang3ViewModel>();
            XuatHang2ViewModels = new List<XuatHang2ViewModel>();
            XuatHang1ViewModels = new List<XuatHang1ViewModel>();
            DataXH = new List<PostOpeationShippingViewModel>();
        }
    }
}
