using AutoMapper;
using Microsoft.AspNetCore.Http;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Data.EF;
using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Infrastructure.Interfaces;
using OPERATION_MNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;

namespace OPERATION_MNS.Application.Implementation
{
    public class StayLotListService : BaseService, IStayLotListService
    {
        private IRespository<STAY_LOT_LIST, int> _StayLotListRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StayLotListService(IRespository<STAY_LOT_LIST, int> StayLotListRepository, IUnitOfWork unitOfWork, IMapper mapper,
                              IHttpContextAccessor httpContextAccessor)
        {
            _StayLotListRepository = StayLotListRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public StayLotListDisPlayViewModel GetStayLotList()
        {
            ResultDB resultDB = _StayLotListRepository.ExecProceduce2("PKG_BUSINESS@GET_STAY_LOT_LIST", new Dictionary<string, string>());
            StayLotListDisPlayViewModel result = new StayLotListDisPlayViewModel();
            if (resultDB.ReturnInt == 0)
            {
                int i = 0;
                foreach (DataRow row in resultDB.ReturnDataSet.Tables[0].Rows)
                {
                    result.StayLotListSumViews.Add(new StayLotListSumViewModel()
                    {
                        Index = ++i,
                        Model = row["Material"].NullString(),
                        QtyChip = decimal.Parse(row["CHIP_QTY"].IfNullIsZero()),
                        QtyWF = decimal.Parse(row["WF_QTY"].IfNullIsZero())
                    });
                }

                i = 0;
                foreach (DataRow row in resultDB.ReturnDataSet.Tables[1].Rows)
                {
                    result.StayLotListTenLoiViews.Add(new StayLotListSumViewModel()
                    {
                        Index = ++i,
                        TenLoi = row["TenLoi"].NullString(),
                        QtyChip = decimal.Parse(row["CHIP_QTY"].IfNullIsZero()),
                        QtyWF = decimal.Parse(row["WF_QTY"].IfNullIsZero()),
                        PhanLoaiLoi = row["PhanLoaiLoi"].NullString()
                    });
                }

                foreach (DataRow row in resultDB.ReturnDataSet.Tables[2].Rows)
                {
                    result.StayLotList_Ex_ViewModels.Add(new StayLotList_Ex_ViewModel()
                    {
                        CassetteId = row["Cassette ID"].NullString(),
                        LotId = row["Lot ID"].NullString(),
                        Material = row["Material"].NullString(),
                        OperationID = row["Operation ID"].NullString(),
                        OperationName = row["Operation Name"].NullString(),
                        StayDay = decimal.Parse(row["Stay Day"].IfNullIsZero()),
                        ChipQty = decimal.Parse(row["Chip Qty"].IfNullIsZero()),
                        ERPProductOrder = row["ERP Product Order"].NullString(),
                        FABLotID = row["FAB Lot ID"].NullString(),
                        HoldTime = row["Hold Time"].NullString(),
                        HoldCode = row["Hold Code"].NullString(),
                        HoldUserName = row["Hold User Name"].NullString(),
                        HoldComment = row["Hold Comment"].NullString(),
                        TenLoi = row["TenLoi"].NullString(),
                        PhuongAnXuLy = row["PhuongAnXuLy"].NullString(),
                        NguoiXuLy = row["NguoiXuLy"].NullString(),
                        UpdateByCassetteId = false,
                        history_seq = double.Parse(row["HISTORY_SEQ"].IfNullIsZero()),
                        PhanLoaiLoi = row["PhanLoaiLoi"].NullString()
                    });
                }
            }

            return result;
        }

        public StayLotList_Ex_ViewModel UpdateLotInfo(StayLotList_Ex_ViewModel model, StayLotListDisPlayViewModel stayLotList)
        {
            if (!model.UpdateByCassetteId)
            {
                var lot = _StayLotListRepository.FindSingle(x => x.LotId == model.LotId && x.CassetteId == model.CassetteId && x.History_seq == model.history_seq);
                if (lot != null)
                {
                    lot.TenLoi = model.TenLoi.NullString();
                    lot.PhuongAnXuLy = model.PhuongAnXuLy.NullString();
                    lot.NguoiXuLy = model.NguoiXuLy.NullString();
                    lot.UserModified = GetUserId();
                    lot.PhanLoaiLoi = model.PhanLoaiLoi.NullString();
                    _StayLotListRepository.Update(lot);
                }
                else
                {
                    STAY_LOT_LIST en = new STAY_LOT_LIST(model.LotId.NullString(), model.PhuongAnXuLy.NullString(), model.TenLoi.NullString(), model.NguoiXuLy.NullString(), model.CassetteId.NullString(), model.history_seq,model.PhanLoaiLoi);
                    en.UserModified = GetUserId();
                    en.UserCreated = GetUserId();
                    _StayLotListRepository.Add(en);
                }
            }
            else
            {
                // update bath theo casseteId
                string cassetteId = model.CassetteId;
                STAY_LOT_LIST lot;
                foreach (var item in stayLotList.StayLotList_Ex_ViewModels.FindAll(x => x.CassetteId == cassetteId))
                {
                    lot = _StayLotListRepository.FindSingle(x => x.LotId == item.LotId && x.CassetteId == item.CassetteId && x.History_seq == item.history_seq);
                    if (lot != null)
                    {
                        lot.TenLoi = model.TenLoi.NullString();
                        lot.PhuongAnXuLy = model.PhuongAnXuLy.NullString();
                        lot.NguoiXuLy = model.NguoiXuLy.NullString();
                        lot.UserModified = GetUserId();
                        lot.PhanLoaiLoi = model.PhanLoaiLoi.NullString();
                        _StayLotListRepository.Update(lot);
                    }
                    else
                    {
                        lot = new STAY_LOT_LIST(item.LotId.NullString(), model.PhuongAnXuLy.NullString(), model.TenLoi.NullString(), model.NguoiXuLy.NullString(), item.CassetteId.NullString(), item.history_seq, model.PhanLoaiLoi);
                        lot.UserModified = GetUserId();
                        lot.UserCreated = GetUserId();
                        _StayLotListRepository.Add(lot);
                    }
                }
            }

            _unitOfWork.Commit();
            return model;
        }

        public List<STAY_LOT_LIST_HISTORY> GetStayLotListHistory(string cassetteId, string lotId, string timeFrom, string timeTo)
        {
            List<STAY_LOT_LIST_HISTORY> rs = new List<STAY_LOT_LIST_HISTORY>(); //((EFUnitOfWork)_unitOfWork).DBContext().STAY_LOT_LIST_HISTORY.ToList();

            if (!string.IsNullOrEmpty(cassetteId))
            {
                rs = ((EFUnitOfWork)_unitOfWork).DBContext().STAY_LOT_LIST_HISTORY.Where(x => x.CassetteId == cassetteId).ToList();
            }

            if (!string.IsNullOrEmpty(lotId))
            {
                if (rs.Count > 0)
                    rs = rs.Where(x => x.LotId == lotId).ToList();
                else
                {
                    rs = ((EFUnitOfWork)_unitOfWork).DBContext().STAY_LOT_LIST_HISTORY.Where(x => x.LotId == lotId).ToList();
                }
            }

            if (!string.IsNullOrEmpty(timeFrom))
            {
                timeFrom = DateTime.Parse(timeFrom).ToString("yyyyMMdd") + "000000";

                if (rs.Count > 0)
                    rs = rs.Where(x => x.HoldTime.CompareTo(timeFrom) >= 0).ToList();
                else
                {
                    rs = ((EFUnitOfWork)_unitOfWork).DBContext().STAY_LOT_LIST_HISTORY.Where(x => x.HoldTime.CompareTo(timeFrom) >= 0).ToList();
                }
            }

            if (!string.IsNullOrEmpty(timeTo))
            {
                timeTo = DateTime.Parse(timeTo).ToString("yyyyMMdd") + "235959";

                if (rs.Count > 0)
                    rs = rs.Where(x => x.HoldTime.CompareTo(timeTo) <= 0).ToList();
                else
                {
                    rs = ((EFUnitOfWork)_unitOfWork).DBContext().STAY_LOT_LIST_HISTORY.Where(x => x.HoldTime.CompareTo(timeFrom) >= 0 && x.HoldTime.CompareTo(timeTo) <= 0).ToList();
                }
            }

            return rs;
        }

        public List<StayLotList_Ex_ViewModel> GetStayLotListByModel(string model, string operation)
        {
            List<StayLotList_Ex_ViewModel> rs = new List<StayLotList_Ex_ViewModel>();

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("A_MODEL", model);
            dic.Add("A_OPERATION", operation);
            ResultDB resultDB = _StayLotListRepository.ExecProceduce2("GET_STAY_LOT_BY_MODEL", dic);

            if (resultDB.ReturnInt == 0)
            {
                DataTable data = resultDB.ReturnDataSet.Tables[0];
                if (data.Rows.Count > 0)
                {
                    StayLotList_Ex_ViewModel stayVm; ;
                    int i = 0;
                    int priority = 1;
                    decimal stayDayOld = decimal.Parse(data.Rows[0]["Stay Day"].IfNullIsZero());
                    foreach (DataRow item in data.Rows)
                    {
                        stayVm = new StayLotList_Ex_ViewModel()
                        {
                            CassetteId = item["Cassette ID"].NullString(),
                            Material = item["Material"].NullString(),
                            OperationID = item["Operation ID"].NullString(),
                            OperationName = item["Operation Name"].NullString(),
                            StayDay = decimal.Parse(item["Stay Day"].IfNullIsZero()),
                            ChipQty = decimal.Parse(item["Chip Qty"].IfNullIsZero()),
                            WaferQty = decimal.Parse(item["Wafer Qty"].IfNullIsZero()),
                            LotStatus = item["LOT_STATUS"].NullString(),
                            STT = ++i
                        };

                        if (stayDayOld == stayVm.StayDay)
                        {
                            stayVm.Priority = priority;
                        }
                        else if (stayDayOld > stayVm.StayDay)
                        {
                            stayVm.Priority = ++priority;
                        }

                        rs.Add(stayVm);

                        stayDayOld = decimal.Parse(item["Stay Day"].IfNullIsZero());
                    }
                }
            }
            return rs;
        }
    }
}
