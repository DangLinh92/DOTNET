using OfficeOpenXml;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels.SCP;
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
using System.Threading.Tasks;

namespace OPERATION_MNS.Application.Implementation
{
    public class GOCModuleService : BaseService, IGOCModuleService
    {
        private IRespository<GOC_PRODUCTION_PLAN_LFEM_UPDATE, int> _GocProductionPlanLfemUpdateResponsitory;
        private IRespository<GOC_PRODUCTION_PLAN_LFEM, int> _GocProductionPlanLfemResponsitory;
        private IRespository<GOC_PLAN_LFEM, int> _GocPlanLfemResponsitory;
        private IRespository<PLAN_RANGE_TIME, int> _PlanRangeTimeResponsitory;
        private IUnitOfWork _unitOfWork;
        private ISCPService _SCPService;

        public GOCModuleService(ISCPService SCPService, IRespository<GOC_PRODUCTION_PLAN_LFEM, int> GocProductionPlanLfemResponsitory, IRespository<PLAN_RANGE_TIME, int> PlanRangeTimeResponsitory, IRespository<GOC_PRODUCTION_PLAN_LFEM_UPDATE, int> gocProductionPlanLfemUpdateResponsitory, IRespository<GOC_PLAN_LFEM, int> gocPlanLfemUpdateResponsitory, IUnitOfWork unitOfWork)
        {
            _GocProductionPlanLfemUpdateResponsitory = gocProductionPlanLfemUpdateResponsitory;
            _GocPlanLfemResponsitory = gocPlanLfemUpdateResponsitory;
            _PlanRangeTimeResponsitory = PlanRangeTimeResponsitory;
            _GocProductionPlanLfemResponsitory = GocProductionPlanLfemResponsitory;
            _SCPService = SCPService;
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<GOC_PRODUCTION_PLAN_LFEM_UPDATE> GetSalesByPlanId(string planId, string masterId, string siteId)
        {
            return _GocProductionPlanLfemUpdateResponsitory.FindAll(x => x.PlanID == planId && x.MasterID == masterId && x.SiteId == siteId).ToList();
        }

        public List<GOC_PRODUCTION_PLAN_LFEM_UPDATE> GetSalesPlanByMonth(string month)
        {
            List<GOC_PRODUCTION_PLAN_LFEM_UPDATE> data = new List<GOC_PRODUCTION_PLAN_LFEM_UPDATE>();
            List<GOC_PRODUCTION_PLAN_LFEM> lstGoc = _GocProductionPlanLfemResponsitory.FindAll(x => x.MonthPlan == month).ToList();

            GOC_PRODUCTION_PLAN_LFEM_UPDATE plan;
            foreach (var item in lstGoc)
            {
                plan = new GOC_PRODUCTION_PLAN_LFEM_UPDATE()
                {
                    MesItemId = item.MesItemId,
                    DatePlan = item.DatePlan,
                    MonthPlan = item.MonthPlan,
                    WeekPlan = item.WeekPlan,
                    QuantityPlan_DEMAND = item.QuantityPlan_DEMAND,
                    QuantityActual_DEMAND = item.QuantityActual_DEMAND,
                    QuantityActual_STOCK = item.QuantityActual_STOCK,
                };
            }
        }

        /// <summary>
        /// Import plan xuất hàng
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultDB> ImportGocSales(string filePath, string param)
        {
            ResultDB resultDB = null;
            try
            {
                string MasterId = "SCP";
                string PlanId = param.Split("^")[0].NullString();
                string SiteID = param.Split("^")[1].NullString(); // WHNP1

                using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ExcelWorksheet chipSheet = packet.Workbook.Worksheets[1];
                    List<GOC_PRODUCTION_PLAN_LFEM_UPDATE> lstGocPlanLfem = new List<GOC_PRODUCTION_PLAN_LFEM_UPDATE>();
                    List<GOC_PRODUCTION_PLAN_LFEM_UPDATE> lstGocPlanLfemUpdate = new List<GOC_PRODUCTION_PLAN_LFEM_UPDATE>();

                    string beginDate = DateTime.Parse(chipSheet.Cells["B1"].Text.NullString()).ToString("yyyy-MM-dd");
                    string endDate = "";

                    string _day = "";

                    var maserIdModel = _SCPService.GetMasterIdCode();
                    if (maserIdModel == null)
                    {
                        await _SCPService.SCP_LoginData();
                    }
                    List<SCPInventory> Inventorys = await _SCPService.GetSCPInventorys(MasterId, PlanId, SiteID);
                    string DateInventory = PlanId.Split('_')[0].NullString().Substring(0, 4) + "-" + PlanId.Split('_')[0].NullString().Substring(4, 2) + "-" + PlanId.Split('_')[0].NullString().Substring(6, 2); // 20230918_P005
                    string mesItemID = "";
                    List<SCPInventory> inventoryByMasterial = new List<SCPInventory>();

                    for (int i = chipSheet.Dimension.Start.Row + 2; i <= chipSheet.Dimension.End.Row; i++)
                    {
                        if (chipSheet.Cells[i, 1].Text.NullString() == "")
                        {
                            break;
                        }
                        mesItemID = chipSheet.Cells[i, 1].Text.NullString();

                        for (int k = 0; k < 40; k++)
                        {
                            if (chipSheet.Cells[2, k + 4].Text.NullString() == "" || !DateTime.TryParse(chipSheet.Cells[2, k + 4].Text.NullString(), out _))
                            {
                                break;
                            }

                            _day = DateTime.Parse(chipSheet.Cells[2, k + 4].Text.NullString()).ToString("yyyy-MM-dd");

                            endDate = _day;

                            GOC_PRODUCTION_PLAN_LFEM_UPDATE lfemPlan = _GocProductionPlanLfemUpdateResponsitory.FindSingle(x => x.MasterID == MasterId && x.PlanID == PlanId && x.SiteId == SiteID &&
                                                                                                                           x.MesItemId == mesItemID && x.DatePlan == _day);
                            GOC_PLAN_LFEM lfemgocPlan = _GocPlanLfemResponsitory.FindSingle(x => x.MesItemId == mesItemID && x.DatePlan == _day && x.DanhMuc == CommonConstants.DEMAND);
                            GOC_PLAN_LFEM lfemgocSX = _GocPlanLfemResponsitory.FindSingle(x => x.MesItemId == mesItemID && x.DatePlan == _day && x.DanhMuc == CommonConstants.KHSX);

                            // là ngày chốt tồn đầu ngày theo SCP để làm kế hoạch
                            if (DateInventory == _day)
                            {
                                inventoryByMasterial = Inventorys.FindAll(x => x.mesItemId == mesItemID);
                            }

                            if (lfemPlan == null)
                            {
                                lfemPlan = new GOC_PRODUCTION_PLAN_LFEM_UPDATE()
                                {
                                    MasterID = MasterId,
                                    PlanID = PlanId,
                                    SiteId = SiteID,
                                    MesItemId = chipSheet.Cells[i, 1].Text.NullString(),
                                    UserCreated = GetUserId(),
                                    DatePlan = _day,
                                    MonthPlan = _day.Substring(0, 7) + "-01",
                                    QuantityPlan_DEMAND = double.Parse(chipSheet.Cells[i, k + 4].Value.IfNullIsZero()),
                                    DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                    WeekPlan = "W" + (DateTime.Parse(_day).GetWeekOfYear() + 1)
                                };

                                if (lfemgocPlan != null)
                                {
                                    lfemPlan.QuantityActual_DEMAND = lfemgocPlan.QuantityActual;
                                }

                                if (lfemgocSX != null)
                                {
                                    lfemPlan.QuantityActual_KHSX = lfemgocSX.QuantityActual;
                                    lfemPlan.QuantityPlan_KHSX = lfemgocSX.QuantityPlan;
                                }

                                // UPDATE TỒN THÀNH PHẨM CỦA NGÀY CHỐT THEO SCP : 8AM
                                if (DateInventory == _day)
                                {
                                    if (inventoryByMasterial.Count > 0)
                                    {
                                        lfemPlan.Size = inventoryByMasterial.FirstOrDefault().size;
                                        lfemPlan.QuantityActual_STOCK = inventoryByMasterial.Sum(x => x.qty);
                                    }
                                    else
                                    {
                                        lfemPlan.Size = "";
                                        lfemPlan.QuantityActual_STOCK = 0;
                                    }
                                }

                                lstGocPlanLfem.Add(lfemPlan);
                            }
                            else
                            {
                                lfemPlan.UserModified = GetUserId();
                                lfemPlan.QuantityPlan_DEMAND = double.Parse(chipSheet.Cells[i, k + 4].Value.IfNullIsZero());
                                lfemPlan.DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                lfemPlan.MonthPlan = _day.Substring(0, 7) + "-01";
                                lfemPlan.WeekPlan = "W" + (DateTime.Parse(_day).GetWeekOfYear() + 1);

                                if (lfemgocPlan != null)
                                {
                                    lfemPlan.QuantityActual_DEMAND = lfemgocPlan.QuantityActual;
                                }

                                if (lfemgocSX != null)
                                {
                                    lfemPlan.QuantityActual_KHSX = lfemgocSX.QuantityActual;
                                    lfemPlan.QuantityPlan_KHSX = lfemgocSX.QuantityPlan;
                                }

                                if (DateInventory == _day)
                                {
                                    if (inventoryByMasterial.Count > 0)
                                    {
                                        lfemPlan.Size = inventoryByMasterial.FirstOrDefault().size;
                                        lfemPlan.QuantityActual_STOCK = inventoryByMasterial.Sum(x => x.qty);
                                    }
                                    else
                                    {
                                        lfemPlan.Size = "";
                                        lfemPlan.QuantityActual_STOCK = 0;
                                    }
                                }

                                lstGocPlanLfemUpdate.Add(lfemPlan);
                            }
                        }
                    }

                    // Lưu thời gian bắt đầu, kết thúc cho từng kế hoạch xuất hàng
                    PLAN_RANGE_TIME rangetime = _PlanRangeTimeResponsitory.FindSingle(x => x.PlanId == PlanId && x.MasterId == MasterId && x.SiteId == SiteID);
                    if (rangetime != null)
                    {
                        rangetime.FromDate = DateInventory;
                        rangetime.EndDate = endDate;
                        // rangetime.PlanId = PlanId;
                        // rangetime.IsUse = false;
                        _PlanRangeTimeResponsitory.Update(rangetime);
                    }
                    else
                    {
                        rangetime = new PLAN_RANGE_TIME()
                        {
                            PlanId = PlanId,
                            MasterId = MasterId,
                            SiteId = SiteID,
                            FromDate = DateInventory,
                            EndDate = endDate,
                            IsUse = false
                        };

                        _PlanRangeTimeResponsitory.Add(rangetime);
                    }

                    // Tính MODULE STOCK
                    string _pDate = DateInventory;
                    GOC_PRODUCTION_PLAN_LFEM_UPDATE preLfem;
                    GOC_PRODUCTION_PLAN_LFEM_UPDATE cLfem;
                    string preDate = "";
                    List<string> lstMesItemID = new List<string>();

                    lstMesItemID.AddRange(lstGocPlanLfem.Select(x => x.MesItemId).Distinct().ToList());
                    lstMesItemID.AddRange(lstGocPlanLfemUpdate.Select(x => x.MesItemId).Distinct().ToList());

                    foreach (var mesItemId in lstMesItemID)
                    {
                        do
                        {
                            if (_pDate.CompareTo(DateInventory) > 0)
                            {
                                if (lstGocPlanLfem.Any(x => x.MesItemId == mesItemId))
                                {
                                    preDate = DateTime.Parse(_pDate).AddDays(-1).ToString("yyyy-MM-dd");
                                    preLfem = lstGocPlanLfem.FirstOrDefault(x => x.DatePlan == preDate && x.MesItemId == mesItemId);

                                    cLfem = lstGocPlanLfem.FirstOrDefault(x => x.DatePlan == _pDate && x.MesItemId == mesItemId);

                                    if (_pDate.CompareTo(DateTime.Now.ToString("yyyy-MM-dd")) >= 0)
                                    {
                                        // stock = preStock + preSX - curent Xuat Hang
                                        cLfem.QuantityActual_STOCK = preLfem.QuantityActual_STOCK - cLfem.QuantityPlan_DEMAND; // kế hoach sx = 0 do cần làm kế hoạch cho ngày >= _pDate 
                                    }
                                    else
                                    {
                                        // thời gian quá khứ thì lấy dữ liệu thực tế
                                        cLfem.QuantityActual_STOCK = preLfem.QuantityActual_STOCK + preLfem.QuantityActual_KHSX - cLfem.QuantityActual_DEMAND;
                                    }
                                }
                                else
                                if (lstGocPlanLfemUpdate.Any(x => x.MesItemId == mesItemId))
                                {
                                    preDate = DateTime.Parse(_pDate).AddDays(-1).ToString("yyyy-MM-dd");
                                    preLfem = lstGocPlanLfemUpdate.FirstOrDefault(x => x.DatePlan == preDate && x.MesItemId == mesItemId);

                                    cLfem = lstGocPlanLfemUpdate.FirstOrDefault(x => x.DatePlan == _pDate && x.MesItemId == mesItemId);

                                    if (_pDate.CompareTo(DateTime.Now.ToString("yyyy-MM-dd")) >= 0)
                                    {
                                        // stock = preStock + preSX - curent Xuat Hang
                                        cLfem.QuantityActual_STOCK = preLfem.QuantityActual_STOCK - cLfem.QuantityPlan_DEMAND; // kế hoach sx = 0 do cần làm kế hoạch cho ngày >= _pDate 
                                    }
                                    else
                                    {
                                        // thời gian quá khứ thì lấy dữ liệu thực tế
                                        cLfem.QuantityActual_STOCK = preLfem.QuantityActual_STOCK + preLfem.QuantityActual_KHSX - cLfem.QuantityActual_DEMAND;
                                    }
                                }
                            }

                            _pDate = DateTime.Parse(_pDate).AddDays(1).ToString("yyyy-MM-dd");
                        }
                        while (_pDate.CompareTo(endDate) <= 0);
                    }

                    // Update kế hoạch xuất hàng mới nhất sẽ gửi cho bộ phận, tổng hợp kế hoạch mỗi khi import
                    // 
                    //List<GOC_PRODUCTION_PLAN_LFEM> lstGocPlanAdd = new List<GOC_PRODUCTION_PLAN_LFEM>();
                    //List<GOC_PRODUCTION_PLAN_LFEM> lstGocPlanUpdate = new List<GOC_PRODUCTION_PLAN_LFEM>();
                    //List<GOC_PRODUCTION_PLAN_LFEM_UPDATE> lstNewGoc = new List<GOC_PRODUCTION_PLAN_LFEM_UPDATE>();
                    //lstNewGoc.AddRange(lstGocPlanLfem);
                    //lstNewGoc.AddRange(lstGocPlanLfemUpdate);
                    //GOC_PRODUCTION_PLAN_LFEM goc;

                    //foreach (var item in lstNewGoc)
                    //{
                    //    if (_GocProductionPlanLfemResponsitory.FindSingle(x => x.MesItemId == item.MesItemId && x.DatePlan == item.DatePlan) == null)
                    //    {
                    //        goc = new GOC_PRODUCTION_PLAN_LFEM()
                    //        {
                    //            MesItemId = item.MesItemId,
                    //            DatePlan = item.DatePlan,
                    //            UserCreated = GetUserId(),
                    //            MonthPlan = item.MonthPlan,
                    //            DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    //            WeekPlan = item.WeekPlan,
                    //            QuantityPlan_DEMAND = item.QuantityPlan_DEMAND
                    //        };

                    //        if(goc.DatePlan.CompareTo(DateInventory) >= 0 && goc.DatePlan.CompareTo(endDate) <= 0)
                    //        {
                    //            goc.QuantityActual_STOCK = item.QuantityPlan_STOCK;
                    //        }

                    //        lstGocPlanAdd.Add(goc);
                    //    }
                    //    else
                    //    {
                    //        goc = _GocProductionPlanLfemResponsitory.FindSingle(x => x.MesItemId == item.MesItemId && x.DatePlan == item.DatePlan);
                    //        goc.QuantityPlan_DEMAND = item.QuantityPlan_DEMAND;
                    //        goc.DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    //        if (goc.DatePlan.CompareTo(DateInventory) >= 0 && goc.DatePlan.CompareTo(endDate) <= 0)
                    //        {
                    //            goc.QuantityActual_STOCK = item.QuantityPlan_STOCK;
                    //        }
                    //        lstGocPlanUpdate.Add(goc);
                    //    }
                    //}

                    //if (lstGocPlanAdd.Count > 0)
                    //    _GocProductionPlanLfemResponsitory.AddRange(lstGocPlanAdd);

                    //if (lstGocPlanUpdate.Count > 0)
                    //    _GocProductionPlanLfemResponsitory.UpdateRange(lstGocPlanUpdate);

                    if (lstGocPlanLfem.Count > 0)
                        _GocProductionPlanLfemUpdateResponsitory.AddRange(lstGocPlanLfem);

                    if (lstGocPlanLfemUpdate.Count > 0)
                        _GocProductionPlanLfemUpdateResponsitory.UpdateRange(lstGocPlanLfemUpdate);

                    Save();

                    resultDB = new ResultDB()
                    {
                        ReturnInt = 0
                    };
                }
            }
            catch (Exception ex)
            {
                resultDB = new ResultDB()
                {
                    ReturnInt = -1,
                    ReturnString = ex.Message
                };
            }

            return resultDB;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
