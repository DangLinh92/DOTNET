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
using OPERATION_MNS.Utilities.Common;
using OPERATION_MNS.Application.ViewModels.Wlp2;
using OPERATION_MNS.Application.ViewModels.Lfem;

namespace OPERATION_MNS.Application.Implementation
{
    public class StayLotListService : BaseService, IStayLotListService
    {
        private IRespository<STAY_LOT_LIST, int> _StayLotListRepository;
        private IRespository<STAY_LOT_LIST_LFEM, int> _StayLotListLFEMRepository;
        private IRespository<STAY_LOT_LIST_SAMPLE, int> _StayLotSampleListRepository;
        private IRespository<SETTING_ITEMS, string> _SettingItemRepository;
        private IRespository<STAY_LOT_LIST_WLP2, int> _StayLotListWLP2Repository;
        private IRespository<STAY_LOT_LIST_PRIORY_WLP2, int> _StayLotListPrioryWLP2Repository;
        private IRespository<STAY_LOT_LIST_PRIORY_LFEM, int> _StayLotListPrioryLFEMRepository;
        private IRespository<DAILY_PLAN_WLP2, int> _DailyPlanRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StayLotListService(IRespository<STAY_LOT_LIST_SAMPLE, int> stayLotSampleListRepository, IRespository<SETTING_ITEMS, string> SettingItemRepository, IRespository<STAY_LOT_LIST, int> StayLotListRepository, IRespository<STAY_LOT_LIST_WLP2, int> stayLotListWLP2Repository,
            IRespository<STAY_LOT_LIST_PRIORY_WLP2, int> stayLotListPrioryWLP2Repository,
            IRespository<DAILY_PLAN_WLP2, int> dailyPlanRepository,
            IUnitOfWork unitOfWork, IMapper mapper,
                              IHttpContextAccessor httpContextAccessor, IRespository<STAY_LOT_LIST_PRIORY_LFEM, int> stayLotListPrioryLFEMRepository, IRespository<STAY_LOT_LIST_LFEM, int> StayLotListLFEMRepository)
        {
            _SettingItemRepository = SettingItemRepository;
            _StayLotListPrioryWLP2Repository = stayLotListPrioryWLP2Repository;
            _StayLotListRepository = StayLotListRepository;
            _StayLotListWLP2Repository = stayLotListWLP2Repository;
            _DailyPlanRepository = dailyPlanRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _StayLotSampleListRepository = stayLotSampleListRepository;
            _StayLotListPrioryLFEMRepository = stayLotListPrioryLFEMRepository;
            _StayLotListLFEMRepository = StayLotListLFEMRepository;
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
                    STAY_LOT_LIST en = new STAY_LOT_LIST(model.LotId.NullString(), model.PhuongAnXuLy.NullString(), model.TenLoi.NullString(), model.NguoiXuLy.NullString(), model.CassetteId.NullString(), model.history_seq, model.PhanLoaiLoi);
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

                        //if (stayDayOld == stayVm.StayDay)
                        //{
                        //    stayVm.Priority = priority;
                        //}
                        //else if (stayDayOld >= stayVm.StayDay)
                        //{
                        //    stayVm.Priority = ++priority;
                        //}

                        stayVm.Priority = priority++;

                        rs.Add(stayVm);

                        stayDayOld = decimal.Parse(item["Stay Day"].IfNullIsZero());
                    }
                }
            }
            return rs;
        }


        #region WLP2
        public StayLotListDisPlayViewModel GetStayLotListWlp2()
        {
            ResultDB resultDB = _StayLotListWLP2Repository.ExecProceduce2("PKG_BUSINESS@GET_STAY_LOT_LIST_WLP2", new Dictionary<string, string>());
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
                        CassetteId = row["CassetteId"].NullString(),
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
                        OperationName = row["OperationName"].NullString()
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
                        HoldTime = row["Hold Time"].NullString(),
                        LotCategory = row["Lot Category"].NullString(),
                        HoldUserName = row["Hold User Name"].NullString(),
                        HoldUser = row["Hold User ID"].NullString(),
                        HoldComment = row["Hold Comment"].NullString(),
                        TenLoi = row["TenLoi"].NullString(),
                        UpdateByCassetteId = false,
                        history_seq = double.Parse(row["HISTORY_SEQ"].IfNullIsZero())
                    });
                }
            }

            return result;
        }

        public StayLotList_Ex_ViewModel UpdateLotInfoWlp2(StayLotList_Ex_ViewModel model, StayLotListDisPlayViewModel stayLotList)
        {
            if (!model.UpdateByCassetteId)
            {
                var lot = _StayLotListWLP2Repository.FindSingle(x => x.LotId == model.LotId && x.CassetteId == model.CassetteId && x.History_seq == model.history_seq);
                if (lot != null)
                {
                    lot.TenLoi = model.TenLoi.NullString();
                    lot.UserModified = GetUserId();
                    _StayLotListWLP2Repository.Update(lot);
                }
                else
                {
                    STAY_LOT_LIST_WLP2 en = new STAY_LOT_LIST_WLP2(model.LotId.NullString(), model.PhuongAnXuLy.NullString(), model.TenLoi.NullString(), model.NguoiXuLy.NullString(), model.CassetteId.NullString(), model.history_seq, model.PhanLoaiLoi);
                    en.UserModified = GetUserId();
                    en.UserCreated = GetUserId();
                    _StayLotListWLP2Repository.Add(en);
                }
            }
            else
            {
                // update bath theo casseteId
                string cassetteId = model.CassetteId;
                STAY_LOT_LIST_WLP2 lot;
                foreach (var item in stayLotList.StayLotList_Ex_ViewModels.FindAll(x => x.CassetteId == cassetteId))
                {
                    lot = _StayLotListWLP2Repository.FindSingle(x => x.LotId == item.LotId && x.CassetteId == item.CassetteId && x.History_seq == item.history_seq);
                    if (lot != null)
                    {
                        lot.TenLoi = model.TenLoi.NullString();
                        lot.UserModified = GetUserId();
                        _StayLotListWLP2Repository.Update(lot);
                    }
                    else
                    {
                        lot = new STAY_LOT_LIST_WLP2(item.LotId.NullString(), model.PhuongAnXuLy.NullString(), model.TenLoi.NullString(), model.NguoiXuLy.NullString(), item.CassetteId.NullString(), item.history_seq, model.PhanLoaiLoi);
                        lot.UserModified = GetUserId();
                        lot.UserCreated = GetUserId();
                        _StayLotListWLP2Repository.Add(lot);
                    }
                }
            }

            _unitOfWork.Commit();
            return model;
        }

        public List<STAY_LOT_LIST_HISTORY_WLP2> GetStayLotListHistoryWlp2(string cassetteId, string lotId, string timeFrom, string timeTo)
        {
            List<STAY_LOT_LIST_HISTORY_WLP2> rs = new List<STAY_LOT_LIST_HISTORY_WLP2>(); //((EFUnitOfWork)_unitOfWork).DBContext().STAY_LOT_LIST_HISTORY.ToList();

            if (!string.IsNullOrEmpty(cassetteId))
            {
                rs = ((EFUnitOfWork)_unitOfWork).DBContext().STAY_LOT_LIST_HISTORY_WLP2.Where(x => x.CassetteId == cassetteId).ToList();
            }

            if (!string.IsNullOrEmpty(lotId))
            {
                if (rs.Count > 0)
                    rs = rs.Where(x => x.LotId == lotId).ToList();
                else
                {
                    rs = ((EFUnitOfWork)_unitOfWork).DBContext().STAY_LOT_LIST_HISTORY_WLP2.Where(x => x.LotId == lotId).ToList();
                }
            }

            if (!string.IsNullOrEmpty(timeFrom))
            {
                timeFrom = DateTime.Parse(timeFrom).ToString("yyyyMMdd") + "000000";

                if (rs.Count > 0)
                    rs = rs.Where(x => x.HoldTime.CompareTo(timeFrom) >= 0).ToList();
                else
                {
                    rs = ((EFUnitOfWork)_unitOfWork).DBContext().STAY_LOT_LIST_HISTORY_WLP2.Where(x => x.HoldTime.CompareTo(timeFrom) >= 0).ToList();
                }
            }

            if (!string.IsNullOrEmpty(timeTo))
            {
                timeTo = DateTime.Parse(timeTo).ToString("yyyyMMdd") + "235959";

                if (rs.Count > 0)
                    rs = rs.Where(x => x.HoldTime.CompareTo(timeTo) <= 0).ToList();
                else
                {
                    rs = ((EFUnitOfWork)_unitOfWork).DBContext().STAY_LOT_LIST_HISTORY_WLP2.Where(x => x.HoldTime.CompareTo(timeFrom) >= 0 && x.HoldTime.CompareTo(timeTo) <= 0).ToList();
                }
            }

            foreach (var item in rs)
            {
                item.HoldTime = item.HoldTime.ConvertTime();
                item.ReleaseTime = item.ReleaseTime.ConvertTime();
            }
            return rs;
        }

        public List<Stay_lot_list_priory_wlp2ViewModel> GetStayLotListByModelWlp2(string model, string operation)
        {
            List<Stay_lot_list_priory_wlp2ViewModel> rs = new List<Stay_lot_list_priory_wlp2ViewModel>();

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("A_MODEL", model);
            dic.Add("A_OPERATION", operation);
            ResultDB resultDB = _StayLotListRepository.ExecProceduce2("GET_STAY_LOT_BY_MODEL_WLP2", dic);

            if (resultDB.ReturnInt == 0)
            {
                DataTable data = resultDB.ReturnDataSet.Tables[0];
                if (data.Rows.Count > 0)
                {
                    Stay_lot_list_priory_wlp2ViewModel stayVm; ;

                    foreach (DataRow item in data.Rows)
                    {
                        stayVm = new Stay_lot_list_priory_wlp2ViewModel()
                        {
                            CassetteID = item["Cassette ID"].NullString(),
                            Material = item["Material"].NullString(),
                            OperationId = item["Operation ID"].NullString(),
                            OperationName = item["Operation Name"].NullString(),
                            LotID = item["Lot ID"].NullString(),
                            ERPProductionOrder = item["ERP Product Order"].NullString(),
                            ChipQty = float.Parse(item["Chip Qty"].IfNullIsZero()),
                            StayDay = float.Parse(item["Stay Day"].IfNullIsZero()),
                            Number_Priory = int.Parse(item["Number_Priory"].IfNullIsZero()),
                            Priory = item["Priory"].NullString().ToLower() == "true" ? true : false,
                            SapCode = item["SAP_Code"].NullString()
                        };
                        rs.Add(stayVm);
                    }
                }
            }

            List<Stay_lot_list_priory_wlp2ViewModel> result = new List<Stay_lot_list_priory_wlp2ViewModel>();
            result.AddRange(rs.FindAll(x => x.Priory == true && x.Number_Priory > 0).OrderBy(x => x.Number_Priory).ToList());

            var lstNoPriory = rs.FindAll(x => !(x.Priory == true && x.Number_Priory > 0)).OrderByDescending(x => x.StayDay).ToList();
            if (lstNoPriory.Count > 0)
            {
                int priority = result.Count > 0 ? result.LastOrDefault().Number_Priory + 1 : 1;
                float stayDayOld = lstNoPriory[0].StayDay;
                foreach (var item in lstNoPriory)
                {
                    if (stayDayOld == item.StayDay)
                    {
                        item.Number_Priory = priority;
                    }
                    else if (stayDayOld > item.StayDay)
                    {
                        item.Number_Priory = ++priority;
                    }

                    stayDayOld = item.StayDay;
                }

                result.AddRange(lstNoPriory);
            }

            for (int i = 0; i < result.Count; i++)
            {
                result[i].STT = i + 1;
            }

            return result;
        }

        public Stay_lot_list_priory_wlp2ViewModel UpdatePrioryLotIdWlp2(Stay_lot_list_priory_wlp2ViewModel model, int index)
        {
            var en = _StayLotListPrioryWLP2Repository.FindAll(x => x.SapCode + "^" + x.OperationId + "^" + x.Material + "^" + x.CassetteID + "^" + x.LotID == model.Key).FirstOrDefault();
            if (en != null)
            {
                en.Priory = model.Priory;
                en.Number_Priory = model.Number_Priory;
                en.UserModified = GetUserId();
                _StayLotListPrioryWLP2Repository.Update(en);
            }
            else
            {
                model.UserCreated = GetUserId();
                en = _mapper.Map<STAY_LOT_LIST_PRIORY_WLP2>(model);
                _StayLotListPrioryWLP2Repository.Add(en);
            }

            if (index == 0)
            {
                SETTING_ITEMS st = _SettingItemRepository.FindSingle(x => x.Id == "RELOAD_AFTER_UPDATE_STAYLOT_DAILY");

                if (st != null && st.ItemValue != "1")
                {
                    st.ItemValue = "1";
                }

                _SettingItemRepository.Update(st);
            }

            return model;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public DAILY_PLAN_WLP2 GetDailyPlanWlp2Update(string sapcode, string datePlan)
        {
            return _DailyPlanRepository.FindAll(x => x.Model == sapcode && x.DatePlan == datePlan).FirstOrDefault();
        }

        public DAILY_PLAN_WLP2 UpdateDailyPlanWlp2Update(DAILY_PLAN_WLP2 dailyPlan, bool isUpdate)
        {
            if (isUpdate)
            {
                dailyPlan.UserModified = GetUserId();
                _DailyPlanRepository.Update(dailyPlan);
            }
            else
            {
                dailyPlan.UserCreated = GetUserId();
                _DailyPlanRepository.Add(dailyPlan);
            }
            Save();
            return dailyPlan;
        }
        #endregion

        #region SAMPLE
        public StayLotListDisPlayViewModel GetStayLotListSample()
        {
            ResultDB resultDB = _StayLotSampleListRepository.ExecProceduce2("PKG_BUSINESS@GET_STAY_LOT_LIST_SAMPLE", new Dictionary<string, string>());
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
                        CassetteId = row["CassetteId"].NullString(),
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
                        OperationName = row["OperationName"].NullString()
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
                        HoldTime = row["Hold Time"].NullString(),
                        LotCategory = row["Lot Category"].NullString(),
                        HoldUserName = row["Hold User Name"].NullString(),
                        HoldUser = row["Hold User ID"].NullString(),
                        HoldComment = row["Hold Comment"].NullString(),
                        TenLoi = row["TenLoi"].NullString(),
                        UpdateByCassetteId = false,
                        history_seq = double.Parse(row["HISTORY_SEQ"].IfNullIsZero())
                    });
                }
            }

            return result;
        }

        public StayLotList_Ex_ViewModel UpdateLotInfoSample(StayLotList_Ex_ViewModel model, StayLotListDisPlayViewModel stayLotList)
        {
            if (!model.UpdateByCassetteId)
            {
                var lot = _StayLotSampleListRepository.FindSingle(x => x.LotId == model.LotId && x.CassetteId == model.CassetteId && x.History_seq == model.history_seq);
                if (lot != null)
                {
                    lot.TenLoi = model.TenLoi.NullString();
                    lot.UserModified = GetUserId();
                    _StayLotSampleListRepository.Update(lot);
                }
                else
                {
                    STAY_LOT_LIST_SAMPLE en = new STAY_LOT_LIST_SAMPLE(model.LotId.NullString(), model.PhuongAnXuLy.NullString(), model.TenLoi.NullString(), model.NguoiXuLy.NullString(), model.CassetteId.NullString(), model.history_seq, model.PhanLoaiLoi);
                    en.UserModified = GetUserId();
                    en.UserCreated = GetUserId();
                    _StayLotSampleListRepository.Add(en);
                }
            }
            else
            {
                // update bath theo casseteId
                string cassetteId = model.CassetteId;
                STAY_LOT_LIST_SAMPLE lot;
                foreach (var item in stayLotList.StayLotList_Ex_ViewModels.FindAll(x => x.CassetteId == cassetteId))
                {
                    lot = _StayLotSampleListRepository.FindSingle(x => x.LotId == item.LotId && x.CassetteId == item.CassetteId && x.History_seq == item.history_seq);
                    if (lot != null)
                    {
                        lot.TenLoi = model.TenLoi.NullString();
                        lot.UserModified = GetUserId();
                        _StayLotSampleListRepository.Update(lot);
                    }
                    else
                    {
                        lot = new STAY_LOT_LIST_SAMPLE(item.LotId.NullString(), model.PhuongAnXuLy.NullString(), model.TenLoi.NullString(), model.NguoiXuLy.NullString(), item.CassetteId.NullString(), item.history_seq, model.PhanLoaiLoi);
                        lot.UserModified = GetUserId();
                        lot.UserCreated = GetUserId();
                        _StayLotSampleListRepository.Add(lot);
                    }
                }
            }

            _unitOfWork.Commit();
            return model;
        }

        public List<STAY_LOT_LIST_HISTORY_SAMPLE> GetStayLotListHistorySample(string cassetteId, string lotId, string timeFrom, string timeTo)
        {
            List<STAY_LOT_LIST_HISTORY_SAMPLE> rs = new List<STAY_LOT_LIST_HISTORY_SAMPLE>();

            if (!string.IsNullOrEmpty(cassetteId))
            {
                rs = ((EFUnitOfWork)_unitOfWork).DBContext().STAY_LOT_LIST_HISTORY_SAMPLE.Where(x => x.CassetteId == cassetteId).ToList();
            }

            if (!string.IsNullOrEmpty(lotId))
            {
                if (rs.Count > 0)
                    rs = rs.Where(x => x.LotId == lotId).ToList();
                else
                {
                    rs = ((EFUnitOfWork)_unitOfWork).DBContext().STAY_LOT_LIST_HISTORY_SAMPLE.Where(x => x.LotId == lotId).ToList();
                }
            }

            if (!string.IsNullOrEmpty(timeFrom))
            {
                timeFrom = DateTime.Parse(timeFrom).ToString("yyyyMMdd") + "000000";

                if (rs.Count > 0)
                    rs = rs.Where(x => x.HoldTime.CompareTo(timeFrom) >= 0).ToList();
                else
                {
                    rs = ((EFUnitOfWork)_unitOfWork).DBContext().STAY_LOT_LIST_HISTORY_SAMPLE.Where(x => x.HoldTime.CompareTo(timeFrom) >= 0).ToList();
                }
            }

            if (!string.IsNullOrEmpty(timeTo))
            {
                timeTo = DateTime.Parse(timeTo).ToString("yyyyMMdd") + "235959";

                if (rs.Count > 0)
                    rs = rs.Where(x => x.HoldTime.CompareTo(timeTo) <= 0).ToList();
                else
                {
                    rs = ((EFUnitOfWork)_unitOfWork).DBContext().STAY_LOT_LIST_HISTORY_SAMPLE.Where(x => x.HoldTime.CompareTo(timeFrom) >= 0 && x.HoldTime.CompareTo(timeTo) <= 0).ToList();
                }
            }

            foreach (var item in rs)
            {
                item.HoldTime = item.HoldTime.ConvertTime();
                item.ReleaseTime = item.ReleaseTime.ConvertTime();
            }
            return rs;
        }

        public List<Stay_lot_list_priory_wlp2ViewModel> GetStayLotListByModelSample(string model, string operation)
        {
            throw new NotImplementedException();
        }

        public Stay_lot_list_priory_wlp2ViewModel UpdatePrioryLotIdSample(Stay_lot_list_priory_wlp2ViewModel model, int index)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region LFEM
        public List<Stay_lot_list_priory_lfem_ViewModel> GetStayLotListByModelLFEM(string material, string operation)
        {
            List<Stay_lot_list_priory_lfem_ViewModel> rs = new List<Stay_lot_list_priory_lfem_ViewModel>();

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("A_MATERIAL", material);
            dic.Add("A_OPERATION", operation);
            ResultDB resultDB = _StayLotListRepository.ExecProceduce2("GET_STAY_LOT_BY_MODEL_LFEM", dic);
            ResultDB resultStay;
            if (resultDB.ReturnInt == 0)
            {
                DataTable data = resultDB.ReturnDataSet.Tables[0];
                DataTable stayLot;

                if (data.Rows.Count > 0)
                {
                    Stay_lot_list_priory_lfem_ViewModel stayVm; ;

                    foreach (DataRow item in data.Rows)
                    {
                        stayVm = new Stay_lot_list_priory_lfem_ViewModel()
                        {
                            StayDay = float.Parse(item["STAY_DAY"].IfNullIsZero()),
                            Size = item["Size"].NullString(),
                            MesItem = item["Material"].NullString(),
                            LotID = item["Lot ID"].NullString(),
                            ProductOrder = item["Product Order"].NullString(),
                            FAID = item["FA ID"].NullString(),
                            AssyLotID = item["Assy Lot ID"].NullString(),
                            Date = item["Date"].NullString(),
                            OperationName = item["Operation Name"].NullString(),
                            OperationId = item["Operation"].NullString(),
                            DateDiff = int.Parse(item["DATE_DIFF"].IfNullIsZero()),
                            ChipQty = float.Parse(item["Chip Qty"].IfNullIsZero()),
                            Unit = item["Unit"].NullString(),
                            StartFlag = item["Start Flag"].NullString(),
                            EquipmentName = item["Equipment Name"].NullString(),
                            Worker = item["Worker"].NullString(),

                            Number_Priory = int.Parse(item["Number_Priory"].IfNullIsZero()),
                            Priory = item["Priory"].NullString().ToLower() == "true" ? true : false,
                        };

                        dic = new Dictionary<string, string>();
                        dic.Add("A_LOT_ID", stayVm.LotID);
                        resultStay = _StayLotListRepository.ExecProceduce2("GET_STAY_DAY_BY_LOT_LFEM_DATA", dic);

                        if (resultStay.ReturnInt == 0)
                        {
                             stayLot = resultStay.ReturnDataSet.Tables[0];
                            if(stayLot.Rows.Count > 0)
                            stayVm.StayDay = float.Parse(stayLot.Rows[0][0].IfNullIsZero());
                        }

                        rs.Add(stayVm);
                    }
                }
            }

            List<Stay_lot_list_priory_lfem_ViewModel> result = new List<Stay_lot_list_priory_lfem_ViewModel>();

            // Add ưu tiên 
            result.AddRange(rs.FindAll(x => x.Priory == true && x.Number_Priory > 0).OrderBy(x => x.Number_Priory).ToList());

            // không ưu tiên
            var lstNoPriory = rs.FindAll(x => !(x.Priory == true && x.Number_Priory > 0)).OrderByDescending(x => x.StayDay).ToList();

            if (lstNoPriory.Count > 0)
            {
                int priority = result.Count > 0 ? result.LastOrDefault().Number_Priory + 1 : 1;
                float stayDayOld = lstNoPriory[0].StayDay;
                foreach (var item in lstNoPriory)
                {
                    if (stayDayOld == item.StayDay)
                    {
                        item.Number_Priory = priority;
                    }
                    else if (stayDayOld > item.StayDay)
                    {
                        item.Number_Priory = ++priority;
                    }

                    stayDayOld = item.StayDay;
                }

                result.AddRange(lstNoPriory);
            }

            for (int i = 0; i < result.Count; i++)
            {
                result[i].STT = i + 1;
            }

            return result;
        }

        public Stay_lot_list_priory_lfem_ViewModel UpdatePrioryLotIdLFEM(Stay_lot_list_priory_lfem_ViewModel model, int index)
        {
            var en = _StayLotListPrioryLFEMRepository.FindAll(x => x.MesItem + "^" + x.OperationId + "^" + x.LotID == model.Key).FirstOrDefault();
            if (en != null)
            {
                en.Priory = model.Priory;
                en.Number_Priory = model.Number_Priory;
                en.UserModified = GetUserId();
                _StayLotListPrioryLFEMRepository.Update(en);
            }
            else
            {
                model.UserCreated = GetUserId();
                en = _mapper.Map<STAY_LOT_LIST_PRIORY_LFEM>(model);
                _StayLotListPrioryLFEMRepository.Add(en);
            }

            if (index == 0)
            {
                SETTING_ITEMS st = _SettingItemRepository.FindSingle(x => x.Id == "RELOAD_AFTER_UPDATE_STAYLOT_DAILY_LFEM");

                if (st != null && st.ItemValue != "1")
                {
                    st.ItemValue = "1";
                }

                _SettingItemRepository.Update(st);
            }

            return model;
        }

        public StayLotListDisPlayViewModel GetStayLotListLFEM()
        {
            ResultDB resultDB = _StayLotListLFEMRepository.ExecProceduce2("PKG_BUSINESS@GET_STAY_LOT_LIST_LFEM", new Dictionary<string, string>());
            StayLotListDisPlayViewModel result = new StayLotListDisPlayViewModel();
            if (resultDB.ReturnInt == 0)
            {
                int i = 0;
                foreach (DataRow row in resultDB.ReturnDataSet.Tables[0].Rows)
                {
                    result.StayLotListSumViews.Add(new StayLotListSumViewModel()
                    {
                        Index = ++i,
                        MaterialCategory = row["Material Category"].NullString(),
                        MaterialGroup = row["Material Group"].NullString(),
                        Material = row["Material"].NullString(),
                        Size = row["Size"].NullString(),
                        QtyChip = decimal.Parse(row["Chip Qty"].IfNullIsZero())
                    });
                }

                i = 0;
                foreach (DataRow row in resultDB.ReturnDataSet.Tables[1].Rows)
                {
                    result.StayLotListTenLoiViews.Add(new StayLotListSumViewModel()
                    {
                        Index = ++i,
                        QtyChip = decimal.Parse(row["CHIP_QTY"].IfNullIsZero()),
                        PhanLoaiLoi = row["LoaiLoi"].NullString()
                    });
                }

                foreach (DataRow row in resultDB.ReturnDataSet.Tables[2].Rows)
                {
                    result.StayLotList_Ex_ViewModels.Add(new StayLotList_Ex_ViewModel()
                    {
                        MaterialCategory = row["Material Category"].NullString(),
                        MaterialGroup = row["Material Group"].NullString(),
                        Size = row["Size"].NullString(),
                        Material = row["Material"].NullString(),
                        LotId = row["Lot ID"].NullString(),
                        FA_ID = row["FA ID"].NullString(),
                        AssyLotID = row["Assy Lot ID"].NullString(),
                        Date = row["Date"].NullString(),
                        DATE_DIFF = double.Parse(row["DATE_DIFF"].IfNullIsZero()),
                        OperationID = row["Operation"].NullString(),
                        OperationName = row["Operation Name"].NullString(),
                        ChipQty = decimal.Parse(row["Chip Qty"].IfNullIsZero()),
                        Worker = row["Worker"].NullString(),
                        Comment = row["Comment"].NullString(),
                        PhanLoaiLoi = row["LoaiLoi"].NullString(),
                        LichTrinhXuLy = row["LichTrinhXuLy"].NullString(),
                        ChiuTrachNhiem = row["ChiuTrachNhiem"].NullString(),
                        UpdateByCassetteId = false,
                        history_seq = double.Parse(row["HISTORY_SEQ"].IfNullIsZero()),
                    });
                }
            }

            return result;
        }

        public StayLotList_Ex_ViewModel UpdateLotInfoLFEM(StayLotList_Ex_ViewModel model, StayLotListDisPlayViewModel stayLotList)
        {
            if (!model.UpdateByCassetteId)
            {
                var lot = _StayLotListLFEMRepository.FindSingle(x => x.LotId == model.LotId && x.Material == model.Material && x.History_seq == model.history_seq);
                if (lot != null)
                {
                    lot.UserModified = GetUserId();
                    lot.LoaiLoi = model.PhanLoaiLoi.NullString();
                    _StayLotListLFEMRepository.Update(lot);
                }
                else
                {
                    STAY_LOT_LIST_LFEM en = new STAY_LOT_LIST_LFEM();
                    en.Material = model.Material;
                    en.LotId = model.LotId;
                    en.History_seq = model.history_seq;
                    en.LoaiLoi = model.PhanLoaiLoi;
                    en.LichTrinhXuLy = model.LichTrinhXuLy;
                    en.ChiuTrachNhiem = model.ChiuTrachNhiem;
                    en.UserModified = GetUserId();
                    en.UserCreated = GetUserId();
                    _StayLotListLFEMRepository.Add(en);
                }
            }
            else
            {
                // update bath theo Material
                string _material = model.Material;
                STAY_LOT_LIST_LFEM lot;
                foreach (var item in stayLotList.StayLotList_Ex_ViewModels.FindAll(x => x.Material == _material))
                {
                    lot = _StayLotListLFEMRepository.FindSingle(x => x.LotId == item.LotId && x.Material == item.Material && x.History_seq == item.history_seq);
                    if (lot != null)
                    {
                        lot.LoaiLoi = model.PhanLoaiLoi;
                        lot.LichTrinhXuLy = model.LichTrinhXuLy;
                        lot.ChiuTrachNhiem = model.ChiuTrachNhiem;
                        lot.UserModified = GetUserId();
                        _StayLotListLFEMRepository.Update(lot);
                    }
                    else
                    {
                        lot = new STAY_LOT_LIST_LFEM();
                        lot.MaterialCategory = item.MaterialCategory;
                        lot.MaterialGroup = item.MaterialGroup;
                        lot.Size = item.Size;
                        lot.FAID = item.FA_ID;
                        lot.Material = item.Material;
                        lot.LotId = item.LotId;
                        lot.History_seq = item.history_seq;
                        lot.LoaiLoi = model.PhanLoaiLoi;
                        lot.LichTrinhXuLy = model.LichTrinhXuLy;
                        lot.ChiuTrachNhiem = model.ChiuTrachNhiem;
                        lot.UserModified = GetUserId();
                        lot.UserCreated = GetUserId();
                        _StayLotListLFEMRepository.Add(lot);
                    }
                }
            }

            _unitOfWork.Commit();
            return model;
        }

        public List<STAY_LOT_LIST_HISTORY_LFEM> GetStayLotListHistoryLFEM(string materialId, string lotId, string timeFrom, string timeTo)
        {
            List<STAY_LOT_LIST_HISTORY_LFEM> rs = new List<STAY_LOT_LIST_HISTORY_LFEM>();

            if (!string.IsNullOrEmpty(materialId))
            {
                rs = ((EFUnitOfWork)_unitOfWork).DBContext().STAY_LOT_LIST_HISTORY_LFEM.Where(x => x.Material == materialId).ToList();
            }

            if (!string.IsNullOrEmpty(lotId))
            {
                if (rs.Count > 0)
                    rs = rs.Where(x => x.LotId == lotId).ToList();
                else
                {
                    rs = ((EFUnitOfWork)_unitOfWork).DBContext().STAY_LOT_LIST_HISTORY_LFEM.Where(x => x.LotId == lotId).ToList();
                }
            }

            if (!string.IsNullOrEmpty(timeFrom))
            {
                timeFrom = DateTime.Parse(timeFrom).ToString("yyyyMMdd") + "000000";

                if (rs.Count > 0)
                    rs = rs.Where(x => x.HoldTime.CompareTo(timeFrom) >= 0).ToList();
                else
                {
                    rs = ((EFUnitOfWork)_unitOfWork).DBContext().STAY_LOT_LIST_HISTORY_LFEM.Where(x => x.HoldTime.CompareTo(timeFrom) >= 0).ToList();
                }
            }

            if (!string.IsNullOrEmpty(timeTo))
            {
                timeTo = DateTime.Parse(timeTo).ToString("yyyyMMdd") + "235959";

                if (rs.Count > 0)
                    rs = rs.Where(x => x.HoldTime.CompareTo(timeTo) <= 0).ToList();
                else
                {
                    rs = ((EFUnitOfWork)_unitOfWork).DBContext().STAY_LOT_LIST_HISTORY_LFEM.Where(x => x.HoldTime.CompareTo(timeFrom) >= 0 && x.HoldTime.CompareTo(timeTo) <= 0).ToList();
                }
            }

            foreach (var item in rs)
            {
                item.HoldTime = item.HoldTime.ConvertTime();
                item.ReleaseTime = item.ReleaseTime.ConvertTime();
            }

            return rs;
        }
        #endregion
    }
}
