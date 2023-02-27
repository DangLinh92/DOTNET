using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.ViewModels.EHS
{
    public class EhsDanhMucKeHoachPageViewModel
    {
        public EhsDanhMucKeHoachPageViewModel()
        {
            EhsDMKeHoachViewModels = new List<EhsDMKeHoachViewModel>(); // ke hoach
        }
        public Guid? MaKeHoachActive { get; set; }
        public List<EhsDMKeHoachViewModel> EhsDMKeHoachViewModels { get; set; }
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

    public class TotalAllItemByYear
    {
        public TotalAllItemByYear()
        {

        }

        public string MaKeHoach { get; set; }
        public int OrderItem { get; set; }
        public int STT { get; set; }
        public string TenDeMuc { get; set; }
        public string TenNoiDung { get; set; }
        public string NhaThau { get; set; }
        public string ChuKy { get; set; }
        public string NguoiPhuTrach { get; set; }
        public string Year { get; set; }
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
