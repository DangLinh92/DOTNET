using HRMNS.Application.ViewModels.HR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IHRLoaiHopDongService
    {
        List<LoaiHopDongViewModel> GetAll();
    }
}
