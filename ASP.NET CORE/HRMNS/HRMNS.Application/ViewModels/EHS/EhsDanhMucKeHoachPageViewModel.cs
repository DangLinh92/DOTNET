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
        public Guid MaNoiDung{ get; set; }
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

        public List<DemucKeHoach> lstDeMucNoiDung { get; set; }
    }
}
