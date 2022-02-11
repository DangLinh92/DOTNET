using HRMNS.Application.ViewModels.HR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface ITinhTrangHoSoService : IDisposable
    {
        TinhTrangHoSoViewModel Add(TinhTrangHoSoViewModel nhanVienVm);

        void Update(TinhTrangHoSoViewModel nhanVienVm);

        void Delete(int id);

        List<TinhTrangHoSoViewModel> GetAll();
        TinhTrangHoSoViewModel GetById(int id);

        TinhTrangHoSoViewModel GetByMaNV(string id);
        void Save();
    }
}
