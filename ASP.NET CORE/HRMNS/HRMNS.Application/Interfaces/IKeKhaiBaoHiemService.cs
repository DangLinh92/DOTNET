using HRMNS.Application.ViewModels.HR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IKeKhaiBaoHiemService: IDisposable
    {
        List<KeKhaiBaoHiemViewModel> GetAll(string filter);
        KeKhaiBaoHiemViewModel Add(KeKhaiBaoHiemViewModel bhxhVm);
        void Update(KeKhaiBaoHiemViewModel bhxhVm);
        KeKhaiBaoHiemViewModel GetById(int id);
        void Delete(int id);
        void Save();
    }
}
