using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Application.ViewModels.Wlp2;
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


        // wlp2
        StayLotListDisPlayViewModel GetStayLotListWlp2();

        StayLotList_Ex_ViewModel UpdateLotInfoWlp2(StayLotList_Ex_ViewModel model, StayLotListDisPlayViewModel stayLotList);

        List<STAY_LOT_LIST_HISTORY_WLP2> GetStayLotListHistoryWlp2(string cassetteId, string lotId, string timeFrom, string timeTo);

        List<Stay_lot_list_priory_wlp2ViewModel> GetStayLotListByModelWlp2(string model, string operation);

        Stay_lot_list_priory_wlp2ViewModel UpdatePrioryLotIdWlp2(Stay_lot_list_priory_wlp2ViewModel model,int index);

        void Save();

    }
}
