using OPERATION_MNS.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels
{
    public class StayLotListDisPlayViewModel
    {
        public StayLotListDisPlayViewModel()
        {
            StayLotListSumViews = new List<StayLotListSumViewModel>();
            StayLotList_Ex_ViewModels = new List<StayLotList_Ex_ViewModel>();
            StayLotListTenLoiViews = new List<StayLotListSumViewModel>();
        }

        public List<StayLotListSumViewModel> StayLotListSumViews { get; set; }
        public List<StayLotListSumViewModel> StayLotListTenLoiViews { get; set; }
        public List<StayLotList_Ex_ViewModel> StayLotList_Ex_ViewModels { get; set; }
    }

    public class StayLotListSumViewModel
    {
        public int Index { get; set; }
        public string Model { get; set; }
        public string CassetteId { get; set; }
        public decimal QtyWF { get; set; }
        public decimal QtyChip { get; set; }
        public string TenLoi { get; set; }
        public string PhanLoaiLoi { get; set; }
        public string OperationName { get; set; }
    }

    public class StayLotListViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string LotId { get; set; }

        [StringLength(1000)]
        public string PhuongAnXuLy { get; set; }

        [StringLength(250)]
        public string TenLoi { get; set; }

        [StringLength(50)]
        public string CassetteId { get; set; }

        [StringLength(50)]
        public string NguoiXuLy { get; set; }

        public double history_seq { get; set; }

        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }
    }

    public class StayLotList_Ex_ViewModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string LotId { get; set; }

        [StringLength(1000)]
        public string PhuongAnXuLy { get; set; }

        [StringLength(250)]
        public string TenLoi { get; set; }

        [StringLength(50)]
        public string CassetteId { get; set; }

        [StringLength(50)]
        public string Material { get; set; }

        [StringLength(50)]
        public string OperationID { get; set; }

        [StringLength(100)]
        public string OperationName { get; set; }

        public decimal StayDay { get; set; }

        public decimal ChipQty { get; set; }

        public string ERPProductOrder { get; set; }

        public string FABLotID { get; set; }

        public string HoldTime { get; set; }
        public string LotCategory { get; set; }

        public string HoldCode { get; set; }

        public string HoldUserName { get; set; }
        public string HoldUser { get; set; }

        public string HoldComment { get; set; }

        [StringLength(50)]
        public string NguoiXuLy { get; set; }

        public string UserModified { get; set; }

        public bool UpdateByCassetteId { get; set; }

        public double history_seq { get; set; }

        public string Key { get => LotId + "-" + CassetteId + "-" + history_seq; }

        public int Priority { get; set; }// ưu tiên

        public decimal WaferQty { get; set; }

        public int STT { get; set; }
        public string LotStatus { get; set; }

        public string PhanLoaiLoi { get; set; }
    }

    public class ViewHistoryHoldLotModel {

        public ViewHistoryHoldLotModel()
        {
            STAY_LOT_LIST_HISTORY_DATA = new List<STAY_LOT_LIST_HISTORY>();
            STAY_LOT_LIST_HISTORY_WLP2_DATA = new List<STAY_LOT_LIST_HISTORY_WLP2>();
        }
       public  List<STAY_LOT_LIST_HISTORY> STAY_LOT_LIST_HISTORY_DATA;
       public  List<STAY_LOT_LIST_HISTORY_WLP2> STAY_LOT_LIST_HISTORY_WLP2_DATA;
        public string LotId { get; set; }
        public string CasseteId { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
    }

    public class StayLotListByModel
    {
        public string Model { get; set; }
        public decimal QtyWF { get; set; }
        public decimal QtyChip { get; set; }
        public string OperationName { get; set; }
        public string OperationId { get; set; }
    }
}
