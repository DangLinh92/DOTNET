using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.ViewModels.EHS
{
    public class EhsDanhMucKeHoachPageViewModel
    {
        public EhsDanhMucKeHoachPageViewModel()
        {
            EhsDMKeHoachViewModels = new List<EhsDMKeHoachViewModel>();
            NoiDungKeHoachViewModels = new List<NoiDungKeHoachModel>();
            EhsNoiDungKeHoachViewModels = new List<EhsNoiDungKeHoachViewModel>();
        }
        public Guid? MaKeHoachActive { get; set; }
        public List<EhsDMKeHoachViewModel> EhsDMKeHoachViewModels { get; set; }
        public List<NoiDungKeHoachModel> NoiDungKeHoachViewModels { get; set; }
        public List<EhsNoiDungKeHoachViewModel> EhsNoiDungKeHoachViewModels { get; set; }
    }

    public class NoiDungKeHoachModel
    {
        public Guid MaKeHoach { get; set; }
        public Guid MaDeMucKH { get; set; }
        public Guid MaNoiDung { get; set; }
        public string NoiDung { get; set; }

        public string TenKeDeMuc_VN { get; set; }
        public string TenKeDeMuc_KR { get; set; }
    }

    public class DeMucLuatDinh
    {
        public string TenDemuc { get; set; }
        public string LuatDinh { get; set; }
    }

    // su dung cho tong hop bao cao
    public class DemucKeHoach
    {
        public DemucKeHoach()
        {
            NoiDungs = new List<NoiDungDeMucKH>();
        }

        public Guid MaKeHoach { get; set; }
        public Guid MaDeMucKH { get; set; }
        public string TenDemuc { get; set; }
        public string LuatDinh { get; set; }

        public List<NoiDungDeMucKH> NoiDungs { get; set; }
    }

    public class NoiDungDeMucKH
    {
        public NoiDungDeMucKH()
        {
            NoiDungChiTiets = new List<EhsNoiDungKeHoachViewModel>();
        }

        public Guid MaNoiDung { get; set; }
        public string NoiDung { get; set; }
        public List<EhsNoiDungKeHoachViewModel> NoiDungChiTiets { get; set; }
    }

    public class TongHopKeHoachALL
    {
        public TongHopKeHoachALL()
        {
            TongHopKeHoachViewModels = new List<TongHopKeHoachViewModel>();
        }
        public List<TongHopKeHoachViewModel> TongHopKeHoachViewModels { get; set; }
        public TotalByYear TotalByYear { get; set; }
    }

    // Tổng hợp kế hoạch theo từng kế hoạch
    public class TongHopKeHoachViewModel
    {
        public TongHopKeHoachViewModel()
        {
            lstDeMucNoiDung = new List<DemucKeHoach>();
        }

        public Guid MaKeHoach { get; set; }
        public string TenKeHoach { get; set; }
        public string Year { get; set; }
        public string LuatDinhKeHoach { get; set; }

        public int OrderItem { get; set; }

        public List<DemucKeHoach> lstDeMucNoiDung { get; set; }
    }

    // Tổng hợp toàn bộ kế hoạch
    public class TotalByYear
    {
        public TotalByYear()
        {
            ItemByYears = new List<TotalAllItemByYear>();
        }
        public string Year { get; set; }

        public double TMonth_1 { get; set; }
        public double TMonth_2 { get; set; }
        public double TMonth_3 { get; set; }
        public double TMonth_4 { get; set; }
        public double TMonth_5 { get; set; }
        public double TMonth_6 { get; set; }
        public double TMonth_7 { get; set; }
        public double TMonth_8 { get; set; }
        public double TMonth_9 { get; set; }
        public double TMonth_10 { get; set; }
        public double TMonth_11 { get; set; }
        public double TMonth_12 { get; set; }

        public double ToTal
        {
            get
            {
                return TMonth_1 + TMonth_2 + TMonth_3 + TMonth_4 + TMonth_5 + TMonth_6 + TMonth_7 + TMonth_8 + TMonth_9 + TMonth_10 + TMonth_11 + TMonth_12;
            }
        }

        public List<TotalAllItemByYear> ItemByYears { get; set; }

    }

    public class TotalAllItemByYear
    {
        public TotalAllItemByYear()
        {

        }

        public Guid MaKeHoach { get; set; }
        public Guid MaDeMuc { get; set; }
        public Guid MaNoiDung { get; set; }

        public int OrderItem { get; set; }

        public string TenDeMuc { get; set; }
        public string TenNoiDung { get; set; }
        public string NhaThau { get; set; }
        public string ChuKy { get; set; }
        public double Month_1 { get; set; }
        public double Month_2 { get; set; }
        public double Month_3 { get; set; }
        public double Month_4 { get; set; }
        public double Month_5 { get; set; }
        public double Month_6 { get; set; }
        public double Month_7 { get; set; }
        public double Month_8 { get; set; }
        public double Month_9 { get; set; }
        public double Month_10 { get; set; }
        public double Month_11 { get; set; }
        public double Month_12 { get; set; }

        public double ToTal
        {
            get
            {
                return Month_1 + Month_2 + Month_3 + Month_4 + Month_5 + Month_6 + Month_7 + Month_8 + Month_9 + Month_10 + Month_11 + Month_12;
            }
        }
    }
}
