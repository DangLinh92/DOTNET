using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.ViewModels.EHS
{
    public class ChiTietNoiDungChiPhiViewModel
    {
        public ChiTietNoiDungChiPhiViewModel()
        {
            NoiDungChiTiet = new List<EhsNoiDungKeHoachViewModel>();
            ChiPhi = new List<EhsChiPhiByMonthViewModel>();
        }

        public List<EhsNoiDungKeHoachViewModel> NoiDungChiTiet { get; set; }
        public List<EhsChiPhiByMonthViewModel> ChiPhi { get; set; }
    }
}
