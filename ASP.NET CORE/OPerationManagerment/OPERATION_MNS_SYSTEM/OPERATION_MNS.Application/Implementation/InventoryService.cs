using AutoMapper;
using Microsoft.AspNetCore.Http;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Application.ViewModels.Lfem;
using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Infrastructure.Interfaces;
using OPERATION_MNS.Utilities.Common;
using OPERATION_MNS.Utilities.Constants;
using OPERATION_MNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPERATION_MNS.Application.Implementation
{
    public class InventoryService : BaseService, IInventoryService
    {
        private IRespository<INVENTORY_ACTUAL, int> _InventoryActualRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public InventoryService(IRespository<INVENTORY_ACTUAL, int> InventoryActualRepository,
                              IUnitOfWork unitOfWork, IMapper mapper,
                              IHttpContextAccessor httpContextAccessor)
        {
            _InventoryActualRepository = InventoryActualRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Hàng normal, loại A/B, đơn vị chip
        /// Get current inventory
        /// </summary>
        /// <param name="unit">CHIP WAFER</param>
        /// <returns></returns>
        public List<InventoryActualViewModel> GetCurrentInventory(string unit)
        {
            List<InventoryActualViewModel> result = new List<InventoryActualViewModel>();
            if (unit == CommonConstants.CHIP)
            {
                ResultDB resultDB = _InventoryActualRepository.ExecProceduce2("PKG_BUSINESS@GET_INVENTORY_CURRENT_CHIP", new Dictionary<string, string>());

                if (resultDB.ReturnInt == 0)
                {
                    result = DataTableToJson.ConvertDataTable<InventoryActualViewModel>(resultDB.ReturnDataSet.Tables[0]);
                }
            }
            else
            {
                ResultDB resultDB = _InventoryActualRepository.ExecProceduce2("PKG_BUSINESS@GET_INVENTORY_CURRENT_WAFER", new Dictionary<string, string>());

                if (resultDB.ReturnInt == 0)
                {
                    result = DataTableToJson.ConvertDataTable<InventoryActualViewModel>(resultDB.ReturnDataSet.Tables[0]);
                }
            }

            return result;
        }

        /// <summary>
        /// Get data for Lot monitoring system
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public LotMonitoringLfemViewModel GetMonitoringLfemData(string operation, string model, decimal stayday, string sheetNumber)
        {
            LotMonitoringLfemViewModel result = new LotMonitoringLfemViewModel();
            List<ViewWIpLfemData> viewWIpLfemDatas = new List<ViewWIpLfemData>();

            ResultDB resultDB = _InventoryActualRepository.ExecProceduce2("GET_VIEW_WIP_LOT_LIST_MONITORING_LFEM", new Dictionary<string, string>());

            if (resultDB.ReturnInt == 0)
            {
                viewWIpLfemDatas = DataTableToJson.ConvertDataTable<ViewWIpLfemData>(resultDB.ReturnDataSet.Tables[0]);

                result.TotalProduct.Models = viewWIpLfemDatas.Select(x => x.Material.Substring(3, 4)).Distinct().ToList();
                result.TotalProduct.StayDays = viewWIpLfemDatas.Select(x => x.StayDay).Distinct().OrderBy(x => x).ToList();

                result.TotalProduct.Operation = viewWIpLfemDatas.Select(x => new OperationItem() { Id = x.Operation, Name = x.OperationName }).ToList();
                foreach (var item in result.TotalProduct.Operation.ToList())
                {
                    if (result.TotalProduct.Operation.Where(x => x.Id == item.Id).Count() > 1)
                    {
                        result.TotalProduct.Operation.Remove(item);
                    }
                }

                List<string> OP_Finish = new List<string>() { "OC430", "OC440", "OC445", "OC450", "OC455", "OC458", "OC459", "OC460", "OS100", "OR001" };
                result.FinishProduct.Models = viewWIpLfemDatas.Where(x => OP_Finish.Contains(x.Operation)).Select(x => x.Material.Substring(3, 4)).Distinct().ToList();
                result.FinishProduct.StayDays = viewWIpLfemDatas.Where(x => OP_Finish.Contains(x.Operation)).Select(x => x.StayDay).Distinct().OrderBy(x => x).ToList();
                result.FinishProduct.Operation = viewWIpLfemDatas.Where(x => OP_Finish.Contains(x.Operation)).Select(x => new OperationItem() { Id = x.Operation, Name = x.OperationName }).ToList();
                foreach (var item in result.FinishProduct.Operation.ToList())
                {
                    if (result.FinishProduct.Operation.Where(x => x.Id == item.Id).Count() > 1)
                    {
                        result.FinishProduct.Operation.Remove(item);
                    }
                }

                if (operation.NullString() != "")
                {
                    viewWIpLfemDatas = viewWIpLfemDatas.Where(x => x.Operation == operation.NullString()).ToList();
                }

                if (model.NullString() != "")
                {
                    viewWIpLfemDatas = viewWIpLfemDatas.Where(x => x.Material.Substring(3, 4) == model.NullString()).ToList();
                }

                if (stayday >= 0)
                {
                    viewWIpLfemDatas = viewWIpLfemDatas.Where(x => x.StayDay == stayday).ToList();
                }

                if (viewWIpLfemDatas.Count > 0)
                {
                    // 전체 재공현황
                    if (sheetNumber == "1")
                    {
                        var lstTotal = viewWIpLfemDatas.GroupBy(x => x.OperationName).Select(x => new StayDayLfemItem
                        {
                            StayDay = x.Average(x => x.StayDay),
                            LotIDCount = x.Count(c => c.LotID != ""),
                            OperationName = x.Key

                        });

                        var lstTotal1 = viewWIpLfemDatas.GroupBy(x => x.Material.Substring(3, 4)).Select(x => new StayDayLfemItem
                        {
                            StayDay = x.Average(x => x.StayDay),
                            LotIDCount = x.Count(c => c.LotID != ""),
                            Model = x.Key

                        });

                        var lstTotal2 = viewWIpLfemDatas.GroupBy(x => x.Size.NullString()).Select(x => new StayDayLfemItem
                        {
                            StayDay = x.Average(x => x.StayDay),
                            LotIDCount = x.Count(c => c.LotID != ""),
                            Size = x.Key

                        });

                        result.TotalProduct.StayDayLfemItems = lstTotal.OrderByDescending(x => x.LotIDCount).ToList();
                        result.TotalProduct.StayDayLfemItems1 = lstTotal1.OrderByDescending(x => x.LotIDCount).ToList();
                        result.TotalProduct.StayDayLfemItems2 = lstTotal2.OrderByDescending(x => x.LotIDCount).ToList();

                        result.TotalProduct.LotCount = viewWIpLfemDatas.Select(x => x.LotID).Count();
                        result.TotalProduct.ChipQTYSum = Math.Round(viewWIpLfemDatas.Select(x => x.ChipQty).Sum(),0);
                        result.TotalProduct.StayDayAvg = Math.Round(viewWIpLfemDatas.Select(x => x.StayDay).Average(),1);

                        result.TotalProduct.ChipQTYSumStr = result.TotalProduct.ChipQTYSum >= 1000000 ? Math.Round(result.TotalProduct.ChipQTYSum / 1000000, 1) + "M" : Math.Round(result.TotalProduct.ChipQTYSum / 1000, 1) + "K";
                        result.TotalProduct.OperationV = operation.NullString();
                        result.TotalProduct.ModelV = model.NullString();
                        result.TotalProduct.StayDayV = stayday;
                    }

                    if (sheetNumber == "2")
                    {
                        // 공정별 정체재공
                        List<string> OP_SMT = new List<string>() { "OC200", "OC2101", "OC211", "OC212" };
                        var smtData = viewWIpLfemDatas.Where(x => OP_SMT.Contains(x.Operation))
                                                      .GroupBy(x => new { x.LotID, x.Size, Model = x.Material.Substring(3, 4), x.OperationName, x.Status })
                                                      .Select(x => new StayDayLfemItem
                                                      {
                                                          LotID = x.Key.LotID,
                                                          Size = x.Key.Size,
                                                          Model = x.Key.Model,
                                                          OperationName = x.Key.OperationName,
                                                          Status = x.Key.Status,
                                                          StayDay = x.Sum(s => s.StayDay),
                                                          Qty = x.Sum(s=>s.ChipQty)
                                                      });
                        result.SmtData = smtData.OrderByDescending(x => x.StayDay).ToList();

                        List<string> OP_AssyLine = new List<string>()
                    { "OC020", "OC2141", "OC2142", "OC2143",
                       "OC2144","OC216","OC217","OC220","OC230",
                       "OC235","OC240","OC245","OC247","OC250",
                       "OC257","OC260","OC290","OC291","OC292",
                       "OC2921","OC2922","OC293","OC294","OC295"
                       ,"OC296","OC300","OC302","OC304","OC310",
                       "OC311","OC312","OC313","OC320","OC330","OC340"
                    };
                        var assyLine = viewWIpLfemDatas.Where(x => OP_AssyLine.Contains(x.Operation))
                                                    .GroupBy(x => new { x.LotID, x.Size, Model = x.Material.Substring(3, 4), x.OperationName, x.Status })
                                                    .Select(x => new StayDayLfemItem
                                                    {
                                                        LotID = x.Key.LotID,
                                                        Size = x.Key.Size,
                                                        Model = x.Key.Model,
                                                        OperationName = x.Key.OperationName,
                                                        Status = x.Key.Status,
                                                        StayDay = x.Sum(s => s.StayDay),
                                                        Qty = x.Sum(s => s.ChipQty)
                                                    });
                        result.AssyLine = assyLine.OrderByDescending(x => x.StayDay).ToList();

                        List<string> OP_TEST = new List<string>() { "OC355", "OC345", "OC357", "OC362", "OC360", "OC365" };
                        var testData = viewWIpLfemDatas.Where(x => OP_TEST.Contains(x.Operation))
                                                      .GroupBy(x => new { x.LotID, x.Size, Model = x.Material.Substring(3, 4), x.OperationName, x.Status })
                                                      .Select(x => new StayDayLfemItem
                                                      {
                                                          LotID = x.Key.LotID,
                                                          Size = x.Key.Size,
                                                          Model = x.Key.Model,
                                                          OperationName = x.Key.OperationName,
                                                          Status = x.Key.Status,
                                                          StayDay = x.Sum(s => s.StayDay),
                                                          Qty = x.Sum(s => s.ChipQty)
                                                      });
                        result.Test = testData.OrderByDescending(x => x.StayDay).ToList();

                        // 공정별 정체재공(Reel Packing)
                        List<string> OP_FA = new List<string>() { "OC370", "OC380" };
                        var FaData = viewWIpLfemDatas.Where(x => OP_FA.Contains(x.Operation))
                                                      .GroupBy(x => new { x.LotID, x.Size, Model = x.Material.Substring(3, 4), x.OperationName, x.Status })
                                                      .Select(x => new StayDayLfemItem
                                                      {
                                                          LotID = x.Key.LotID,
                                                          Size = x.Key.Size,
                                                          Model = x.Key.Model,
                                                          OperationName = x.Key.OperationName,
                                                          Status = x.Key.Status,
                                                          StayDay = x.Sum(s => s.StayDay),
                                                          Qty = x.Sum(s => s.ChipQty)
                                                      });
                        result.FA = FaData.OrderByDescending(x => x.StayDay).ToList();

                        List<string> OP_VI = new List<string>() { "OC410" };
                        var ViData = viewWIpLfemDatas.Where(x => OP_VI.Contains(x.Operation))
                                                      .GroupBy(x => new { x.LotID, x.Size, Model = x.Material.Substring(3, 4), x.OperationName, x.Status })
                                                      .Select(x => new StayDayLfemItem
                                                      {
                                                          LotID = x.Key.LotID,
                                                          Size = x.Key.Size,
                                                          Model = x.Key.Model,
                                                          OperationName = x.Key.OperationName,
                                                          Status = x.Key.Status,
                                                          StayDay = x.Sum(s => s.StayDay),
                                                          Qty = x.Sum(s => s.ChipQty)
                                                      });
                        result.VI = ViData.OrderByDescending(x => x.StayDay).ToList();

                        List<string> OP_OQC = new List<string>() { "OC420" };
                        var OqcData = viewWIpLfemDatas.Where(x => OP_OQC.Contains(x.Operation))
                                                      .GroupBy(x => new { x.LotID, x.Size, Model = x.Material.Substring(3, 4), x.OperationName, x.Status })
                                                      .Select(x => new StayDayLfemItem
                                                      {
                                                          LotID = x.Key.LotID,
                                                          Size = x.Key.Size,
                                                          Model = x.Key.Model,
                                                          OperationName = x.Key.OperationName,
                                                          Status = x.Key.Status,
                                                          StayDay = x.Sum(s => s.StayDay),
                                                          Qty = x.Sum(s => s.ChipQty)
                                                      });
                        result.OQC = OqcData.OrderByDescending(x => x.StayDay).ToList();
                    }

                    if (sheetNumber == "3")
                    {
                        // 공정별 정체재공(완제품)
                       
                        var lstFinish = viewWIpLfemDatas.Where(x => OP_Finish.Contains(x.Operation)).GroupBy(x => x.Material.Substring(3, 4)).Select(x => new StayDayLfemItem
                        {
                            StayDay = x.Average(x => x.StayDay),
                            LotIDCount = x.Count(c => c.LotID != ""),
                            Model = x.Key

                        });

                        result.FinishProduct.StayDayLfemItems = lstFinish.OrderByDescending(x => x.LotIDCount).ToList();

                        var finishData = viewWIpLfemDatas.Where(x => OP_Finish.Contains(x.Operation))
                                                     .GroupBy(x => new { x.LotID, x.Size, Model = x.Material.Substring(3, 4), x.OperationName, x.Status })
                                                     .Select(x => new StayDayLfemItem
                                                     {
                                                         LotID = x.Key.LotID,
                                                         Size = x.Key.Size,
                                                         Model = x.Key.Model,
                                                         OperationName = x.Key.OperationName,
                                                         Status = x.Key.Status,
                                                         StayDay = x.Sum(s => s.StayDay)
                                                     });

                        // data table
                        result.FinishProduct.StayDayLfemItems1 = finishData.OrderByDescending(x => x.StayDay).ToList();

                        result.FinishProduct.LotCount = viewWIpLfemDatas.Where(x => OP_Finish.Contains(x.Operation)).Select(x => x.LotID).Count();
                        result.FinishProduct.ChipQTYSum = Math.Round(viewWIpLfemDatas.Where(x => OP_Finish.Contains(x.Operation)).Select(x => x.ChipQty).Sum(),0);
                        result.FinishProduct.StayDayAvg = Math.Round(viewWIpLfemDatas.Where(x => OP_Finish.Contains(x.Operation)).Select(x => x.StayDay).Average(),1);

                        result.FinishProduct.ChipQTYSumStr = result.FinishProduct.ChipQTYSum >= 1000000 ? Math.Round(result.FinishProduct.ChipQTYSum / 1000000, 1) + "M" : Math.Round(result.FinishProduct.ChipQTYSum / 1000, 1) + "K";
                        result.FinishProduct.OperationV = operation.NullString();
                        result.FinishProduct.ModelV = model.NullString();
                        result.FinishProduct.StayDayV = stayday;
                    }
                }
            }

            result.TotalProduct.OperationV = operation.NullString();
            result.TotalProduct.ModelV = model.NullString();
            result.TotalProduct.StayDayV = stayday;

            result.FinishProduct.OperationV = operation.NullString();
            result.FinishProduct.ModelV = model.NullString();
            result.FinishProduct.StayDayV = stayday;
            return result;
        }
    }
}
