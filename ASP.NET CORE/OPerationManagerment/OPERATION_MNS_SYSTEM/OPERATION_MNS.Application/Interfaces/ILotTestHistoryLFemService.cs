using OPERATION_MNS.Application.ViewModels.Lfem;
using OPERATION_MNS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.Interfaces
{
    public interface ILotTestHistoryLFemService : IDisposable
    {
        List<LOT_TEST_HISTOTY_LFEM> GetAllData();
        LOT_TEST_HISTOTY_LFEM PostData(LOT_TEST_HISTOTY_LFEM en);
        LOT_TEST_HISTOTY_LFEM PutData(LOT_TEST_HISTOTY_LFEM en);
        void DeleteData(int id);
        LOT_TEST_HISTOTY_LFEM FindById(int id);

        List<WipLotListLFEMViewModel> GetWIPLotListLfem();

        void Save();
    }
}
