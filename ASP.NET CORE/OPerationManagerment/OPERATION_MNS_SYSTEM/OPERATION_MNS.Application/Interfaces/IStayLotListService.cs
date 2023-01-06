using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.Interfaces
{
    public interface IStayLotListService : IDisposable
    {
        StayLotListDisPlayViewModel GetStayLotList();

        StayLotList_Ex_ViewModel UpdateLotInfo(StayLotList_Ex_ViewModel model, StayLotListDisPlayViewModel stayLotList);

        List<STAY_LOT_LIST_HISTORY> GetStayLotListHistory(string cassetteId, string lotId, string timeFrom, string timeTo);

        List<StayLotList_Ex_ViewModel> GetStayLotListByModel(string model, string operation);
    }
}
