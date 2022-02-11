using HRMNS.Application.ViewModels.HR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IQuatrinhLamViecService : IDisposable
    {
        QuaTrinhLamViecViewModel Add(QuaTrinhLamViecViewModel qtrinhLviecVm);

        void Update(QuaTrinhLamViecViewModel qtrinhLviecVm);

        void Delete(int id);

        List<QuaTrinhLamViecViewModel> GetAll();

        List<QuaTrinhLamViecViewModel> GetAll(string keyword);

        QuaTrinhLamViecViewModel GetById(int id);

        void Save();
    }
}
