using Microsoft.EntityFrameworkCore.Internal;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels.LotTracking;
using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Infrastructure.Interfaces;
using OPERATION_MNS.Utilities.Dtos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace OPERATION_MNS.Application.Implementation
{
    public class LotTrackingService : BaseService, ILotTrackingService
    {
        private IRespository<SETTING_ITEMS, string> _SettingItemRepository;

        public LotTrackingService(IRespository<SETTING_ITEMS, string> settingItemRepository)
        {
            _SettingItemRepository = settingItemRepository;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<WaferInfo> GetAffectlotInfo(string cassetId)
        {
            List<WaferInfo> WaferInfos = new List<WaferInfo>();
            List<WaferInfo> result = new List<WaferInfo>();
            List<WaferInfo> result2 = new List<WaferInfo>();
            List<string> lstPackingUnitID = new List<string>();

            // Get casset History : VIEW WIP CASSET HISTORY
            var dic = new Dictionary<string, string>();
            dic.Add("A_CASSET_IDS", cassetId);
            var resultDB = _SettingItemRepository.ExecProceduce2("LOT_TRACKING_GET_CASSETID_HISTORYINFO", dic);

            if (resultDB.ReturnInt == 0)
            {
                WaferInfo waferInfo;
                ResultDB _resultDB;
                List<string> lstLot = new List<string>();

                foreach (DataRow item in resultDB.ReturnDataSet.Tables[0].Rows)
                {
                    waferInfo = new WaferInfo()
                    {
                        Model = item["Material"].NullString(),
                        CassetId = cassetId, // WLP1 Wafer Number
                        LotID = item["Lot ID"].NullString(),// WLP1 Lot Number
                        LotFAB = item["FAB Lot ID"].NullString(),
                        WaferID = item["Wafer ID"].NullString(),
                        Operation = item["OPERATION_SHORT_NAME"].NullString()
                    };

                    if (!lstLot.Contains(waferInfo.LotID))
                    {
                        lstLot.Add(waferInfo.LotID);
                    }

                    WaferInfos.Add(waferInfo);
                }

                if (lstLot.Count > 0)
                {

                    // Get casset Id : VIEW WIP LOT HISTORY : Lot history
                    dic = new Dictionary<string, string> { { "A_LOT_IDS", lstLot.Join(",") } };
                    _resultDB = _SettingItemRepository.ExecProceduce2("LOT_TRACKING_GET_LOT_HISTORY_INFO", dic);

                    if (_resultDB.ReturnInt == 0)
                    {
                        foreach (var item in WaferInfos)
                        {
                            foreach (DataRow his in _resultDB.ReturnDataSet.Tables[0].Rows)
                            {
                                if (item.LotID == his["LOT_ID"].NullString() && his["Relation Lot"].NullString() != "")
                                {
                                    item.LotHistories.Add(new LotHistory()
                                    {
                                        CassetId = item.CassetId,
                                        LotID = his["LOT_ID"].NullString(),
                                        TranTime = his["Tran Time"].NullString(),
                                        Operation = his["Operation ID"].NullString(),
                                        EquiptmentName = his["Equipment"].NullString(),
                                        OnlineOffLine = his["User Name"].NullString(),
                                        Wlp2_Reel_Number = his["Relation Lot"].NullString()
                                    });

                                    if (!lstPackingUnitID.Contains(his["Relation Lot"].NullString()))
                                    {
                                        lstPackingUnitID.Add(his["Relation Lot"].NullString());
                                    }
                                }
                            }
                        }
                    }
                }
            }

            WaferInfo wf;
            foreach (var item in WaferInfos)
            {
                if (item.LotHistories.Count() > 0)
                {
                    foreach (var his in item.LotHistories)
                    {
                        wf = new WaferInfo()
                        {
                            CassetId = item.CassetId,
                            Model = item.Model,
                            LotID = item.LotID,
                            WaferID = item.WaferID,
                            LotFAB = item.LotFAB,
                            Wlp2_Reel_Number = his.Wlp2_Reel_Number,
                            Operation = item.Operation
                        };

                        result.Add(wf);
                    }
                }
                else
                {
                    result.Add(item);
                }
            }

            if (lstPackingUnitID.Count() > 0)
            {
                // get lot module
                ResultDB m_ResultDB;
                var configuation = GetConfiguration();
                string conn = configuation.GetSection("ConnectionStrings").GetSection("WHNP1Connection").Value;
                foreach (var item in lstPackingUnitID)
                {
                    dic = new Dictionary<string, string> { { "A_PACKING_UNIT", item } };
                    m_ResultDB = _SettingItemRepository.ExecProceduce3("PKG_SMT018.GET_LIST", dic, conn);

                    if (m_ResultDB.ReturnInt == 0 && m_ResultDB.ReturnDataSet.Tables[1].Rows.Count > 0)
                    {
                        if (result.Where(x => x.Wlp2_Reel_Number == item).Count() > 0)
                        {
                            foreach (var rs in result.Where(x => x.Wlp2_Reel_Number == item))
                            {
                                foreach (DataRow row in m_ResultDB.ReturnDataSet.Tables[1].Rows)
                                {
                                    wf = new WaferInfo()
                                    {
                                        CassetId = rs.CassetId,
                                        Model = rs.Model,
                                        LotID = rs.LotID,
                                        WaferID = rs.WaferID,
                                        LotFAB = rs.LotFAB,
                                        Wlp2_Reel_Number = rs.Wlp2_Reel_Number,
                                        LotModule = row["LOT_NO"].NullString(),
                                        Operation = rs.Operation
                                    };

                                    result2.Add(wf);
                                }
                            }
                        }
                        else
                        {
                            result2.AddRange(result.FindAll(x => x.Wlp2_Reel_Number == item));
                        }
                    }
                    else
                    {
                        result2.AddRange(result.FindAll(x => x.Wlp2_Reel_Number == item));
                    }
                }
            }
            else
            {
                result2.AddRange(result);
            }

            List<string> LotModules = new List<string>();
            foreach (var item in result2)
            {
                if(item.LotModule.NullString() != "" && !LotModules.Contains(item.LotModule.NullString()))
                {
                    LotModules.Add(item.LotModule);
                }
            }

            dic = new Dictionary<string, string> { { "A_LOT_MODULE", LotModules.Join(",") } };
            ResultDB _resultDB1 = _SettingItemRepository.ExecProceduce2("GET_CURRENT_OPERATION_LOT_TRACKING", dic);
            if(_resultDB1.ReturnInt == 0)
            {
                DataTable data = _resultDB1.ReturnDataSet.Tables[0];

                foreach (var item in result2)
                {
                    foreach (DataRow row in data.Rows)
                    {
                        if (row["LOT_ID"].NullString() == item.LotModule)
                        {
                            item.Operation = row["OperationName"].NullString();
                        }
                    }
                }
            }
            return result2;
        }

        /// <summary>
        /// Lot module 
        /// </summary>
        /// <param name="lotNo"></param>
        /// <returns></returns>
        public List<LotTrackingViewModel> GetPackingInfo(string lotNo)
        {
            List<LotTrackingViewModel> lots = new List<LotTrackingViewModel>();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            List<string> lstLABC = new List<string>() { "LA", "LB", "LC" };
            //if (lotNo.StartsWith("HNM"))
            // {
            dic.Add("A_LOT_NO", lotNo);
            dic.Add("A_MATERIAL", "");
            var configuation = GetConfiguration();
            string conn = configuation.GetSection("ConnectionStrings").GetSection("WHNP1Connection").Value;

            ResultDB resultDB1 = _SettingItemRepository.ExecProceduce3("PKG_SMT017.GET_LIST2", dic, conn);

            if (resultDB1.ReturnInt == 0)
            {
                // get packing unit id
                LotTrackingViewModel lot;
                foreach (DataRow item in resultDB1.ReturnDataSet.Tables[1].Rows)
                {
                    lot = new LotTrackingViewModel();
                    lot.LotModule = lotNo;
                    lot.Wlp2_Reel_Number = item["PACKING_UNIT_ID"].NullString();
                    if (!lots.Any(x => x.Wlp2_Reel_Number == lot.Wlp2_Reel_Number) && lstLABC.Contains(lot.Wlp2_Reel_Number.Substring(0, 2)))
                    {
                        lots.Add(lot);
                    }
                }
            }
            //}

            // Get casset Id : VIEW WIP LOT HISTORY
            dic = new Dictionary<string, string>();

            if (lots.Count > 0)
            {
                dic.Add("A_LOT_IDS", lots.Select(x => x.Wlp2_Reel_Number).Join(","));
            }
            else
            {
                dic.Add("A_LOT_IDS", lotNo);

                lots = new List<LotTrackingViewModel>()
                {
                    new LotTrackingViewModel()
                    {
                        Wlp2_Reel_Number = lotNo,
                    }
                };
            }

            ResultDB resultDB = _SettingItemRepository.ExecProceduce2("LOT_TRACKING_GET_LOT_STATUS_INFO", dic);

            if (resultDB.ReturnInt == 0)
            {
                foreach (DataRow item in resultDB.ReturnDataSet.Tables[0].Rows)
                {
                    foreach (var lot in lots)
                    {
                        if (lot.Wlp2_Reel_Number == item["LOT_ID"].NullString())
                        {
                            lot.CassetId = item["CASSETTE_ID"].NullString();
                            lot.Model = item["MATERIAL_ID"].NullString();

                            foreach (DataRow his in resultDB.ReturnDataSet.Tables[1].Rows)
                            {
                                if (lot.Wlp2_Reel_Number == his["LOT_ID"].NullString() && !lot.RelationLots.Contains(his["Relation Lot"].NullString()) && his["Relation Lot"].NullString() != "")
                                {
                                    lot.RelationLots.Add(his["Relation Lot"].NullString());
                                }
                            }
                        }
                    }
                }
            }

            // Get casset History : VIEW WIP CASSET HISTORY
            dic = new Dictionary<string, string>();
            dic.Add("A_CASSET_IDS", lots.Select(x => x.CassetId).Join(","));
            resultDB = _SettingItemRepository.ExecProceduce2("LOT_TRACKING_GET_CASSETID_HISTORYINFO", dic);

            if (resultDB.ReturnInt == 0)
            {
                WaferInfo waferInfo;
                ResultDB _resultDB;
                foreach (var lot in lots)
                {
                    foreach (DataRow item in resultDB.ReturnDataSet.Tables[0].Rows)
                    {
                        if (lot.CassetId == item["CASSETTE_ID"].NullString() && lot.RelationLots.Contains(item["Lot ID"].NullString()))
                        {
                            waferInfo = lot.WaferInfos.FirstOrDefault(x => x.LotID == item["Lot ID"].NullString());
                            if (waferInfo == null)
                            {
                                waferInfo = new WaferInfo()
                                {
                                    LotModule = lot.LotModule,
                                    Model = lot.Model,
                                    Wlp2_Reel_Number = lot.Wlp2_Reel_Number,
                                    CassetId = lot.CassetId, // WLP1 Wafer Number
                                    LotID = item["Lot ID"].NullString(),// WLP1 Lot Number
                                    LotFAB = item["FAB Lot ID"].NullString(),
                                    WaferID = item["Wafer ID"].NullString(),
                                    LotIDView = new LotIDView()
                                    {
                                        LotID = item["Lot ID"].NullString(),
                                        IsRelationLot = lot.RelationLots.Contains(item["Lot ID"].NullString()) == true ? "1" : "0",
                                    }
                                };
                                lot.WaferInfos.Add(waferInfo);
                            }
                        }
                    }
                }

                List<string> lstLot = new List<string>();

                // Get casset Id : VIEW WIP LOT HISTORY : Lot history
                foreach (var cass in lots)
                {
                    foreach (var item in cass.WaferInfos)
                    {
                        if (!lstLot.Contains(item.LotID))
                        {
                            lstLot.Add(item.LotID);
                        }
                    }
                }

                dic = new Dictionary<string, string> { { "A_LOT_IDS", lstLot.Join(",") } };
                _resultDB = _SettingItemRepository.ExecProceduce2("LOT_TRACKING_GET_LOT_HISTORY_INFO", dic);

                if (_resultDB.ReturnInt == 0)
                {
                    foreach (var cass in lots)
                    {
                        foreach (var item in cass.WaferInfos)
                        {
                            foreach (DataRow his in _resultDB.ReturnDataSet.Tables[0].Rows)
                            {
                                if (item.LotID == his["LOT_ID"].NullString())
                                {
                                    item.LotHistories.Add(new LotHistory()
                                    {
                                        CassetId = item.CassetId,
                                        LotID = his["LOT_ID"].NullString(),
                                        TranTime = his["Tran Time"].NullString(),
                                        Operation = his["Operation ID"].NullString(),
                                        EquiptmentName = his["Equipment"].NullString(),
                                        OnlineOffLine = his["User Name"].NullString()
                                    });
                                }
                            }
                        }
                    }
                }
            }



            return lots;
        }

        public List<WaferInfo> GetTrackingVIEWs(string lotNo)
        {
            List<WaferInfo> data = new List<WaferInfo>();
            List<WaferInfo> result = new List<WaferInfo>();
            List<LotTrackingViewModel> lstLotTracking = GetPackingInfo(lotNo);

            foreach (var item in lstLotTracking)
            {
                data.AddRange(item.WaferInfos);
            }

            WaferInfo wf = null;
            foreach (var item in data)
            {
                foreach (var lot in item.LotHistories)
                {
                    wf = new WaferInfo()
                    {
                        LotModule = item.LotModule,
                        Wlp2_Reel_Number = item.Wlp2_Reel_Number,
                        CassetId = item.CassetId,
                        Model = item.Model,
                        LotID = item.LotID,
                        WaferID = item.WaferID,
                        LotFAB = item.LotFAB,
                        LotIDView = item.LotIDView,
                        Operation = lot.Operation,
                        EquiptmentName = lot.EquiptmentName,
                        OnlineOffLine = lot.OnlineOffLine,
                        TranTime = lot.TranTime
                    };

                    result.Add(wf);
                }
            }

            return result;
        }
    }
}
