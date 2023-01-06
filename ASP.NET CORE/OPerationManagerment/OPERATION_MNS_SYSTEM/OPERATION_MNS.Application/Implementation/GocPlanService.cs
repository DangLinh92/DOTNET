using AutoMapper;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Infrastructure.Interfaces;
using OPERATION_MNS.Utilities.Constants;
using OPERATION_MNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace OPERATION_MNS.Application.Implementation
{
    public class GocPlanService : BaseService, IGocPlanService
    {
        private IRespository<GOC_PLAN, int> _GocPlanRepository;
        private IRespository<FAB_PLAN, int> _FABPlanRepository;
        private IRespository<GOC_STANDAR_QTY, int> _GocStandarQtyRepository;
        private IRespository<MATERIAL_TO_SAP, int> _MaterialToSapRepository;
        private IRespository<CTQ_SETTING, int> _CTQ_SettingRepository;
        private IRespository<DATE_OFF_LINE, int> _DateOffLineRepository;
        private IRespository<LEAD_TIME_WLP, int> _LeadTimeRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GocPlanService(IRespository<GOC_PLAN, int> GocPlanRepository,
                              IRespository<MATERIAL_TO_SAP, int> MaterialToSapRepository,
                              IRespository<CTQ_SETTING, int> CTQ_SettingRepository,
                              IRespository<GOC_STANDAR_QTY, int> GocStandarQtyRepository,
                              IRespository<DATE_OFF_LINE, int> DateOffLineRepository,
                              IRespository<FAB_PLAN, int> FABPlanRepository,
                              IRespository<LEAD_TIME_WLP, int> LeadTimeRepository,
                              IUnitOfWork unitOfWork, IMapper mapper,
                              IHttpContextAccessor httpContextAccessor)
        {
            _LeadTimeRepository = LeadTimeRepository;
            _GocPlanRepository = GocPlanRepository;
            _FABPlanRepository = FABPlanRepository;
            _GocStandarQtyRepository = GocStandarQtyRepository;
            _MaterialToSapRepository = MaterialToSapRepository;
            _CTQ_SettingRepository = CTQ_SettingRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _DateOffLineRepository = DateOffLineRepository;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Add(GocPlanViewModel model)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            _GocPlanRepository.Remove(id);
            _unitOfWork.Commit();
        }

        public List<GocPlanViewModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public GocPlanViewModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ResultDB ImportExcel(string filePath, string param)
        {
            ResultDB resultDB = new ResultDB();

            try
            {
                if (param == "STANDAR_QTY")
                {
                    using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                    {
                        ExcelWorksheet chipSheet = packet.Workbook.Worksheets[1];
                        List<GOC_STANDAR_QTY> lstChipStandarQty = new List<GOC_STANDAR_QTY>();

                        // import chip
                        for (int i = chipSheet.Dimension.Start.Row + 2; i <= chipSheet.Dimension.End.Row; i++)
                        {
                            if (chipSheet.Cells[i, 2].Text.NullString() == "")
                            {
                                break;
                            }

                            GOC_STANDAR_QTY standarQtyEN = new GOC_STANDAR_QTY()
                            {
                                Module = chipSheet.Cells[i, 3].Text.NullString(),
                                Model = chipSheet.Cells[i, 2].Text.NullString(),
                                Material = chipSheet.Cells[i, 1].Text.NullString(),
                                Division = chipSheet.Cells[i, 5].Text.NullString(),
                                StandardQtyForMonth = float.TryParse(chipSheet.Cells[i, 4].Value.IfNullIsZero(), out _) ? float.Parse(chipSheet.Cells[i, 4].Value.IfNullIsZero()) : 0,
                                UserCreated = GetUserId(),
                                Unit = CommonConstants.CHIP,
                                Department = GetDepartment(),
                                DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                            };

                            GOC_STANDAR_QTY standarQtyWfer = new GOC_STANDAR_QTY()
                            {
                                Module = chipSheet.Cells[i, 3].Text.NullString(),
                                Model = chipSheet.Cells[i, 2].Text.NullString(),
                                Material = chipSheet.Cells[i, 1].Text.NullString(),
                                Division = chipSheet.Cells[i, 5].Text.NullString(),
                                StandardQtyForMonth = float.TryParse(chipSheet.Cells[i, 4].Value.IfNullIsZero(), out _) ? float.Parse(chipSheet.Cells[i, 4].Value.IfNullIsZero()) : 0,
                                UserCreated = GetUserId(),
                                Unit = CommonConstants.WAFER,
                                Department = GetDepartment(),
                                DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                            };
                            lstChipStandarQty.Add(standarQtyEN);
                            lstChipStandarQty.Add(standarQtyWfer);
                        }

                        GOC_STANDAR_QTY en;
                        foreach (var item in lstChipStandarQty)
                        {
                            en = _GocStandarQtyRepository.FindSingle(x => x.Model == item.Model);
                            if (en != null)
                            {
                                _GocStandarQtyRepository.Remove(en);
                            }
                        }

                        _GocStandarQtyRepository.AddRange(lstChipStandarQty);
                    }
                }
                else if (param == "GOC_PLAN")
                {

                    string beginDate = "";
                    string EndDate = "";

                    using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                    {
                        ExcelWorksheet chipSheet = packet.Workbook.Worksheets[1];
                        ExcelWorksheet waferSheet = packet.Workbook.Worksheets[2];
                        List<GOC_PLAN> lstChipPlanQty = new List<GOC_PLAN>();
                        List<GOC_PLAN> lstWaferPlanQty = new List<GOC_PLAN>();
                        Dictionary<string, string> DayNoPlan = new Dictionary<string, string>();

                        // import chip
                        for (int i = chipSheet.Dimension.Start.Row + 3; i <= chipSheet.Dimension.End.Row; i++)
                        {
                            if (chipSheet.Cells[i, 2].Text.NullString() == "")
                            {
                                break;
                            }

                            for (int k = 7; k <= 60; k++)
                            {
                                if (!DateTime.TryParse(chipSheet.Cells[3, 7].Text.NullString(), out DateTime date) || !chipSheet.Cells[3, 7].Text.NullString().Contains("-"))
                                {
                                    resultDB.ReturnInt = -1;
                                    resultDB.ReturnString = "Format ngày tháng theo định dạng yyyy-MM-dd";
                                    return resultDB;
                                }

                                if (!DateTime.TryParse(chipSheet.Cells[3, k].Text.NullString(), out _))
                                {
                                    break;
                                }

                                //if (DateTime.Parse(chipSheet.Cells[3, k].Text.NullString()).ToString("yyyyMM").CompareTo(DateTime.Now.ToString("yyyyMM")) < 0)
                                //{
                                //    resultDB.ReturnInt = -1;
                                //    resultDB.ReturnString = "Ngày Lên Kế Hoạch Đã cũ:" + DateTime.Parse(chipSheet.Cells[3, k].Text.NullString()).ToString("yyyy-MM-dd");
                                //    return resultDB;
                                //}

                                if (k == 7)
                                {
                                    beginDate = DateTime.Parse(chipSheet.Cells[3, k].Text.NullString()).ToString("yyyy-MM-dd");
                                    EndDate = DateTime.Parse(beginDate).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                                }

                                GOC_PLAN standarQtyEN = new GOC_PLAN()
                                {
                                    Module = chipSheet.Cells[i, 3].Text.NullString(),
                                    Model = chipSheet.Cells[i, 2].Text.NullString(),
                                    Material = chipSheet.Cells[i, 1].Text.NullString(),
                                    Division = chipSheet.Cells[i, 5].Text.NullString(),
                                    StandardQtyForMonth = float.TryParse(chipSheet.Cells[i, 4].Value.IfNullIsZero(), out _) ? float.Parse(chipSheet.Cells[i, 4].Value.IfNullIsZero()) : 0,
                                    UserCreated = GetUserId(),
                                    Unit = CommonConstants.CHIP,
                                    Department = GetDepartment(),
                                    QuantityPlan = float.TryParse(chipSheet.Cells[i, k].Value.IfNullIsZero(), out _) ? float.Parse(chipSheet.Cells[i, k].Value.IfNullIsZero()) : 0,
                                    DatePlan = DateTime.Parse(chipSheet.Cells[3, k].Text.NullString()).ToString("yyyy-MM-dd"),
                                    DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                                };

                                if (standarQtyEN.QuantityPlan == 0 || standarQtyEN.Model.ToLower() == "sample")
                                {
                                    if (!DayNoPlan.ContainsKey(standarQtyEN.DatePlan))
                                    {
                                        DayNoPlan.Add(standarQtyEN.DatePlan, "Y");
                                    }
                                }
                                else if (standarQtyEN.QuantityPlan > 0)
                                {
                                    if (DayNoPlan.ContainsKey(standarQtyEN.DatePlan) && DayNoPlan[standarQtyEN.DatePlan] == "Y" && standarQtyEN.Model.ToLower() != "sample")
                                    {
                                        DayNoPlan[standarQtyEN.DatePlan] = "N";
                                    }
                                }

                                lstChipPlanQty.Add(standarQtyEN);
                            }
                        }

                        // import wafer
                        for (int i = waferSheet.Dimension.Start.Row + 3; i <= waferSheet.Dimension.End.Row; i++)
                        {
                            if (waferSheet.Cells[i, 2].Text.NullString() == "")
                            {
                                break;
                            }

                            for (int k = 7; k <= 60; k++)
                            {
                                if (!DateTime.TryParse(waferSheet.Cells[3, 7].Text.NullString(), out DateTime date) || !waferSheet.Cells[3, 7].Text.NullString().Contains("-"))
                                {
                                    resultDB.ReturnInt = -1;
                                    resultDB.ReturnString = "Format ngày tháng theo định dạng yyyy-MM-dd";
                                    return resultDB;
                                }

                                if (!DateTime.TryParse(waferSheet.Cells[3, k].Text.NullString(), out _))
                                {
                                    break;
                                }

                                //if (DateTime.Parse(waferSheet.Cells[3, k].Text.NullString()).ToString("yyyyMM").CompareTo(DateTime.Now.ToString("yyyyMM")) < 0)
                                //{
                                //    resultDB.ReturnInt = -1;
                                //    resultDB.ReturnString = "Ngày Lên Kế Hoạch Đã cũ:" + DateTime.Parse(waferSheet.Cells[3, k].Text.NullString()).ToString("yyyy-MM-dd");
                                //    return resultDB;
                                //}

                                GOC_PLAN standarQtyEN = new GOC_PLAN()
                                {
                                    Module = waferSheet.Cells[i, 3].Text.NullString(),
                                    Model = waferSheet.Cells[i, 2].Text.NullString(),
                                    Material = waferSheet.Cells[i, 1].Text.NullString(),
                                    Division = waferSheet.Cells[i, 5].Text.NullString(),
                                    StandardQtyForMonth = float.TryParse(waferSheet.Cells[i, 4].Value.IfNullIsZero(), out _) ? float.Parse(waferSheet.Cells[i, 4].Value.IfNullIsZero()) : 0,
                                    UserCreated = GetUserId(),
                                    Unit = CommonConstants.WAFER,
                                    Department = GetDepartment(),
                                    QuantityPlan = float.TryParse(waferSheet.Cells[i, k].Value.IfNullIsZero(), out _) ? float.Parse(waferSheet.Cells[i, k].Value.IfNullIsZero()) : 0,
                                    DatePlan = DateTime.Parse(waferSheet.Cells[3, k].Text.NullString()).ToString("yyyy-MM-dd"),
                                    DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                                };

                                lstWaferPlanQty.Add(standarQtyEN);
                            }
                        }

                        lstChipPlanQty.AddRange(lstWaferPlanQty);

                        // xoa item đã có sẵn, add item mới
                        GOC_PLAN en;
                        foreach (var item in lstChipPlanQty.ToList())
                        {
                            en = _GocPlanRepository.FindSingle(x => x.Model == item.Model && x.DatePlan == item.DatePlan && x.Unit == item.Unit);
                            if (en != null)
                            {
                                en.Module = item.Module;
                                en.Model = item.Model;
                                en.Material = item.Material;
                                en.Division = item.Division;
                                en.StandardQtyForMonth = item.StandardQtyForMonth;
                                en.UserCreated = item.UserCreated;
                                en.Unit = item.Unit;
                                en.Department = item.Department;
                                en.QuantityPlan = item.QuantityPlan;
                                en.DatePlan = item.DatePlan;
                                en.DateModified = item.DateModified;
                                _GocPlanRepository.Update(en);

                                lstChipPlanQty.Remove(item);
                            }
                        }

                        if (lstChipPlanQty.Count > 0)
                        {
                            _GocPlanRepository.AddRange(lstChipPlanQty);
                        }

                        // lưu ngày không kế hoạch 
                        DATE_OFF_LINE dateOff;
                        List<DATE_OFF_LINE> lstDateOff = new List<DATE_OFF_LINE>();
                        List<string> dayOff = new List<string>();
                        foreach (KeyValuePair<string, string> item in DayNoPlan)
                        {
                            if (item.Value == "Y")
                            {
                                dateOff = new DATE_OFF_LINE()
                                {
                                    ItemValue = item.Key,
                                    ON_OFF = CommonConstants.OFF,
                                    WLP = "WLP1",
                                    UserCreated = GetUserId()
                                };
                                lstDateOff.Add(dateOff);
                            }
                        }

                        var lstOff = _DateOffLineRepository.FindAll(x => x.ItemValue.CompareTo(beginDate) >= 0 && x.ItemValue.CompareTo(EndDate) <= 0 && x.WLP == "WLP1").ToList();
                        _DateOffLineRepository.RemoveMultiple(lstOff);
                        _DateOffLineRepository.AddRange(lstDateOff);
                        dayOff = lstDateOff.Select(x => x.ItemValue.Replace("-", "")).ToList();

                        //TODO: THÊM DK CHO wlp = WLP1,WLP2 nếu k cùng date off
                        var leadTimes = _LeadTimeRepository.FindAll(x => x.WorkDate.CompareTo(beginDate.Replace("-", "")) >= 0 && x.WorkDate.CompareTo(EndDate.Replace("-", "")) <= 0);

                        foreach (var item in leadTimes)
                        {
                            if (dayOff.Contains(item.WorkDate))
                            {
                                item.Ox = "X";
                            }
                            else
                            {
                                item.Ox = "";
                            }

                            _LeadTimeRepository.Update(item);
                        }

                        Save();

                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add("A_MONTH", beginDate);

                        // LẤY LẠI SẢN LƯỢNG THỰC TẾ trong thang
                        _DateOffLineRepository.ExecProceduce2("UPDATE_PLAN_FROM_MONTH_MANUAL", dic);
                    }
                }
                else if (param == "FAB_WLP1")
                {
                    string beginDate = "";
                    string EndDate = "";

                    using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                    {
                        ExcelWorksheet chipSheet = packet.Workbook.Worksheets["CHIP_PLAN_QTY"];
                        ExcelWorksheet chipActualSheet = packet.Workbook.Worksheets["CHIP_ACTUAL_QTY"];

                        List<FAB_PLAN> lstChipPlanQty = new List<FAB_PLAN>();
                        List<FAB_PLAN> lstWaferPlanQty = new List<FAB_PLAN>();

                        // import chip plan
                        for (int i = chipSheet.Dimension.Start.Row + 3; i <= chipSheet.Dimension.End.Row; i++)
                        {
                            if (chipSheet.Cells[i, 2].Text.NullString() == "")
                            {
                                break;
                            }

                            for (int k = 7; k <= 60; k++)
                            {
                                if (!DateTime.TryParse(chipSheet.Cells[3, 7].Text.NullString(), out DateTime date) || !chipSheet.Cells[3, 7].Text.NullString().Contains("-"))
                                {
                                    resultDB.ReturnInt = -1;
                                    resultDB.ReturnString = "Format ngày tháng theo định dạng yyyy-MM-dd";
                                    return resultDB;
                                }

                                if (!DateTime.TryParse(chipSheet.Cells[3, k].Text.NullString(), out _))
                                {
                                    break;
                                }

                                //if (DateTime.Parse(chipSheet.Cells[3, k].Text.NullString()).ToString("yyyyMM").CompareTo(DateTime.Now.ToString("yyyyMM")) < 0)
                                //{
                                //    resultDB.ReturnInt = -1;
                                //    resultDB.ReturnString = "Ngày Lên Kế Hoạch Đã cũ:" + DateTime.Parse(chipSheet.Cells[3, k].Text.NullString()).ToString("yyyy-MM-dd");
                                //    return resultDB;
                                //}

                                if (k == 7)
                                {
                                    beginDate = DateTime.Parse(chipSheet.Cells[3, k].Text.NullString()).ToString("yyyy-MM-dd");
                                    EndDate = DateTime.Parse(beginDate).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                                }

                                FAB_PLAN standarQtyEN = new FAB_PLAN()
                                {
                                    Module = chipSheet.Cells[i, 3].Text.NullString(),
                                    Model = chipSheet.Cells[i, 2].Text.NullString(),
                                    Material = chipSheet.Cells[i, 1].Text.NullString(),
                                    Division = chipSheet.Cells[i, 5].Text.NullString(),
                                    StandardQtyForMonth = float.TryParse(chipSheet.Cells[i, 4].Value.IfNullIsZero(), out _) ? float.Parse(chipSheet.Cells[i, 4].Value.IfNullIsZero()) : 0,
                                    UserCreated = GetUserId(),
                                    Unit = CommonConstants.CHIP,
                                    Department = GetDepartment(),
                                    QuantityPlan = float.TryParse(chipSheet.Cells[i, k].Value.IfNullIsZero(), out _) ? float.Parse(chipSheet.Cells[i, k].Value.IfNullIsZero()) : 0,
                                    DatePlan = DateTime.Parse(chipSheet.Cells[3, k].Text.NullString()).ToString("yyyy-MM-dd"),
                                    DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                                };

                                FAB_PLAN standarQtyEN_Wf = new FAB_PLAN()
                                {
                                    Module = chipSheet.Cells[i, 3].Text.NullString(),
                                    Model = chipSheet.Cells[i, 2].Text.NullString(),
                                    Material = chipSheet.Cells[i, 1].Text.NullString(),
                                    Division = chipSheet.Cells[i, 5].Text.NullString(),
                                    StandardQtyForMonth = float.TryParse(chipSheet.Cells[i, 4].Value.IfNullIsZero(), out _) ? float.Parse(chipSheet.Cells[i, 4].Value.IfNullIsZero()) : 0,
                                    UserCreated = GetUserId(),
                                    Unit = CommonConstants.WAFER,
                                    Department = GetDepartment(),
                                    QuantityPlan = (float)Math.Round((float.TryParse(chipSheet.Cells[i, k].Value.IfNullIsZero(), out _) ? float.Parse(chipSheet.Cells[i, k].Value.IfNullIsZero()) : 0) / (float.Parse(chipSheet.Cells[i, 4].Value.IfNullIsZero())), 1),
                                    DatePlan = DateTime.Parse(chipSheet.Cells[3, k].Text.NullString()).ToString("yyyy-MM-dd"),
                                    DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                                };

                                lstChipPlanQty.Add(standarQtyEN);

                                lstWaferPlanQty.Add(standarQtyEN_Wf);
                            }
                        }

                        // import chip actual
                        for (int i = chipActualSheet.Dimension.Start.Row + 3; i <= chipActualSheet.Dimension.End.Row; i++)
                        {
                            if (chipActualSheet.Cells[i, 2].Text.NullString() == "")
                            {
                                break;
                            }

                            for (int k = 7; k <= 60; k++)
                            {
                                if (!DateTime.TryParse(chipActualSheet.Cells[3, 7].Text.NullString(), out DateTime date) || !chipActualSheet.Cells[3, 7].Text.NullString().Contains("-"))
                                {
                                    resultDB.ReturnInt = -1;
                                    resultDB.ReturnString = "Format ngày tháng theo định dạng yyyy-MM-dd";
                                    return resultDB;
                                }

                                if (!DateTime.TryParse(chipActualSheet.Cells[3, k].Text.NullString(), out _))
                                {
                                    break;
                                }

                                //if (DateTime.Parse(chipActualSheet.Cells[3, k].Text.NullString()).ToString("yyyyMM").CompareTo(DateTime.Now.ToString("yyyyMM")) < 0)
                                //{
                                //    resultDB.ReturnInt = -1;
                                //    resultDB.ReturnString = "Ngày Lên Kế Hoạch Đã cũ:" + DateTime.Parse(chipActualSheet.Cells[3, k].Text.NullString()).ToString("yyyy-MM-dd");
                                //    return resultDB;
                                //}

                                if (k == 7)
                                {
                                    beginDate = DateTime.Parse(chipActualSheet.Cells[3, k].Text.NullString()).ToString("yyyy-MM-dd");
                                    EndDate = DateTime.Parse(beginDate).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                                }

                                FAB_PLAN standarQtyEN = lstChipPlanQty.FirstOrDefault(x => x.Model == chipActualSheet.Cells[i, 2].Text.NullString() && x.DatePlan == DateTime.Parse(chipActualSheet.Cells[3, k].Text.NullString()).ToString("yyyy-MM-dd"));
                                FAB_PLAN standarQtyEN_Wf = lstWaferPlanQty.FirstOrDefault(x => x.Model == chipActualSheet.Cells[i, 2].Text.NullString() && x.DatePlan == DateTime.Parse(chipActualSheet.Cells[3, k].Text.NullString()).ToString("yyyy-MM-dd"));
                                if (standarQtyEN != null)
                                {
                                    standarQtyEN.QuantityActual = float.TryParse(chipActualSheet.Cells[i, k].Value.IfNullIsZero(), out _) ? float.Parse(chipActualSheet.Cells[i, k].Value.IfNullIsZero()) : 0;
                                }

                                if (standarQtyEN_Wf != null)
                                {
                                    standarQtyEN_Wf.QuantityActual = (float)Math.Round((float.TryParse(chipActualSheet.Cells[i, k].Value.IfNullIsZero(), out _) ? float.Parse(chipActualSheet.Cells[i, k].Value.IfNullIsZero()) : 0) / (float.Parse(chipActualSheet.Cells[i, 4].Value.IfNullIsZero())), 1);
                                }
                            }
                        }

                        lstChipPlanQty.AddRange(lstWaferPlanQty);

                        // xoa item đã có sẵn, add item mới
                        FAB_PLAN en;
                        foreach (var item in lstChipPlanQty.ToList())
                        {
                            en = _FABPlanRepository.FindSingle(x => x.Model == item.Model && x.DatePlan == item.DatePlan && x.Unit == item.Unit);
                            if (en != null)
                            {
                                en.Module = item.Module;
                                en.Model = item.Model;
                                en.Material = item.Material;
                                en.Division = item.Division;
                                en.StandardQtyForMonth = item.StandardQtyForMonth;
                                en.UserCreated = item.UserCreated;
                                en.Unit = item.Unit;
                                en.Department = item.Department;
                                en.QuantityPlan = item.QuantityPlan;
                                en.QuantityActual = item.QuantityActual;
                                en.DatePlan = item.DatePlan;
                                en.DateModified = item.DateModified;
                                _FABPlanRepository.Update(en);

                                lstChipPlanQty.Remove(item);
                            }
                        }

                        if (lstChipPlanQty.Count > 0)
                        {
                            _FABPlanRepository.AddRange(lstChipPlanQty);
                        }

                        Save();

                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add("A_MONTH", beginDate);
                        // Tính GAP
                        _FABPlanRepository.ExecProceduce2("UPDATE_FAB_WLP1_FROM_MONTH", dic);
                    }
                }

            }
            catch (Exception ex)
            {
                resultDB.ReturnInt = -1;
                resultDB.ReturnString = ex.Message;
                return resultDB;
            }

            resultDB.ReturnInt = 0;
            return resultDB;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(GocPlanViewModel model)
        {
            throw new NotImplementedException();
        }

        public List<GocPlanViewModelEx> GetByTime(string unit, string fromDate, string toDate)
        {
            List<GocPlanViewModelEx> lstResult = new List<GocPlanViewModelEx>();

            var lstStandar = _GocStandarQtyRepository.FindAll(x => x.Unit == unit);

            GocPlanViewModelEx plan;

            foreach (var item in lstStandar.ToList())
            {
                plan = new GocPlanViewModelEx()
                {
                    Id = item.Id,
                    Module = item.Module,
                    Model = item.Model,
                    Material = item.Material,
                    Division = item.Division,
                    StandardQtyForMonth = item.StandardQtyForMonth,
                    Unit = unit
                };

                var lstQty = _GocPlanRepository.FindAll(x =>
                x.Model == item.Model &&
                x.Unit == unit &&
                x.DatePlan.CompareTo(fromDate) >= 0 && x.DatePlan.CompareTo(toDate) <= 0);

                float totalPlan = 0;
                float totalActual = 0;
                float totalGap = 0;

                foreach (var qty in lstQty.ToList())
                {
                    plan.QuantityByDays.Add(new QuantityByDay()
                    {
                        DatePlan = qty.DatePlan,
                        Id = qty.Id,
                        QuantityPlan = qty.QuantityPlan,//unit == CommonConstants.CHIP ? (float)Math.Round(qty.QuantityPlan / 1000) : (float)Math.Round(qty.QuantityPlan),
                        QuantityActual = qty.QuantityActual,// unit == CommonConstants.CHIP ? (float)Math.Round(qty.QuantityActual / 1000, 0) : (float)Math.Round(qty.QuantityActual),
                        QuantityGap = qty.QuantityGap//unit == CommonConstants.CHIP ? (float)Math.Round(qty.QuantityGap / 1000, 0) : (float)Math.Round(qty.QuantityGap),
                    });

                    totalPlan += qty.QuantityPlan;//unit == CommonConstants.CHIP ? (float)Math.Round(qty.QuantityPlan / 1000, 0) : (float)Math.Round(qty.QuantityPlan);
                    totalActual += qty.QuantityActual;//unit == CommonConstants.CHIP ? (float)Math.Round(qty.QuantityActual / 1000, 0) : (float)Math.Round(qty.QuantityActual);
                    totalGap += qty.QuantityGap;//unit == CommonConstants.CHIP ? (float)Math.Round(qty.QuantityGap / 1000, 0) : (float)Math.Round(qty.QuantityGap);
                }

                plan.Total_Plan = unit == CommonConstants.CHIP ? (float)Math.Round(totalPlan / 1000, 0) : (float)Math.Round(totalPlan, 0);
                plan.Total_Actual = unit == CommonConstants.CHIP ? (float)Math.Round(totalActual / 1000, 0) : (float)Math.Round(totalActual, 0);
                plan.Total_Gap = plan.Total_Plan - plan.Total_Actual;//unit == CommonConstants.CHIP ? (float)Math.Round(totalGap / 1000, 0) : (float)Math.Round(totalGap, 0);

                lstResult.Add(plan);
            }

            return lstResult;
        }

        public List<GocPlanViewModelEx> GetByTime_fab(string unit, string fromDate, string toDate)
        {
            List<GocPlanViewModelEx> lstResult = new List<GocPlanViewModelEx>();

            var lstStandar = _GocStandarQtyRepository.FindAll(x => x.Unit == unit);

            GocPlanViewModelEx plan;

            foreach (var item in lstStandar.ToList())
            {
                plan = new GocPlanViewModelEx()
                {
                    Id = item.Id,
                    Module = item.Module,
                    Model = item.Model,
                    Material = item.Material,
                    Division = item.Division,
                    StandardQtyForMonth = item.StandardQtyForMonth,
                    Unit = unit
                };

                var lstQty = _FABPlanRepository.FindAll(x =>
                x.Model == item.Model &&
                x.Unit == unit &&
                x.DatePlan.CompareTo(fromDate) >= 0 && x.DatePlan.CompareTo(toDate) <= 0);

                float totalPlan = 0;
                float totalActual = 0;
                float totalGap = 0;

                foreach (var qty in lstQty.ToList())
                {
                    plan.QuantityByDays.Add(new QuantityByDay()
                    {
                        DatePlan = qty.DatePlan,
                        Id = qty.Id,
                        QuantityPlan = qty.QuantityPlan,//unit == CommonConstants.CHIP ? (float)Math.Round(qty.QuantityPlan / 1000) : (float)Math.Round(qty.QuantityPlan),
                        QuantityActual = qty.QuantityActual,// unit == CommonConstants.CHIP ? (float)Math.Round(qty.QuantityActual / 1000, 0) : (float)Math.Round(qty.QuantityActual),
                        QuantityGap = qty.QuantityGap//unit == CommonConstants.CHIP ? (float)Math.Round(qty.QuantityGap / 1000, 0) : (float)Math.Round(qty.QuantityGap),
                    });

                    totalPlan += qty.QuantityPlan;//unit == CommonConstants.CHIP ? (float)Math.Round(qty.QuantityPlan / 1000, 0) : (float)Math.Round(qty.QuantityPlan);
                    totalActual += qty.QuantityActual;//unit == CommonConstants.CHIP ? (float)Math.Round(qty.QuantityActual / 1000, 0) : (float)Math.Round(qty.QuantityActual);
                    totalGap += qty.QuantityActual;//unit == CommonConstants.CHIP ? (float)Math.Round(qty.QuantityGap / 1000, 0) : (float)Math.Round(qty.QuantityGap);
                }

                plan.Total_Plan = unit == CommonConstants.CHIP ? (float)Math.Round(totalPlan / 1000, 0) : (float)Math.Round(totalPlan, 0);
                plan.Total_Actual = unit == CommonConstants.CHIP ? (float)Math.Round(totalActual / 1000, 0) : (float)Math.Round(totalActual, 0);
                plan.Total_Gap = unit == CommonConstants.CHIP ? (float)Math.Round(totalGap / 1000, 0) : (float)Math.Round(totalGap, 0);

                lstResult.Add(plan);
            }

            return lstResult;
        }

        public void DeleteGocModel(int Id, string fromDate, string toDate)
        {
            GOC_STANDAR_QTY en = _GocStandarQtyRepository.FindById(Id);
            string model = en.Model;
            var lst = _GocPlanRepository.FindAll(x => x.Model == model && x.DatePlan.CompareTo(fromDate) >= 0 && x.DatePlan.CompareTo(toDate) <= 0).ToList();
            _GocPlanRepository.RemoveMultiple(lst);
            _GocStandarQtyRepository.Remove(Id);
            _unitOfWork.Commit();
        }

        public List<ProcActualPlanModel> GetProcActualPlanModel(string month)
        {
            int dayOfMonth = DateTime.Parse(month + "-01").AddMonths(1).AddDays(-1).Day;

            List<ProcActualPlanModel> result = new List<ProcActualPlanModel>();
            var lstGoc = _GocPlanRepository.FindAll(x => x.DatePlan.Contains(month));
            var lstFAB = _FABPlanRepository.FindAll(x => x.DatePlan.Contains(month));

            // CHIP
            var lstGroupChip = (from p in lstGoc.Where(x => x.Unit == CommonConstants.CHIP && x.DatePlan.Contains(month)).AsEnumerable()
                                group p by new
                                {
                                    Day = p.DatePlan
                                } into g
                                select new QuantityByDay
                                {
                                    DatePlan = g.Key.Day.Split("-")[2],
                                    QuantityPlan = g.Sum(x => x.QuantityPlan),
                                    QuantityActual = g.Sum(x => x.QuantityActual),
                                    QuantityGap = g.Sum(x => x.QuantityActual) - g.Sum(x => x.QuantityPlan),
                                }).OrderBy(x => int.Parse(x.DatePlan)).ToList();

            var lstChips = UpdateDataLost(lstGroupChip, dayOfMonth);

            if (lstChips.Count > 0)
            {
                for (int i = 1; i <= dayOfMonth; i++)
                {
                    for (int j = 1; j <= i; j++)
                    {
                        lstChips[i - 1].QtyPlan_Ytd += lstChips[j - 1].QuantityPlan;
                        lstChips[i - 1].QtyActual_Ytd += lstChips[j - 1].QuantityActual;
                        lstChips[i - 1].QtyGap_Ytd += lstChips[j - 1].QuantityGap;
                    }
                }

                float totalYtdPlan = lstChips.Last().QtyPlan_Ytd;
                foreach (var item in lstChips)
                {
                    item.Qty_Percen_Ytd = Math.Round((item.QtyActual_Ytd / totalYtdPlan) * 100, 1);
                }
            }


            // WAFER
            var lstGroupWafer = (from p in lstGoc.Where(x => x.Unit == CommonConstants.WAFER && x.DatePlan.Contains(month)).AsEnumerable()
                                 group p by new
                                 {
                                     Day = p.DatePlan
                                 } into g
                                 select new QuantityByDay
                                 {
                                     DatePlan = g.Key.Day.Split("-")[2],
                                     QuantityPlan = g.Sum(x => x.QuantityPlan),
                                     QuantityActual = g.Sum(x => x.QuantityActual),
                                     QuantityGap = g.Sum(x => x.QuantityActual) - g.Sum(x => x.QuantityPlan),
                                 }).OrderBy(x => int.Parse(x.DatePlan)).ToList();

            var lstWafer = UpdateDataLost(lstGroupWafer, dayOfMonth);

            if (lstWafer.Count > 0)
            {
                for (int i = 1; i <= dayOfMonth; i++)
                {
                    for (int j = 1; j <= i; j++)
                    {
                        lstWafer[i - 1].QtyPlan_Ytd += lstWafer[j - 1].QuantityPlan;
                        lstWafer[i - 1].QtyActual_Ytd += lstWafer[j - 1].QuantityActual;
                        lstWafer[i - 1].QtyGap_Ytd += lstWafer[j - 1].QuantityGap;
                    }
                }

                float totalYtdPlan = lstWafer.Last().QtyPlan_Ytd;
                foreach (var item in lstWafer)
                {
                    item.Qty_Percen_Ytd = Math.Round((item.QtyActual_Ytd / totalYtdPlan) * 100, 1);
                }
            }

            // FAB
            var lstGroupfabWafer = (from p in lstFAB.Where(x => x.Unit == CommonConstants.WAFER && x.DatePlan.Contains(month)).AsEnumerable()
                                    group p by new
                                    {
                                        Day = p.DatePlan
                                    } into g
                                    select new QuantityByDay
                                    {
                                        DatePlan = g.Key.Day.Split("-")[2],
                                        QuantityPlan = g.Sum(x => x.QuantityPlan),
                                        QuantityActual = g.Sum(x => x.QuantityActual),
                                        QuantityGap = g.Sum(x => x.QuantityGap),
                                    }).OrderBy(x => int.Parse(x.DatePlan)).ToList();

            var lstFABWafer = UpdateDataLost(lstGroupfabWafer, dayOfMonth);

            if (lstFABWafer.Count > 0)
            {
                for (int i = 1; i <= dayOfMonth; i++)
                {
                    for (int j = 1; j <= i; j++)
                    {
                        lstFABWafer[i - 1].QtyPlan_Ytd += lstFABWafer[j - 1].QuantityPlan;
                        lstFABWafer[i - 1].QtyActual_Ytd += lstFABWafer[j - 1].QuantityActual;
                        lstFABWafer[i - 1].QtyGap_Ytd += lstFABWafer[j - 1].QuantityGap;
                    }
                }
            }

            ProcActualPlanModel proc = new ProcActualPlanModel();
            proc.QuantityByDays = lstChips;
            proc.Month = month;
            proc.DayOfMonth = dayOfMonth;
            proc.CFAB = CommonConstants.CHIP;

            ProcActualPlanModel procWf = new ProcActualPlanModel();
            procWf.QuantityByDays = lstWafer;
            procWf.Month = month;
            procWf.DayOfMonth = dayOfMonth;
            procWf.CFAB = CommonConstants.WAFER;

            ProcActualPlanModel procWffab = new ProcActualPlanModel();
            procWffab.QuantityByDays = lstFABWafer;
            procWffab.Month = month;
            procWffab.DayOfMonth = dayOfMonth;
            procWffab.CFAB = CommonConstants.FAB_WLP1;

            result.Add(proc);
            result.Add(procWf);
            result.Add(procWffab);

            return result;
        }

        private List<QuantityByDay> UpdateDataLost(List<QuantityByDay> data, int dayOfMonth)
        {
            if (data.Count == 0) return data;

            for (int i = 1; i <= dayOfMonth; i++)
            {
                if (data.Count >= i)
                {
                    if (int.Parse(data[i - 1].DatePlan) != i)
                    {
                        data.Insert(i - 1, new QuantityByDay()
                        {
                            DatePlan = i + "",
                            QuantityPlan = 0,
                            QuantityActual = 0,
                            QuantityGap = 0
                        });
                    }
                }
                else
                {
                    data.Add(new QuantityByDay()
                    {
                        DatePlan = i + "",
                        QuantityPlan = 0,
                        QuantityActual = 0,
                        QuantityGap = 0
                    });
                }
            }

            return data.OrderBy(x => int.Parse(x.DatePlan)).ToList();
        }

        public ViewControlChartDataModel GetDataControlChart(string date, string toDate, string operation, string mattertial)
        {
            ViewControlChartDataModel result = new ViewControlChartDataModel();
            List<ViewControlChartModel> chartModels = new List<ViewControlChartModel>();
            List<ViewControlChartModel> chartModelsErr = new List<ViewControlChartModel>();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("A_OPERATION", operation);
            dic.Add("A_FROM_DATE", date);
            dic.Add("A_TO_DATE", toDate);
            dic.Add("A_MATERTIAL", mattertial);

            ResultDB rs = _GocPlanRepository.ExecProceduce2("PKG_BUSINESS@VIEW_CONTROL_CHART_DATA", dic);
            if (rs.ReturnInt == 0)
            {
                DataTable tbl = rs.ReturnDataSet.Tables[0];
                DataTable tbl_Err = rs.ReturnDataSet.Tables[1];
                foreach (DataRow row in tbl.Rows)
                {
                    ViewControlChartModel model = new ViewControlChartModel();
                    model.CHART_X = row["CHART_X"].NullString();
                    model.DATE = row["DATE"].NullString();
                    model.MATERIAL_ID = row["MATERIAL_ID"].NullString();

                    model.LOT_ID = row["LOT_ID"].NullString();
                    model.CASSETTE_ID = row["CASSETTE_ID"].NullString();
                    model.MAIN_OPERATION = row["MAIN_OPERATION"].NullString();
                    model.MAIN_OPERATION_ID = row["OPERATION_ID"].NullString();

                    model.MAIN_EQUIPMENT_ID = row["MAIN_EQUIPMENT_ID"].NullString();
                    model.MAIN_EQUIPMENT_NAME = row["MAIN_EQUIPMENT_NAME"].NullString();
                    model.MAIN_CHARACTER = row["MAIN_CHARACTER"].NullString();
                    model.MAIN_UNIT = row["MAIN_UNIT"].NullString();
                    model.MAIN_TARGET_USL = double.Parse(row["MAIN_TARGET_USL"].IfNullIsZero());
                    model.MAIN_FIXED_UCL = double.Parse(row["MAIN_FIXED_UCL"].IfNullIsZero());
                    model.MAIN_TARGET = double.Parse(row["MAIN_TARGET"].IfNullIsZero());
                    model.MAIN_FIXED_LCL = double.Parse(row["MAIN_FIXED_LCL"].IfNullIsZero());
                    model.MAIN_TARGET_LSL = double.Parse(row["MAIN_TARGET_LSL"].IfNullIsZero());
                    model.MAIN_TARGET_UCL = double.Parse(row["MAIN_TARGET_UCL"].IfNullIsZero());
                    model.MAIN_TARGET_LCL = double.Parse(row["MAIN_TARGET_LCL"].IfNullIsZero());
                    model.MAIN_VALUE_COUNT = double.Parse(row["MAIN_VALUE_COUNT"].IfNullIsZero());
                    model.MAIN_VALUE1 = double.Parse(row["MAIN_VALUE1"].IfNullIsZero());
                    model.MAIN_VALUE2 = double.Parse(row["MAIN_VALUE2"].IfNullIsZero());
                    model.MAIN_VALUE3 = double.Parse(row["MAIN_VALUE3"].IfNullIsZero());
                    model.MAIN_VALUE4 = double.Parse(row["MAIN_VALUE4"].IfNullIsZero());
                    model.MAIN_VALUE5 = double.Parse(row["MAIN_VALUE5"].IfNullIsZero());
                    model.MAIN_VALUE6 = double.Parse(row["MAIN_VALUE6"].IfNullIsZero());
                    model.MAIN_VALUE7 = double.Parse(row["MAIN_VALUE7"].IfNullIsZero());
                    model.MAIN_VALUE8 = double.Parse(row["MAIN_VALUE8"].IfNullIsZero());
                    model.MAIN_VALUE9 = double.Parse(row["MAIN_VALUE9"].IfNullIsZero());
                    model.MAIN_VALUE10 = double.Parse(row["MAIN_VALUE10"].IfNullIsZero());
                    model.MAIN_VALUE11 = double.Parse(row["MAIN_VALUE11"].IfNullIsZero());
                    model.MAIN_VALUE12 = double.Parse(row["MAIN_VALUE12"].IfNullIsZero());
                    model.MAIN_VALUE13 = double.Parse(row["MAIN_VALUE13"].IfNullIsZero());
                    model.MAIN_VALUE14 = double.Parse(row["MAIN_VALUE14"].IfNullIsZero());
                    model.MAIN_VALUE15 = double.Parse(row["MAIN_VALUE15"].IfNullIsZero());
                    model.MAIN_VALUE16 = double.Parse(row["MAIN_VALUE16"].IfNullIsZero());
                    model.MAIN_VALUE17 = double.Parse(row["MAIN_VALUE17"].IfNullIsZero());
                    model.MAIN_VALUE18 = double.Parse(row["MAIN_VALUE18"].IfNullIsZero());
                    model.MAIN_VALUE19 = double.Parse(row["MAIN_VALUE19"].IfNullIsZero());
                    model.MAIN_VALUE20 = double.Parse(row["MAIN_VALUE20"].IfNullIsZero());
                    model.MAIN_VALUE21 = double.Parse(row["MAIN_VALUE21"].IfNullIsZero());
                    model.MAIN_VALUE22 = double.Parse(row["MAIN_VALUE22"].IfNullIsZero());
                    model.MAIN_VALUE23 = double.Parse(row["MAIN_VALUE23"].IfNullIsZero());
                    model.MAIN_VALUE24 = double.Parse(row["MAIN_VALUE24"].IfNullIsZero());
                    model.MAIN_VALUE25 = double.Parse(row["MAIN_VALUE25"].IfNullIsZero());
                    model.MAIN_VALUE26 = double.Parse(row["MAIN_VALUE26"].IfNullIsZero());
                    model.MAIN_VALUE27 = double.Parse(row["MAIN_VALUE27"].IfNullIsZero());
                    model.MAIN_VALUE28 = double.Parse(row["MAIN_VALUE28"].IfNullIsZero());
                    model.MAIN_VALUE29 = double.Parse(row["MAIN_VALUE29"].IfNullIsZero());
                    model.MAIN_VALUE30 = double.Parse(row["MAIN_VALUE30"].IfNullIsZero());
                    model.MAIN_MAX_VALUE = double.Parse(row["MAIN_MAX_VALUE"].IfNullIsZero());
                    model.MAIN_MIN_VALUE = double.Parse(row["MAIN_MIN_VALUE"].IfNullIsZero());
                    model.MAIN_AVG_VALUE = double.Parse(row["MAIN_AVG_VALUE"].IfNullIsZero());
                    model.MAIN_RANGE = double.Parse(row["MAIN_RANGE"].IfNullIsZero());
                    model.MAIN_JUDGE_FLAG = row["MAIN_JUDGE_FLAG"].NullString();
                    chartModels.Add(model);
                }

                foreach (DataRow row in tbl_Err.Rows)
                {
                    ViewControlChartModel model = new ViewControlChartModel();
                    model.CHART_X = row["CHART_X"].NullString();
                    model.DATE = row["DATE"].NullString();
                    model.MATERIAL_ID = row["MATERIAL_ID"].NullString();

                    model.LOT_ID = row["LOT_ID"].NullString();
                    model.CASSETTE_ID = row["CASSETTE_ID"].NullString();
                    model.MAIN_OPERATION = row["OperationName"].NullString();
                    model.MAIN_OPERATION_ID = row["MAIN_OPERATION"].NullString();

                    model.MAIN_EQUIPMENT_ID = row["MAIN_EQUIPMENT_ID"].NullString();
                    model.MAIN_EQUIPMENT_NAME = row["MAIN_EQUIPMENT_NAME"].NullString();
                    model.MAIN_CHARACTER = row["MAIN_CHARACTER"].NullString();
                    model.MAIN_UNIT = row["MAIN_UNIT"].NullString();
                    model.MAIN_TARGET_USL = double.Parse(row["MAIN_TARGET_USL"].IfNullIsZero());
                    model.MAIN_FIXED_UCL = double.Parse(row["MAIN_FIXED_UCL"].IfNullIsZero());
                    model.MAIN_TARGET = double.Parse(row["MAIN_TARGET"].IfNullIsZero());
                    model.MAIN_FIXED_LCL = double.Parse(row["MAIN_FIXED_LCL"].IfNullIsZero());
                    model.MAIN_TARGET_LSL = double.Parse(row["MAIN_TARGET_LSL"].IfNullIsZero());
                    model.MAIN_TARGET_UCL = double.Parse(row["MAIN_TARGET_UCL"].IfNullIsZero());
                    model.MAIN_TARGET_LCL = double.Parse(row["MAIN_TARGET_LCL"].IfNullIsZero());
                    model.MAIN_VALUE_COUNT = double.Parse(row["MAIN_VALUE_COUNT"].IfNullIsZero());
                    model.MAIN_VALUE1 = double.Parse(row["MAIN_VALUE1"].IfNullIsZero());
                    model.MAIN_VALUE2 = double.Parse(row["MAIN_VALUE2"].IfNullIsZero());
                    model.MAIN_VALUE3 = double.Parse(row["MAIN_VALUE3"].IfNullIsZero());
                    model.MAIN_VALUE4 = double.Parse(row["MAIN_VALUE4"].IfNullIsZero());
                    model.MAIN_VALUE5 = double.Parse(row["MAIN_VALUE5"].IfNullIsZero());
                    model.MAIN_VALUE6 = double.Parse(row["MAIN_VALUE6"].IfNullIsZero());
                    model.MAIN_VALUE7 = double.Parse(row["MAIN_VALUE7"].IfNullIsZero());
                    model.MAIN_VALUE8 = double.Parse(row["MAIN_VALUE8"].IfNullIsZero());
                    model.MAIN_VALUE9 = double.Parse(row["MAIN_VALUE9"].IfNullIsZero());
                    model.MAIN_VALUE10 = double.Parse(row["MAIN_VALUE10"].IfNullIsZero());
                    model.MAIN_VALUE11 = double.Parse(row["MAIN_VALUE11"].IfNullIsZero());
                    model.MAIN_VALUE12 = double.Parse(row["MAIN_VALUE12"].IfNullIsZero());
                    model.MAIN_VALUE13 = double.Parse(row["MAIN_VALUE13"].IfNullIsZero());
                    model.MAIN_VALUE14 = double.Parse(row["MAIN_VALUE14"].IfNullIsZero());
                    model.MAIN_VALUE15 = double.Parse(row["MAIN_VALUE15"].IfNullIsZero());
                    model.MAIN_VALUE16 = double.Parse(row["MAIN_VALUE16"].IfNullIsZero());
                    model.MAIN_VALUE17 = double.Parse(row["MAIN_VALUE17"].IfNullIsZero());
                    model.MAIN_VALUE18 = double.Parse(row["MAIN_VALUE18"].IfNullIsZero());
                    model.MAIN_VALUE19 = double.Parse(row["MAIN_VALUE19"].IfNullIsZero());
                    model.MAIN_VALUE20 = double.Parse(row["MAIN_VALUE20"].IfNullIsZero());
                    model.MAIN_VALUE21 = double.Parse(row["MAIN_VALUE21"].IfNullIsZero());
                    model.MAIN_VALUE22 = double.Parse(row["MAIN_VALUE22"].IfNullIsZero());
                    model.MAIN_VALUE23 = double.Parse(row["MAIN_VALUE23"].IfNullIsZero());
                    model.MAIN_VALUE24 = double.Parse(row["MAIN_VALUE24"].IfNullIsZero());
                    model.MAIN_VALUE25 = double.Parse(row["MAIN_VALUE25"].IfNullIsZero());
                    model.MAIN_VALUE26 = double.Parse(row["MAIN_VALUE26"].IfNullIsZero());
                    model.MAIN_VALUE27 = double.Parse(row["MAIN_VALUE27"].IfNullIsZero());
                    model.MAIN_VALUE28 = double.Parse(row["MAIN_VALUE28"].IfNullIsZero());
                    model.MAIN_VALUE29 = double.Parse(row["MAIN_VALUE29"].IfNullIsZero());
                    model.MAIN_VALUE30 = double.Parse(row["MAIN_VALUE30"].IfNullIsZero());
                    model.MAIN_MAX_VALUE = double.Parse(row["MAIN_MAX_VALUE"].IfNullIsZero());
                    model.MAIN_MIN_VALUE = double.Parse(row["MAIN_MIN_VALUE"].IfNullIsZero());
                    model.MAIN_AVG_VALUE = double.Parse(row["MAIN_AVG_VALUE"].IfNullIsZero());
                    model.MAIN_RANGE = double.Parse(row["MAIN_RANGE"].IfNullIsZero());
                    model.MAIN_JUDGE_FLAG = row["MAIN_JUDGE_FLAG"].NullString();
                    model.LWL = double.Parse(row["LWL"].IfNullIsZero());
                    model.UWL = double.Parse(row["UWL"].IfNullIsZero());
                    chartModelsErr.Add(model);
                }
            }

            result.lstData = chartModels;
            result.lstDataErr = chartModelsErr;

            return result;
        }

        // CTQ
        public List<CTQSettingViewModel> GetCTQ()
        {
            return _mapper.Map<List<CTQSettingViewModel>>(_CTQ_SettingRepository.FindAll());
        }

        public CTQSettingViewModel PutCTQ(CTQSettingViewModel ctq)
        {
            CTQ_SETTING en = _mapper.Map<CTQ_SETTING>(ctq);
            _CTQ_SettingRepository.Update(en);
            return ctq;
        }

        public CTQSettingViewModel PostCTQ(CTQSettingViewModel ctq)
        {
            CTQ_SETTING en = _mapper.Map<CTQ_SETTING>(ctq);
            _CTQ_SettingRepository.Add(en);
            return ctq;
        }

        public CTQSettingViewModel DeleteCTQ(CTQSettingViewModel ctq)
        {
            CTQ_SETTING en = _mapper.Map<CTQ_SETTING>(ctq);
            _CTQ_SettingRepository.Remove(en);
            return ctq;
        }

        public CTQSettingViewModel GetCTQ_Id(int Id)
        {
            return _mapper.Map<CTQSettingViewModel>(_CTQ_SettingRepository.FindById(Id));
        }
    }
}
