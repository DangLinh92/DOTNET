using HRMNS.Application.ViewModels.HR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IHopDongService : IDisposable
    {
        HopDongViewModel Add(HopDongViewModel hopDongVm);

        void Update(HopDongViewModel hopDongVm);

        void UpdateSingle(HopDongViewModel nhanVienVm);

        void Delete(int id);

        List<HopDongViewModel> GetAll();

        HopDongViewModel GetById(int id);

        HopDongViewModel GetByMaHD(string maHd);

        List<HopDongViewModel> GetNVHetHanHD();

        void Save();
    }
}
