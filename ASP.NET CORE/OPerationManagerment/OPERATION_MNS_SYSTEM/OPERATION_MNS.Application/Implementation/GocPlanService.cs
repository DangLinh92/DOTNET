using AutoMapper;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OPERATION_MNS.Application.Implementation
{
    public class GocPlanService : BaseService, IGocPlanService
    {
        private IRespository<GOC_PLAN_LFEM, int> _GocPlanLfemRepository;
        private IRespository<GOC_PLAN_SMT, int> _GocPlanSMTRepository;
        private IRespository<GOC_PLAN, int> _GocPlanRepository;
        private IRespository<GOC_PLAN_WLP2, int> _GocPlanWLP2Repository;
        private IRespository<FAB_PLAN, int> _FABPlanRepository;
        private IRespository<GOC_STANDAR_QTY, int> _GocStandarQtyRepository;

        private IRespository<CTQ_SETTING, int> _CTQ_SettingRepository;
        private IRespository<CTQ_SETTING_WLP2, int> _CTQ_WLP2_SettingRepository;
        private IRespository<DATE_OFF_LINE, int> _DateOffLineRepository;
        private IRespository<DATE_OFF_LINE_LFEM, int> _DateOfflfemLineRepository;
        private IRespository<DATE_OFF_LINE_SMT, int> _DateOffLineSMTRepository;
        private IRespository<LEAD_TIME_WLP, int> _LeadTimeRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GocPlanService(IRespository<GOC_PLAN, int> GocPlanRepository,
                              IRespository<CTQ_SETTING, int> CTQ_SettingRepository,
                              IRespository<GOC_STANDAR_QTY, int> GocStandarQtyRepository,
                              IRespository<DATE_OFF_LINE, int> DateOffLineRepository,
                              IRespository<FAB_PLAN, int> FABPlanRepository,
                              IRespository<LEAD_TIME_WLP, int> LeadTimeRepository,
                              IRespository<GOC_PLAN_WLP2, int> GocPlanWLP2Repository,
                              IRespository<CTQ_SETTING_WLP2, int> CTQ_WLP2_SettingRepository,
                              IRespository<GOC_PLAN_SMT, int> GocPlanSMTRepository,
                              IRespository<DATE_OFF_LINE_SMT, int> DateOffLineSMTRepository,
                              IUnitOfWork unitOfWork, IMapper mapper,
                              IHttpContextAccessor httpContextAccessor, IRespository<GOC_PLAN_LFEM, int> gocPlanLfemRepository, IRespository<DATE_OFF_LINE_LFEM, int> dateOffLineRepository)
        {
            _LeadTimeRepository = LeadTimeRepository;
            _GocPlanRepository = GocPlanRepository;
            _FABPlanRepository = FABPlanRepository;
            _GocStandarQtyRepository = GocStandarQtyRepository;
            _CTQ_SettingRepository = CTQ_SettingRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _DateOffLineRepository = DateOffLineRepository;
            _GocPlanWLP2Repository = GocPlanWLP2Repository;
            _CTQ_WLP2_SettingRepository = CTQ_WLP2_SettingRepository;
            _GocPlanLfemRepository = gocPlanLfemRepository;
            this._DateOfflfemLineRepository = dateOffLineRepository;
            _GocPlanSMTRepository = GocPlanSMTRepository;
            _DateOffLineSMTRepository = DateOffLineSMTRepository;
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
                                Model = chipSheet.Cells[i, 2].Text.NullString(),// SAP CODE
                                Material = chipSheet.Cells[i, 1].Text.NullString(),
                                Division = chipSheet.Cells[i, 5].Text.NullString(),
                                StandardQtyForMonth = float.TryParse(chipSheet.Cells[i, 4].Value.IfNullIsZero(), out _) ? float.Parse(chipSheet.Cells[i, 4].Value.IfNullIsZero()) : 0,
                                UserCreated = GetUserId(),
                                Unit = CommonConstants.CHIP,
                                Department = CommonConstants.WLP1,
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
                                Department = CommonConstants.WLP1,
                                DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                            };
                            lstChipStandarQty.Add(standarQtyEN);
                            lstChipStandarQty.Add(standarQtyWfer);
                        }

                        GOC_STANDAR_QTY en;
                        foreach (var item in lstChipStandarQty)
                        {
                            en = _GocStandarQtyRepository.FindSingle(x => x.Model == item.Model && x.Department == CommonConstants.WLP1);
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
                        //DATE_OFF_LINE dateOff2;
                        List<DATE_OFF_LINE> lstDateOff = new List<DATE_OFF_LINE>();
                        //List<DATE_OFF_LINE> lstDateOffWlp2 = new List<DATE_OFF_LINE>();
                        List<string> dayOff = new List<string>();
                        foreach (KeyValuePair<string, string> item in DayNoPlan)
                        {
                            if (item.Value == "Y")
                            {
                                dateOff = new DATE_OFF_LINE()
                                {
                                    ItemValue = item.Key,
                                    ON_OFF = CommonConstants.OFF,
                                    WLP = CommonConstants.WLP1,
                                    UserCreated = GetUserId(),
                                    OWNER = CommonConstants.WLP1,
                                    DanhMuc = ""
                                };

                                //dateOff2 = new DATE_OFF_LINE()
                                //{
                                //    ItemValue = item.Key,
                                //    ON_OFF = CommonConstants.OFF,
                                //    WLP = CommonConstants.WLP2,
                                //    UserCreated = GetUserId(),
                                //    OWNER = CommonConstants.WLP1,
                                //    DanhMuc = ""
                                //};

                                lstDateOff.Add(dateOff);
                                //lstDateOffWlp2.Add(dateOff2);
                            }
                        }

                        var lstOff = _DateOffLineRepository.FindAll(x => x.ItemValue.CompareTo(beginDate) >= 0 && x.ItemValue.CompareTo(EndDate) <= 0 && x.OWNER.Contains("WLP1")).ToList();
                        _DateOffLineRepository.RemoveMultiple(lstOff);
                        _DateOffLineRepository.AddRange(lstDateOff);
                        //_DateOffLineRepository.AddRange(lstDateOffWlp2);

                        //dayOff = lstDateOff.Select(x => x.ItemValue.Replace("-", "")).ToList();

                        //TODO: THÊM DK CHO wlp = WLP1,WLP2 nếu k cùng date off
                        //var leadTimes = _LeadTimeRepository.FindAll(x => x.WorkDate.CompareTo(beginDate.Replace("-", "")) >= 0 && x.WorkDate.CompareTo(EndDate.Replace("-", "")) <= 0 && x.WLP.Contains("WLP"));

                        //foreach (var item in leadTimes)
                        //{
                        //    if (dayOff.Contains(item.WorkDate))
                        //    {
                        //        item.Ox = "X";
                        //    }
                        //    else
                        //    {
                        //        item.Ox = "";
                        //    }

                        //    _LeadTimeRepository.Update(item);
                        //}

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

        public List<GocPlanViewModelEx> GetByTime(string unit, string fromDate, string toDate, string wlp = "WLP1", string danhmuc = "")
        {
            List<GocPlanViewModelEx> lstResult = new List<GocPlanViewModelEx>();

            if (wlp == CommonConstants.WLP1)
            {
                var lstStandar = _GocStandarQtyRepository.FindAll(x => x.Unit == unit && x.Department == wlp);

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
                    plan.Total_Gap = -plan.Total_Plan + plan.Total_Actual;//unit == CommonConstants.CHIP ? (float)Math.Round(totalGap / 1000, 0) : (float)Math.Round(totalGap, 0);

                    lstResult.Add(plan);
                }
            }
            else if (wlp == CommonConstants.WLP2)
            {
                GocPlanViewModelEx plan = null;
                var lstQty = _GocPlanWLP2Repository.FindAll(x => x.DanhMuc == danhmuc && x.Unit == CommonConstants.CHIP && x.DatePlan.CompareTo(fromDate) >= 0 && x.DatePlan.CompareTo(toDate) <= 0);
                var lstStandar = _GocStandarQtyRepository.FindAll(x => x.Unit == CommonConstants.CHIP && x.Department == CommonConstants.WLP2);

                List<string> lstModel = lstQty.Select(x => x.Model).Distinct().ToList();
                List<GOC_PLAN_WLP2> gocModels;
                float totalPlan = 0;
                float totalActual = 0;
                float totalGap = 0;
                int number = 0;
                float standarQty = 0;
                foreach (var item in lstModel)
                {
                    gocModels = lstQty.Where(x => x.Model == item).ToList();

                    totalPlan = 0;
                    totalActual = 0;
                    totalGap = 0;
                    number = 0;

                    foreach (var qty in gocModels.ToList())
                    {
                        if (number++ == 0)
                        {
                            standarQty = lstStandar.FirstOrDefault(x => x.Model == item) != null ? lstStandar.FirstOrDefault(x => x.Model == item).StandardQtyForMonth : 1;
                            plan = new GocPlanViewModelEx()
                            {
                                Id = qty.Id,
                                Module = qty.Module,
                                Model = qty.Model,
                                Material = qty.Material,
                                Division = qty.Division,
                                DanhMuc = qty.DanhMuc,
                                StandardQtyForMonth = standarQty,
                                Unit = unit,
                                Type = qty.Type
                            };
                        }

                        plan.QuantityByDays.Add(new QuantityByDay()
                        {
                            Id = qty.Id,
                            QuantityPlan = qty.QuantityPlan,
                            QuantityActual = qty.QuantityActual,
                            QuantityGap = qty.QuantityGap,
                            DatePlan = qty.DatePlan
                        });

                        totalPlan += qty.QuantityPlan;
                        totalActual += qty.QuantityActual;
                        totalGap += qty.QuantityGap;
                    }

                    if (plan != null)
                    {
                        plan.Total_Plan = unit == CommonConstants.CHIP ? (float)Math.Round(totalPlan / 1000, 0) : (float)Math.Round(totalPlan, 0);
                        plan.Total_Actual = unit == CommonConstants.CHIP ? (float)Math.Round(totalActual / 1000, 0) : (float)Math.Round(totalActual, 0);
                        plan.Total_Gap = -plan.Total_Plan + plan.Total_Actual;

                        lstResult.Add(plan);
                    }
                }
            }
            else if (wlp == CommonConstants.LFEM)
            {
                GocPlanViewModelEx plan = null;
                var lstQty = _GocPlanLfemRepository.FindAll(x => x.DanhMuc == danhmuc && x.DatePlan.CompareTo(fromDate) >= 0 && x.DatePlan.CompareTo(toDate) <= 0);

                List<string> lstModel = lstQty.Select(x => x.MesItemId).Distinct().ToList();
                List<GOC_PLAN_LFEM> gocModels;
                double totalPlan = 0;
                double totalActual = 0;
                double totalGap = 0;
                int number = 0;
                foreach (var item in lstModel)
                {
                    gocModels = lstQty.Where(x => x.MesItemId == item).ToList();

                    totalPlan = 0;
                    totalActual = 0;
                    totalGap = 0;
                    number = 0;

                    foreach (var qty in gocModels.ToList())
                    {
                        if (number++ == 0)
                        {
                            plan = new GocPlanViewModelEx()
                            {
                                Id = qty.Id,
                                Model = qty.MesItemId,
                                Material = qty.MasterialName,
                                Size = qty.Size,
                                DanhMuc = qty.DanhMuc,
                                GroupName = qty.Group2
                            };
                        }

                        plan.QuantityByDays.Add(new QuantityByDay()
                        {
                            Id = qty.Id,
                            QuantityPlan = (float)qty.QuantityPlan,
                            QuantityActual = (float)qty.QuantityActual,
                            QuantityGap = (float)qty.QuantityGap,
                            DatePlan = qty.DatePlan
                        });

                        totalPlan += qty.QuantityPlan;
                        totalActual += qty.QuantityActual;
                        totalGap += qty.QuantityGap;
                    }

                    if (plan != null)
                    {
                        plan.Total_Plan = (float)Math.Round(totalPlan, 0);
                        plan.Total_Actual = (float)Math.Round(totalActual, 0);
                        plan.Total_Gap = -plan.Total_Plan + plan.Total_Actual;

                        lstResult.Add(plan);
                    }
                }
            }
            else if (wlp == CommonConstants.SMT)
            {
                GocPlanViewModelEx plan = null;
                var lstQty = _GocPlanSMTRepository.FindAll(x => x.DanhMuc == danhmuc && x.DatePlan.CompareTo(fromDate) >= 0 && x.DatePlan.CompareTo(toDate) <= 0);

                List<string> lstModel = lstQty.Select(x => x.MesItemId).Distinct().ToList();
                List<GOC_PLAN_SMT> gocModels;
                double totalPlan = 0;
                double totalActual = 0;
                double totalGap = 0;
                int number = 0;
                foreach (var item in lstModel)
                {
                    gocModels = lstQty.Where(x => x.MesItemId == item).ToList();

                    totalPlan = 0;
                    totalActual = 0;
                    totalGap = 0;
                    number = 0;

                    foreach (var qty in gocModels.ToList())
                    {
                        if (number++ == 0)
                        {
                            plan = new GocPlanViewModelEx()
                            {
                                Id = qty.Id,
                                Model = qty.MesItemId,
                                Material = qty.MasterialName,
                                Size = qty.Size,
                                DanhMuc = qty.DanhMuc,
                                GroupName = qty.Group2
                            };
                        }

                        plan.QuantityByDays.Add(new QuantityByDay()
                        {
                            Id = qty.Id,
                            QuantityPlan = (float)qty.QuantityPlan,
                            QuantityActual = (float)qty.QuantityActual,
                            QuantityGap = (float)qty.QuantityGap,
                            DatePlan = qty.DatePlan
                        });

                        totalPlan += qty.QuantityPlan;
                        totalActual += qty.QuantityActual;
                        totalGap += qty.QuantityGap;
                    }

                    if (plan != null)
                    {
                        plan.Total_Plan = (float)Math.Round(totalPlan, 0);
                        plan.Total_Actual = (float)Math.Round(totalActual, 0);
                        plan.Total_Gap = -plan.Total_Plan + plan.Total_Actual;

                        lstResult.Add(plan);
                    }
                }
            }
            return lstResult.OrderBy(x => x.Model).ToList();
        }

        public List<GocPlanViewModelEx> GetByTime_fab(string unit, string fromDate, string toDate)
        {
            List<GocPlanViewModelEx> lstResult = new List<GocPlanViewModelEx>();

            var lstStandar = _GocStandarQtyRepository.FindAll(x => x.Unit == unit && x.Department == CommonConstants.WLP1);

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

        public void DeleteGocModel(int Id, string fromDate, string toDate, string wlp)
        {
            if (wlp == CommonConstants.WLP1)
            {
                GOC_STANDAR_QTY en = _GocStandarQtyRepository.FindById(Id);
                string model = en.Model;
                var lst = _GocPlanRepository.FindAll(x => x.Model == model && x.Department == wlp && x.DatePlan.CompareTo(fromDate) >= 0 && x.DatePlan.CompareTo(toDate) <= 0).ToList();
                _GocPlanRepository.RemoveMultiple(lst);
                _GocStandarQtyRepository.Remove(Id);
                _unitOfWork.Commit();
            }
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

            var lstChips = UpdateDataLost(lstGroupChip, dayOfMonth, 1, CommonConstants.Day);

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

            var lstWafer = UpdateDataLost(lstGroupWafer, dayOfMonth, 1, CommonConstants.Day);

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

            var lstFABWafer = UpdateDataLost(lstGroupfabWafer, dayOfMonth, 1, CommonConstants.Day);

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

        private List<QuantityByDay> UpdateDataLost(List<QuantityByDay> data, int dayOfMonth, int begin, string weekDay)
        {
            if (weekDay == CommonConstants.Day)
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
            else if (weekDay == CommonConstants.Week)
            {
                if (data.Count == 0) return data;

                int numWeek = 0;
                int numW = dayOfMonth - begin + 1;
                for (int i = 0; i < numW; i++)
                {
                    numWeek = begin + i;

                    if (data.Count >= i + 1)
                    {
                        if (int.Parse(data[i].DatePlan.Substring(1)) != numWeek)
                        {
                            data.Insert(i, new QuantityByDay()
                            {
                                DatePlan = "W" + numWeek,
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
                            DatePlan = "W" + numWeek,
                            QuantityPlan = 0,
                            QuantityActual = 0,
                            QuantityGap = 0
                        });
                    }
                }

                return data.OrderBy(x => x.DatePlan).ToList();
            }

            return data;
        }

        [Obsolete]
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
            List<double> AvgItem = new List<double>();
            List<double> UslItem = new List<double>();
            List<double> LslItem = new List<double>();

            ResultDB rs = _GocPlanRepository.ExecProceduce2("PKG_BUSINESS@VIEW_CONTROL_CHART_DATA", dic);
            if (rs.ReturnInt == 0)
            {
                DataTable tbl = rs.ReturnDataSet.Tables[0];
                DataTable tbl_Err = rs.ReturnDataSet.Tables[1];
                DataTable tbl_MatertialId = rs.ReturnDataSet.Tables[2];



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

                    AvgItem.Add(model.MAIN_AVG_VALUE);
                    UslItem.Add(model.MAIN_TARGET_USL);
                    LslItem.Add(model.MAIN_TARGET_LSL);
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

                foreach (DataRow row in tbl_MatertialId.Rows)
                {
                    result.lstMaterialId.Add(row["MATERIAL_ID"].NullString());
                }
            }

            result.lstData = chartModels;
            result.lstDataErr = chartModelsErr;

            double stdev = AvgItem.Count > 0 ? AvgItem.StdDev(false) : 0;

            result.STDEV = Math.Round(stdev, 1);

            if ((operation == "OP50000" || operation == "OP69000") && AvgItem.Count > 0 && LslItem.Count > 0 && stdev != 0)
            {
                result.CPK_bst = Math.Round((AvgItem.Average() - LslItem.Average()) / (3 * stdev), 1);
            }

            if ((operation == "OP37000" || operation == "OP49000" || operation == "OP57000" || operation == "OP65000" || operation == "OP64000")
                && UslItem.Count > 0 && AvgItem.Count > 0 && LslItem.Count > 0 && stdev != 0)
            {
                result.CPK_Thickness = Math.Round(Math.MinMagnitude((UslItem.Average() - AvgItem.Average()) / (3 * stdev), (AvgItem.Average() - LslItem.Average()) / (3 * stdev)), 1);
            }

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

        public List<string> DateOffLine(string year, string owner, string wlp, string danhmuc = "")
        {
            if (wlp.Contains("WLP"))
            {
                if (danhmuc == "")
                {
                    return _DateOffLineRepository.FindAll(x => x.ON_OFF == "OFF" && x.WLP == wlp && x.OWNER == owner && x.ItemValue.StartsWith(year)).Select(x => x.ItemValue).ToList();
                }
                else
                {
                    return _DateOffLineRepository.FindAll(x => x.ON_OFF == "OFF" && x.WLP == wlp && x.OWNER == owner && x.ItemValue.StartsWith(year) && x.DanhMuc == danhmuc).Select(x => x.ItemValue).ToList();
                }
            }
            else if (wlp == CommonConstants.LFEM)
            {
                return _DateOfflfemLineRepository.FindAll(x => x.ItemValue.StartsWith(year) && x.DanhMuc == danhmuc).Select(x => x.ItemValue).ToList();
            }
            else if (wlp == CommonConstants.SMT)
            {
                return _DateOffLineSMTRepository.FindAll(x => x.ItemValue.StartsWith(year) && x.DanhMuc == danhmuc).Select(x => x.ItemValue).ToList();
            }

            return new List<string>();
        }

        public ResultDB ImportExcel_Wlp2(string filePath, string param)
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
                                Model = chipSheet.Cells[i, 2].Text.NullString(),// SAP CODE
                                Material = chipSheet.Cells[i, 1].Text.NullString(),
                                Division = chipSheet.Cells[i, 5].Text.NullString(),
                                StandardQtyForMonth = float.TryParse(chipSheet.Cells[i, 4].Value.IfNullIsZero(), out _) ? float.Parse(chipSheet.Cells[i, 4].Value.IfNullIsZero()) : 0,
                                UserCreated = GetUserId(),
                                Unit = CommonConstants.CHIP,
                                Department = CommonConstants.WLP2,
                                DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                            };
                            lstChipStandarQty.Add(standarQtyEN);
                        }

                        GOC_STANDAR_QTY en;
                        foreach (var item in lstChipStandarQty)
                        {
                            en = _GocStandarQtyRepository.FindSingle(x => x.Model == item.Model && x.Department == CommonConstants.WLP2);
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
                    string danhmuc = "";

                    using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                    {
                        ExcelWorksheet chipSheet = packet.Workbook.Worksheets[1];
                        List<GOC_PLAN_WLP2> lstChipPlanQty = new List<GOC_PLAN_WLP2>();
                        Dictionary<string, string> DayNoPlan = new Dictionary<string, string>();
                        string Sapcode = "";
                        // import chip
                        for (int i = chipSheet.Dimension.Start.Row + 3; i <= chipSheet.Dimension.End.Row; i++)
                        {
                            if (chipSheet.Cells[i, 2].Text.NullString() == "")
                            {
                                break;
                            }

                            Sapcode = chipSheet.Cells[i, 2].Text.NullString();

                            if (Sapcode.ToLower() == "sample")
                            {
                                Sapcode = "Sample";
                            }

                            if (chipSheet.Cells[i, 1].Text.NullString() == CommonConstants.NHAP_KHO)
                            {
                                danhmuc = CommonConstants.NHAP_KHO;
                            }
                            else if (chipSheet.Cells[i, 1].Text.NullString() == CommonConstants.SAN_XUAT)
                            {
                                danhmuc = CommonConstants.SAN_XUAT;
                            }
                            else if (chipSheet.Cells[i, 1].Text.NullString() == CommonConstants.XUAT_SMT)
                            {
                                danhmuc = CommonConstants.XUAT_SMT;
                            }

                            if (danhmuc.NullString() == "")
                            {
                                break;
                            }

                            for (int k = 7; k <= 60; k++)
                            {
                                if (!DateTime.TryParse(chipSheet.Cells[3, 7].Text.NullString(), out DateTime date) || !chipSheet.Cells[3, 7].Text.NullString().Contains("-") || chipSheet.Cells[3, 7].Text.NullString().Contains("2021"))
                                {
                                    resultDB.ReturnInt = -1;
                                    resultDB.ReturnString = "Format ngày tháng theo định dạng yyyy-MM-dd và ngày tháng phải >= 2023";
                                    return resultDB;
                                }

                                if (!DateTime.TryParse(chipSheet.Cells[3, k].Text.NullString(), out _))
                                {
                                    break;
                                }

                                if (k == 7)
                                {
                                    beginDate = DateTime.Parse(chipSheet.Cells[3, k].Text.NullString()).ToString("yyyy-MM-dd");
                                    EndDate = DateTime.Parse(beginDate).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                                }

                                GOC_PLAN_WLP2 standarQtyEN = new GOC_PLAN_WLP2()
                                {
                                    DanhMuc = danhmuc,
                                    Model = Sapcode, // Sap code
                                    Module = chipSheet.Cells[i, 3].Text.NullString(),
                                    Type = chipSheet.Cells[i, 4].Text.NullString(),
                                    Division = chipSheet.Cells[i, 5].Text.NullString(),

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

                        // xoa item đã có sẵn, add item mới
                        GOC_PLAN_WLP2 en;
                        foreach (var item in lstChipPlanQty.ToList())
                        {
                            en = _GocPlanWLP2Repository.FindSingle(x => x.Model == item.Model && x.DatePlan == item.DatePlan && x.Unit == item.Unit && x.DanhMuc == item.DanhMuc);
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
                                en.Type = item.Type;
                                en.DanhMuc = item.DanhMuc;

                                _GocPlanWLP2Repository.Update(en);

                                lstChipPlanQty.Remove(item);
                            }
                        }

                        if (lstChipPlanQty.Count > 0)
                        {
                            _GocPlanWLP2Repository.AddRange(lstChipPlanQty);
                        }

                        // lưu ngày không kế hoạch 
                        DATE_OFF_LINE dateOff;
                        DATE_OFF_LINE dateOff2;
                        List<DATE_OFF_LINE> lstDateOff = new List<DATE_OFF_LINE>();
                        List<DATE_OFF_LINE> lstDateOffWlp2 = new List<DATE_OFF_LINE>();
                        List<string> dayOff = new List<string>();
                        foreach (KeyValuePair<string, string> item in DayNoPlan)
                        {
                            if (item.Value == "Y")
                            {
                                dateOff = new DATE_OFF_LINE()
                                {
                                    ItemValue = item.Key,
                                    ON_OFF = CommonConstants.OFF,
                                    WLP = CommonConstants.WLP2,
                                    UserCreated = GetUserId(),
                                    OWNER = CommonConstants.WLP2,
                                    DanhMuc = danhmuc
                                };

                                dateOff2 = new DATE_OFF_LINE()
                                {
                                    ItemValue = item.Key,
                                    ON_OFF = CommonConstants.OFF,
                                    WLP = CommonConstants.WLP2,
                                    UserCreated = GetUserId(),
                                    OWNER = CommonConstants.WLP1,
                                    DanhMuc = ""
                                };

                                lstDateOff.Add(dateOff);
                                lstDateOffWlp2.Add(dateOff2);
                            }
                        }

                        var lstOff = _DateOffLineRepository.FindAll(x => x.ItemValue.CompareTo(beginDate) >= 0 && x.ItemValue.CompareTo(EndDate) <= 0 && x.OWNER.Contains("WLP2") && x.DanhMuc == danhmuc).ToList();
                        var lstOffWlp12 = _DateOffLineRepository.FindAll(x => x.ItemValue.CompareTo(beginDate) >= 0 && x.ItemValue.CompareTo(EndDate) <= 0 && x.OWNER.Contains("WLP1") && x.WLP == "WLP2").ToList();

                        _DateOffLineRepository.RemoveMultiple(lstOff);
                        _DateOffLineRepository.RemoveMultiple(lstOffWlp12);

                        _DateOffLineRepository.AddRange(lstDateOff);
                        _DateOffLineRepository.AddRange(lstDateOffWlp2);

                        //dayOff = lstDateOff.Select(x => x.ItemValue.Replace("-", "")).ToList();

                        ////TODO: THÊM DK CHO wlp = WLP1,WLP2 nếu k cùng date off
                        //var leadTimes = _LeadTimeRepository.FindAll(x => x.WorkDate.CompareTo(beginDate.Replace("-", "")) >= 0 && x.WorkDate.CompareTo(EndDate.Replace("-", "")) <= 0 && x.WLP.Contains("WLP"));

                        //foreach (var item in leadTimes)
                        //{
                        //    if (dayOff.Contains(item.WorkDate))
                        //    {
                        //        item.Ox = "X";
                        //    }
                        //    else
                        //    {
                        //        item.Ox = "";
                        //    }

                        //    _LeadTimeRepository.Update(item);
                        //}

                        Save();

                        if (DateTime.Now.ToString("yyyy-MM") + "-01" == beginDate)
                        {
                            Dictionary<string, string> dic = new Dictionary<string, string>();
                            dic.Add("A_BEGIN_MONTH", DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd"));

                            // LẤY LẠI SẢN LƯỢNG NHẬP TỪ WLP1 THỰC TẾ trong thang
                            var rs = _GocPlanWLP2Repository.ExecProceduce2("GET_QTY_ACTUAL_CHIP_BY_DAY_MANUAL_WLP2", dic);
                        }

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

        public void DeleteGocModelWlp2(string model, string fromDate, string toDate, string danhmuc)
        {
            var lst = _GocPlanWLP2Repository.FindAll(x => x.Model == model && x.DanhMuc == danhmuc && x.DatePlan.CompareTo(fromDate) >= 0 && x.DatePlan.CompareTo(toDate) <= 0).ToList();
            _GocPlanWLP2Repository.RemoveMultiple(lst);
            _unitOfWork.Commit();
        }

        public List<ProcActualPlanModel> GetProcActualPlanWlp2Model(string month, string danhmuc)
        {
            int dayOfMonth = DateTime.Parse(month + "-01").AddMonths(1).AddDays(-1).Day;

            List<ProcActualPlanModel> result = new List<ProcActualPlanModel>();
            var lstGoc = _GocPlanWLP2Repository.FindAll(x => x.DatePlan.Contains(month) && x.DanhMuc == danhmuc && x.Unit == CommonConstants.CHIP);

            // CHIP
            var lstGroupChip = (from p in lstGoc.AsEnumerable()
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

            var lstChips = UpdateDataLost(lstGroupChip, dayOfMonth, 1, CommonConstants.Day);

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

            ProcActualPlanModel proc = new ProcActualPlanModel();
            proc.QuantityByDays = lstChips;
            proc.Month = month;
            proc.DayOfMonth = dayOfMonth;
            proc.CFAB = CommonConstants.CHIP;
            proc.DanhMuc = danhmuc;
            result.Add(proc);

            return result;
        }

        public List<GocPlanViewModel> GetDataByDay(string dayFrom, string dayTo, string danhmuc)
        {
            return _mapper.Map<List<GocPlanViewModel>>(_GocPlanWLP2Repository.FindAll(x => x.DanhMuc == danhmuc && x.Unit == CommonConstants.CHIP && x.DatePlan.CompareTo(dayFrom) >= 0 && x.DatePlan.CompareTo(dayTo) <= 0));
        }

        public ViewControlChartDataModel GetDataControlChartWLP2(string date, string toDate, string operation, string mattertial)
        {
            ViewControlChartDataModel result = new ViewControlChartDataModel();
            List<ViewControlChartModel> chartModels = new List<ViewControlChartModel>();
            List<ViewControlChartModel> chartModelsErr = new List<ViewControlChartModel>();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("A_OPERATION", operation);
            dic.Add("A_FROM_DATE", date);
            dic.Add("A_TO_DATE", toDate);
            dic.Add("A_MATERTIAL", mattertial);

            List<double> AvgItem = new List<double>();
            List<double> UslItem = new List<double>();
            List<double> LslItem = new List<double>();

            List<double> AvgItem_150 = new List<double>();
            List<double> UslItem_150 = new List<double>();
            List<double> LslItem_150 = new List<double>();

            List<double> AvgItem_200 = new List<double>();
            List<double> UslItem_200 = new List<double>();
            List<double> LslItem_200 = new List<double>();

            ResultDB rs = _GocPlanRepository.ExecProceduce2("PKG_BUSINESS@VIEW_CONTROL_CHART_DATA_WLP2", dic);
            if (rs.ReturnInt == 0)
            {
                DataTable tbl = rs.ReturnDataSet.Tables[0];
                DataTable tbl_Err = rs.ReturnDataSet.Tables[1];
                DataTable tbl_MatertialId = rs.ReturnDataSet.Tables[2];

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
                    model.Thicknet = double.Parse(row["ThickNet"].IfNullIsZero());

                    chartModels.Add(model);

                    if (model.Thicknet == 130)
                    {
                        AvgItem.Add(model.MAIN_AVG_VALUE);
                        UslItem.Add(model.MAIN_TARGET_USL);
                        LslItem.Add(model.MAIN_TARGET_LSL);
                    }
                    else if (model.Thicknet == 150)
                    {
                        AvgItem_150.Add(model.MAIN_AVG_VALUE);
                        UslItem_150.Add(model.MAIN_TARGET_USL);
                        LslItem_150.Add(model.MAIN_TARGET_LSL);
                    }
                    else if (model.Thicknet == 200)
                    {
                        AvgItem_200.Add(model.MAIN_AVG_VALUE);
                        UslItem_200.Add(model.MAIN_TARGET_USL);
                        LslItem_200.Add(model.MAIN_TARGET_LSL);
                    }
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
                    model.Thicknet = double.Parse(row["ThickNet"].IfNullIsZero());
                    chartModelsErr.Add(model);
                }

                foreach (DataRow row in tbl_MatertialId.Rows)
                {
                    result.lstMaterialId.Add(row["MATERIAL_ID"].NullString());
                }
            }

            result.lstData = chartModels;
            result.lstDataErr = chartModelsErr;

            // 130um
            double stdev = AvgItem.Count > 0 ? AvgItem.StdDev(false) : 0;

            result.STDEV = Math.Round(stdev, 1);

            if (AvgItem.Count > 0 && LslItem.Count > 0 && stdev != 0)
            {
                result.CPK_bst = Math.Round((AvgItem.Average() - LslItem.Average()) / (3 * stdev), 1);
            }

            if (UslItem.Count > 0 && AvgItem.Count > 0 && LslItem.Count > 0 && stdev != 0)
            {
                result.CPK_Thickness = Math.Round(Math.MinMagnitude((UslItem.Average() - AvgItem.Average()) / (3 * stdev), (AvgItem.Average() - LslItem.Average()) / (3 * stdev)), 1);
            }

            // 150um
            double stdev150 = AvgItem_150.Count > 0 ? AvgItem_150.StdDev(false) : 0;

            result.STDEV150 = Math.Round(stdev150, 1);

            if (AvgItem_150.Count > 0 && LslItem_150.Count > 0 && stdev150 != 0)
            {
                result.CPK_bst150 = Math.Round((AvgItem_150.Average() - LslItem_150.Average()) / (3 * stdev150), 1);
            }

            if (UslItem_150.Count > 0 && AvgItem_150.Count > 0 && LslItem_150.Count > 0 && stdev150 != 0)
            {
                result.CPK_Thickness150 = Math.Round(Math.MinMagnitude((UslItem_150.Average() - AvgItem_150.Average()) / (3 * stdev150), (AvgItem_150.Average() - LslItem_150.Average()) / (3 * stdev150)), 1);
            }

            // 200um
            double stdev200 = AvgItem_200.Count > 0 ? AvgItem_200.StdDev(false) : 0;

            result.STDEV200 = Math.Round(stdev200, 1);

            if (AvgItem_200.Count > 0 && LslItem_200.Count > 0 && stdev200 != 0)
            {
                result.CPK_bst200 = Math.Round((AvgItem_200.Average() - LslItem_200.Average()) / (3 * stdev200), 1);
            }

            if (UslItem_200.Count > 0 && AvgItem_200.Count > 0 && LslItem_200.Count > 0 && stdev200 != 0)
            {
                result.CPK_Thickness200 = Math.Round(Math.MinMagnitude((UslItem_200.Average() - AvgItem_200.Average()) / (3 * stdev200), (AvgItem_200.Average() - LslItem_200.Average()) / (3 * stdev200)), 1);
            }

            return result;
        }

        #region CTQ WLP2
        public CTQSettingWLP2ViewModel PutCTQ_Wlp2(CTQSettingWLP2ViewModel ctq)
        {
            CTQ_SETTING_WLP2 en = _mapper.Map<CTQ_SETTING_WLP2>(ctq);
            _CTQ_WLP2_SettingRepository.Update(en);
            return ctq;
        }

        public CTQSettingWLP2ViewModel PostCTQ_Wlp2(CTQSettingWLP2ViewModel ctq)
        {
            CTQ_SETTING_WLP2 en = _mapper.Map<CTQ_SETTING_WLP2>(ctq);
            _CTQ_WLP2_SettingRepository.Add(en);
            return ctq;
        }

        public CTQSettingWLP2ViewModel DeleteCTQ_Wlp2(CTQSettingWLP2ViewModel ctq)
        {
            CTQ_SETTING_WLP2 en = _mapper.Map<CTQ_SETTING_WLP2>(ctq);
            _CTQ_WLP2_SettingRepository.Remove(en);
            return ctq;
        }

        public CTQSettingWLP2ViewModel GetCTQ_Wlp2_Id(int Id)
        {
            return _mapper.Map<CTQSettingWLP2ViewModel>(_CTQ_WLP2_SettingRepository.FindById(Id));
        }

        public List<CTQSettingWLP2ViewModel> GetCTQ_Wlp2()
        {
            return _mapper.Map<List<CTQSettingWLP2ViewModel>>(_CTQ_WLP2_SettingRepository.FindAll());
        }
        #endregion

        #region LFEM
        public ResultDB ImportExcel_Lfem(string filePath, string param)
        {
            ResultDB resultDB = new ResultDB();

            try
            {
                if (param == CommonConstants.KHSX)
                {
                    using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                    {
                        ExcelWorksheet chipSheet = packet.Workbook.Worksheets[1];
                        List<GOC_PLAN_LFEM> lstGocPlanLfem = new List<GOC_PLAN_LFEM>();
                        List<GOC_PLAN_LFEM> lstGocPlanLfemUpdate = new List<GOC_PLAN_LFEM>();
                        Dictionary<string, string> DayNoPlan = new Dictionary<string, string>();


                        string beginDate = chipSheet.Cells["B1"].Text.NullString();
                        string endDate = "";

                        string _day = "";
                        for (int i = chipSheet.Dimension.Start.Row + 2; i <= chipSheet.Dimension.End.Row; i++)
                        {
                            if (chipSheet.Cells[i, 1].Text.NullString() == "")
                            {
                                break;
                            }

                            for (int k = 0; k < 40; k++)
                            {

                                if (chipSheet.Cells[2, k + 4].Text.NullString() == "" || !DateTime.TryParse(chipSheet.Cells[2, k + 4].Text.NullString(), out _))
                                {
                                    break;
                                }

                                _day = DateTime.Parse(chipSheet.Cells[2, k + 4].Text.NullString()).ToString("yyyy-MM-dd");

                                endDate = _day;

                                GOC_PLAN_LFEM lfemPlan = _GocPlanLfemRepository.FindSingle(x => x.MesItemId == chipSheet.Cells[i, 1].Text.NullString() && x.DatePlan == _day && x.DanhMuc == CommonConstants.KHSX);
                                if (lfemPlan == null)
                                {
                                    lfemPlan = new GOC_PLAN_LFEM()
                                    {
                                        MesItemId = chipSheet.Cells[i, 1].Text.NullString(),
                                        UserCreated = GetUserId(),
                                        DatePlan = _day,
                                        MonthPlan = _day.Substring(0, 7) + "-01",
                                        QuantityPlan = double.Parse(chipSheet.Cells[i, k + 4].Value.IfNullIsZero()),
                                        DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                        DanhMuc = CommonConstants.KHSX,
                                        WeekPlan = "W" + (DateTime.Parse(_day).GetWeekOfYear() + 1)
                                    };
                                    lstGocPlanLfem.Add(lfemPlan);
                                }
                                else
                                {
                                    lfemPlan.UserModified = GetUserId();
                                    lfemPlan.QuantityPlan = double.Parse(chipSheet.Cells[i, k + 4].Value.IfNullIsZero());
                                    lfemPlan.DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    lfemPlan.DanhMuc = CommonConstants.KHSX;
                                    lfemPlan.MonthPlan = _day.Substring(0, 7) + "-01";
                                    lfemPlan.WeekPlan = "W" + (DateTime.Parse(_day).GetWeekOfYear() + 1);
                                    lstGocPlanLfemUpdate.Add(lfemPlan);
                                }

                                if (lfemPlan.QuantityPlan == 0)
                                {
                                    if (!DayNoPlan.ContainsKey(lfemPlan.DatePlan))
                                    {
                                        DayNoPlan.Add(lfemPlan.DatePlan, "Y");
                                    }
                                }
                                else if (lfemPlan.QuantityPlan > 0)
                                {
                                    if (DayNoPlan.ContainsKey(lfemPlan.DatePlan) && DayNoPlan[lfemPlan.DatePlan] == "Y")
                                    {
                                        DayNoPlan[lfemPlan.DatePlan] = "N";
                                    }
                                }
                            }
                        }

                        if (lstGocPlanLfem.Count > 0)
                            _GocPlanLfemRepository.AddRange(lstGocPlanLfem);

                        if (lstGocPlanLfemUpdate.Count > 0)
                            _GocPlanLfemRepository.UpdateRange(lstGocPlanLfemUpdate);

                        // lưu ngày không kế hoạch 
                        DATE_OFF_LINE_LFEM dateOff;

                        List<DATE_OFF_LINE_LFEM> lstDateOffLFEM = new List<DATE_OFF_LINE_LFEM>();
                        foreach (KeyValuePair<string, string> item in DayNoPlan)
                        {
                            if (item.Value == "Y")
                            {
                                dateOff = new DATE_OFF_LINE_LFEM()
                                {
                                    ItemValue = item.Key,
                                    UserCreated = GetUserId(),
                                    DanhMuc = CommonConstants.KHSX
                                };
                                lstDateOffLFEM.Add(dateOff);
                            }
                        }

                        string _beginDate = DateTime.Parse(beginDate).ToString("yyyy-MM-dd");
                        var lstOff = _DateOfflfemLineRepository.FindAll(x => x.ItemValue.CompareTo(_beginDate) >= 0 && x.ItemValue.CompareTo(endDate) <= 0 && x.DanhMuc == CommonConstants.KHSX).ToList();
                        _DateOfflfemLineRepository.RemoveMultiple(lstOff);

                        if (lstDateOffLFEM.Count > 0)
                        {
                            _DateOfflfemLineRepository.AddRange(lstDateOffLFEM);
                        }

                        Save();

                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add("A_BEGIN_MONTH", _beginDate);

                        // LẤY LẠI SẢN LƯỢNG THỰC TẾ trong thang LFEM
                        var rs = _GocPlanLfemRepository.ExecProceduce2("GET_QTY_ACTUAL_BY_DAY_MANUAL_LFEM", dic);
                    }
                }
                else if (param == CommonConstants.DEMAND)
                {
                    using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                    {
                        ExcelWorksheet chipSheet = packet.Workbook.Worksheets[1];
                        List<GOC_PLAN_LFEM> lstGocPlanLfem = new List<GOC_PLAN_LFEM>();
                        List<GOC_PLAN_LFEM> lstGocPlanLfemUpdate = new List<GOC_PLAN_LFEM>();
                        Dictionary<string, string> DayNoPlan = new Dictionary<string, string>();

                        string beginDate = chipSheet.Cells["B1"].Text.NullString();
                        string endDate = "";

                        string _day = "";

                        for (int i = chipSheet.Dimension.Start.Row + 2; i <= chipSheet.Dimension.End.Row; i++)
                        {
                            if (chipSheet.Cells[i, 1].Text.NullString() == "")
                            {
                                break;
                            }

                            for (int k = 0; k < 31; k++)
                            {
                                if (chipSheet.Cells[2, k + 4].Text.NullString() == "" || !DateTime.TryParse(chipSheet.Cells[2, k + 4].Text.NullString(), out _))
                                {
                                    break;
                                }

                                _day = DateTime.Parse(chipSheet.Cells[2, k + 4].Text.NullString()).ToString("yyyy-MM-dd");
                                endDate = _day;

                                GOC_PLAN_LFEM lfemPlan = _GocPlanLfemRepository.FindSingle(x => x.MesItemId == chipSheet.Cells[i, 1].Text.NullString() && x.DatePlan == _day && x.DanhMuc == CommonConstants.DEMAND);
                                if (lfemPlan == null)
                                {
                                    lfemPlan = new GOC_PLAN_LFEM()
                                    {
                                        MesItemId = chipSheet.Cells[i, 1].Text.NullString(),
                                        UserCreated = GetUserId(),
                                        DatePlan = _day,
                                        QuantityPlan = double.Parse(chipSheet.Cells[i, k + 4].Value.IfNullIsZero()),
                                        DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                        DanhMuc = CommonConstants.DEMAND,
                                        MonthPlan = _day.Substring(0, 7) + "-01",
                                        WeekPlan = "W" + (DateTime.Parse(_day).GetWeekOfYear() + 1)
                                    };
                                    lstGocPlanLfem.Add(lfemPlan);
                                }
                                else
                                {
                                    lfemPlan.UserModified = GetUserId();
                                    lfemPlan.QuantityPlan = double.Parse(chipSheet.Cells[i, k + 4].Value.IfNullIsZero());
                                    lfemPlan.DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    lfemPlan.DanhMuc = CommonConstants.DEMAND;
                                    lfemPlan.MonthPlan = _day.Substring(0, 7) + "-01";
                                    lfemPlan.WeekPlan = "W" + (DateTime.Parse(_day).GetWeekOfYear() + 1);
                                    lstGocPlanLfemUpdate.Add(lfemPlan);
                                }

                                if (lfemPlan.QuantityPlan == 0)
                                {
                                    if (!DayNoPlan.ContainsKey(lfemPlan.DatePlan))
                                    {
                                        DayNoPlan.Add(lfemPlan.DatePlan, "Y");
                                    }
                                }
                                else if (lfemPlan.QuantityPlan > 0)
                                {
                                    if (DayNoPlan.ContainsKey(lfemPlan.DatePlan) && DayNoPlan[lfemPlan.DatePlan] == "Y")
                                    {
                                        DayNoPlan[lfemPlan.DatePlan] = "N";
                                    }
                                }
                            }
                        }

                        if (lstGocPlanLfem.Count > 0)
                            _GocPlanLfemRepository.AddRange(lstGocPlanLfem);

                        if (lstGocPlanLfemUpdate.Count > 0)
                            _GocPlanLfemRepository.UpdateRange(lstGocPlanLfemUpdate);

                        // lưu ngày không kế hoạch 
                        DATE_OFF_LINE_LFEM dateOff;

                        List<DATE_OFF_LINE_LFEM> lstDateOffLFEM = new List<DATE_OFF_LINE_LFEM>();
                        foreach (KeyValuePair<string, string> item in DayNoPlan)
                        {
                            if (item.Value == "Y")
                            {
                                dateOff = new DATE_OFF_LINE_LFEM()
                                {
                                    ItemValue = item.Key,
                                    UserCreated = GetUserId(),
                                    DanhMuc = CommonConstants.DEMAND
                                };
                                lstDateOffLFEM.Add(dateOff);
                            }
                        }

                        string _beginDate = DateTime.Parse(beginDate).ToString("yyyy-MM-dd");
                        var lstOff = _DateOfflfemLineRepository.FindAll(x => x.ItemValue.CompareTo(_beginDate) >= 0 && x.ItemValue.CompareTo(endDate) <= 0 && x.DanhMuc == CommonConstants.DEMAND).ToList();
                        _DateOfflfemLineRepository.RemoveMultiple(lstOff);

                        if (lstDateOffLFEM.Count > 0)
                        {
                            _DateOfflfemLineRepository.AddRange(lstDateOffLFEM);
                        }

                        Save();

                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add("A_BEGIN_MONTH", _beginDate);

                        // LẤY LẠI SẢN LƯỢNG THỰC TẾ trong thang LFEM
                        var rs = _GocPlanLfemRepository.ExecProceduce2("GET_QTY_DEMAND_ACTUAL_BY_DAY_MANUAL_LFEM", dic);
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

        public void DeleteGocModelLfem(string model, string fromDate, string toDate, string danhmuc)
        {
            var lst = _GocPlanLfemRepository.FindAll(x => x.MesItemId == model && x.DanhMuc == danhmuc && x.DatePlan.CompareTo(fromDate) >= 0 && x.DatePlan.CompareTo(toDate) <= 0).ToList();
            _GocPlanLfemRepository.RemoveMultiple(lst);
            _unitOfWork.Commit();
        }

        public List<ProcActualPlanModel> GetProcActualPlanLfemModel(string month, string dateto, string danhmuc, string display)
        {
            if (display == CommonConstants.Month)
            {
                int dayOfMonth = DateTime.Parse(month + "-01").AddMonths(1).AddDays(-1).Day;

                List<ProcActualPlanModel> result = new List<ProcActualPlanModel>();
                var lstGoc = _GocPlanLfemRepository.FindAll(x => x.DatePlan.Contains(month) && x.DanhMuc == danhmuc);

                // CHIP
                var lstGroupChip = (from p in lstGoc.AsEnumerable()
                                    group p by new
                                    {
                                        Day = p.DatePlan
                                    } into g
                                    select new QuantityByDay
                                    {
                                        DatePlan = g.Key.Day.Split("-")[2],
                                        QuantityPlan = (float)g.Sum(x => x.QuantityPlan),
                                        QuantityActual = (float)g.Sum(x => x.QuantityActual),
                                        QuantityGap = (float)(g.Sum(x => x.QuantityActual) - g.Sum(x => x.QuantityPlan)),
                                    }).OrderBy(x => int.Parse(x.DatePlan)).ToList();

                var lstChips = UpdateDataLost(lstGroupChip, dayOfMonth, 1, CommonConstants.Day);

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

                ProcActualPlanModel proc = new ProcActualPlanModel();
                proc.QuantityByDays = lstChips;
                proc.Month = month;
                proc.DayOfMonth = dayOfMonth;
                proc.CFAB = CommonConstants.CHIP;
                proc.DanhMuc = danhmuc;
                proc.Display = display;
                proc.DateFrom = DateTime.Parse(month + "-01").ToString("yyyy-MM-dd");
                proc.DateTo = DateTime.Parse(month + "-01").AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                result.Add(proc);

                return result;
            }
            else if (display == CommonConstants.Week)
            {
                int weekStart = DateTime.Parse(month).GetWeekOfYear() + 1;
                int weekEnd = DateTime.Parse(dateto).GetWeekOfYear() + 1;

                List<ProcActualPlanModel> result = new List<ProcActualPlanModel>();
                var lstGoc = _GocPlanLfemRepository.FindAll(x => x.DatePlan.CompareTo(month) >= 0 && x.DatePlan.CompareTo(dateto) <= 0 && x.DanhMuc == danhmuc);

                // CHIP
                var lstGroupChip = (from p in lstGoc.AsEnumerable()
                                    group p by new
                                    {
                                        Week = p.WeekPlan
                                    } into g
                                    select new QuantityByDay
                                    {
                                        DatePlan = g.Key.Week,
                                        QuantityPlan = (float)g.Sum(x => x.QuantityPlan),
                                        QuantityActual = (float)g.Sum(x => x.QuantityActual),
                                        QuantityGap = (float)(g.Sum(x => x.QuantityActual) - g.Sum(x => x.QuantityPlan)),
                                    }).OrderBy(x => x.DatePlan).ToList();

                var lstChips = UpdateDataLost(lstGroupChip, weekEnd, weekStart, CommonConstants.Week);

                if (lstChips.Count > 0)
                {
                    for (int i = 1; i <= lstChips.Count; i++)
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
                        if (item.QtyPlan_Ytd > 0)
                        {
                            item.Qty_Percen_Ytd = Math.Round((item.QtyActual_Ytd / item.QtyPlan_Ytd) * 100, 1);
                        }
                    }
                }

                ProcActualPlanModel proc = new ProcActualPlanModel();
                proc.QuantityByDays = lstChips;
                proc.Month = month;
                proc.DayOfMonth = weekEnd - weekStart + 1;
                proc.CFAB = CommonConstants.CHIP;
                proc.DanhMuc = danhmuc;
                proc.Display = display;
                proc.DateFrom = month;
                proc.DateTo = dateto;
                proc.WeekFrom = weekStart;
                proc.WeekTo = weekEnd;
                result.Add(proc);

                return result;
            }
            else
            {
                var data = _GocPlanLfemRepository.FindAll(x => x.DanhMuc == danhmuc && x.DatePlan.CompareTo(month) == 0).OrderBy(x => x.MesItemId).ToList();

                List<QuantityByDay> newData = new List<QuantityByDay>();
                foreach (var item in data)
                {
                    if (!newData.Exists(x => x.SapCode == item.MesItemId))
                    {
                        newData.Add(new QuantityByDay()
                        {
                            SapCode = item.MesItemId,
                            QuantityPlan = (float)item.QuantityPlan,
                            QuantityActual = (float)item.QuantityActual,
                            QuantityGap = (float)item.QuantityGap,
                            Qty_Percen_Ytd = item.QuantityPlan > 0 ? Math.Round(item.QuantityActual / item.QuantityPlan * 100, 1) : 0
                        });
                    }
                    else
                    {
                        QuantityByDay pl = newData.FirstOrDefault(x => x.SapCode == item.MesItemId);
                        pl.QuantityPlan += (float)item.QuantityPlan;
                        pl.QuantityActual += (float)item.QuantityActual;
                        pl.QuantityGap = (float)(pl.QuantityActual - pl.QuantityPlan);

                        if (pl.QuantityPlan > 0)
                        {
                            pl.Qty_Percen_Ytd = Math.Round(pl.QuantityActual / pl.QuantityPlan * 100, 1);
                        }
                    }
                }

                List<ProcActualPlanModel> result = new List<ProcActualPlanModel>();
                ProcActualPlanModel proc = new ProcActualPlanModel();
                proc.QuantityByDays = newData;
                proc.Month = month;
                proc.CFAB = CommonConstants.CHIP;
                proc.DanhMuc = danhmuc;
                proc.Display = display;
                proc.DateFrom = month;
                proc.DateTo = dateto;
                result.Add(proc);

                return result;
            }
        }

        public List<GOC_PLAN_LFEM> GetDataByDayLfem(string dayFrom, string danhmuc)
        {
            return _GocPlanLfemRepository.FindAll(x => x.DanhMuc == danhmuc && x.DatePlan.CompareTo(dayFrom) == 0).ToList();
        }

        public List<SanXuat_XuatHang_ViewModel> GetSanXuatXuatHang(string month)
        {
            List<SanXuat_XuatHang_ViewModel> lstSXXH = new List<SanXuat_XuatHang_ViewModel>();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("A_MONTH", month);
            ResultDB rs = _GocPlanLfemRepository.ExecProceduce2("GET_TTSX_KHXH_BY_MONTH", dic);
            if (rs.ReturnInt == 0)
            {
                DataTable ActualSX = rs.ReturnDataSet.Tables[0];
                DataTable PlanDemand = rs.ReturnDataSet.Tables[1];
                DataTable Stock = rs.ReturnDataSet.Tables[2];

                // add thuc te san xuat
                SanXuat_XuatHang_ViewModel vm;
                foreach (DataRow Row in ActualSX.Rows)
                {
                    vm = lstSXXH.FirstOrDefault(x => x.MesItem == Row["MesItemId"].NullString());

                    if (vm == null)
                    {
                        vm = new SanXuat_XuatHang_ViewModel()
                        {
                            MesItem = Row["MesItemId"].NullString(),
                            Model = Row["Model"].NullString(),
                            Thang = month
                        };

                        DataRow r = Stock.Select("MesItemId = '" + Row["MesItemId"].NullString() + "'").FirstOrDefault();
                        if (r != null)
                        {
                            vm.TonDauKy = float.Parse(r["TON_DAU_KY"].IfNullIsZero());
                        }

                        lstSXXH.Add(vm);
                    }

                    vm.SX_XH.Add(new SX_XH()
                    {
                        Day = Row["DatePlan"].NullString(),
                        TTSX = float.Parse(Row["QuantityActual"].IfNullIsZero()),
                    });
                }

                // add ke hoach xuat hang
                foreach (DataRow Row in PlanDemand.Rows)
                {
                    vm = lstSXXH.FirstOrDefault(x => x.MesItem == Row["MesItemId"].NullString());

                    if (vm == null)
                    {
                        vm = new SanXuat_XuatHang_ViewModel()
                        {
                            MesItem = Row["MesItemId"].NullString(),
                            Model = Row["Model"].NullString(),
                            Thang = month
                        };

                        DataRow r = Stock.Select("MesItemId = '" + Row["MesItemId"].NullString() + "'").FirstOrDefault();
                        if (r != null)
                        {
                            vm.TonDauKy = float.Parse(r["TON_DAU_KY"].IfNullIsZero());
                        }

                        lstSXXH.Add(vm);
                    }

                    if (vm.SX_XH.Count > 0)
                    {
                        foreach (var item in vm.SX_XH)
                        {
                            if (item.Day == Row["DatePlan"].NullString())
                            {
                                item.KHXH = float.Parse(Row["QuantityPlan"].IfNullIsZero());
                            }
                        }
                    }
                    else
                    {
                        vm.SX_XH.Add(new SX_XH()
                        {
                            Day = Row["DatePlan"].NullString(),
                            KHXH = float.Parse(Row["QuantityPlan"].IfNullIsZero()),
                        });
                    }
                }

                float gap = 0;
                float qtyPre = 0;
                int num = 0;

                // tinh luong con thieu
                foreach (var item in lstSXXH)
                {
                    gap = 0;
                    num = 0;
                    foreach (var sx in item.SX_XH.OrderBy(x => x.Day))
                    {
                        gap = sx.TTSX - sx.KHXH;
                        sx.QtyThieu = num == 0 ? item.TonDauKy + gap : qtyPre + sx.TTSX - sx.KHXH;
                        qtyPre = sx.QtyThieu;
                        num += 1;
                    }
                }

                foreach (var item in lstSXXH)
                {
                    foreach (var sx in item.SX_XH.OrderBy(x => x.Day))
                    {
                        item.D_1 = DateTime.Parse(sx.Day).Day == 1 ? sx.QtyThieu : item.D_1;
                        item.D_2 = DateTime.Parse(sx.Day).Day == 2 ? sx.QtyThieu : item.D_2;
                        item.D_3 = DateTime.Parse(sx.Day).Day == 3 ? sx.QtyThieu : item.D_3;
                        item.D_4 = DateTime.Parse(sx.Day).Day == 4 ? sx.QtyThieu : item.D_4;
                        item.D_5 = DateTime.Parse(sx.Day).Day == 5 ? sx.QtyThieu : item.D_5;
                        item.D_6 = DateTime.Parse(sx.Day).Day == 6 ? sx.QtyThieu : item.D_6;
                        item.D_7 = DateTime.Parse(sx.Day).Day == 7 ? sx.QtyThieu : item.D_7;
                        item.D_8 = DateTime.Parse(sx.Day).Day == 8 ? sx.QtyThieu : item.D_8;
                        item.D_9 = DateTime.Parse(sx.Day).Day == 9 ? sx.QtyThieu : item.D_9;
                        item.D_10 = DateTime.Parse(sx.Day).Day == 10 ? sx.QtyThieu : item.D_10;
                        item.D_11 = DateTime.Parse(sx.Day).Day == 11 ? sx.QtyThieu : item.D_11;
                        item.D_12 = DateTime.Parse(sx.Day).Day == 12 ? sx.QtyThieu : item.D_12;

                        item.D_13 = DateTime.Parse(sx.Day).Day == 13 ? sx.QtyThieu : item.D_13;
                        item.D_14 = DateTime.Parse(sx.Day).Day == 14 ? sx.QtyThieu : item.D_14;
                        item.D_15 = DateTime.Parse(sx.Day).Day == 15 ? sx.QtyThieu : item.D_15;
                        item.D_16 = DateTime.Parse(sx.Day).Day == 16 ? sx.QtyThieu : item.D_16;
                        item.D_17 = DateTime.Parse(sx.Day).Day == 17 ? sx.QtyThieu : item.D_17;
                        item.D_18 = DateTime.Parse(sx.Day).Day == 18 ? sx.QtyThieu : item.D_18;
                        item.D_19 = DateTime.Parse(sx.Day).Day == 19 ? sx.QtyThieu : item.D_19;
                        item.D_20 = DateTime.Parse(sx.Day).Day == 20 ? sx.QtyThieu : item.D_20;
                        item.D_21 = DateTime.Parse(sx.Day).Day == 21 ? sx.QtyThieu : item.D_21;
                        item.D_22 = DateTime.Parse(sx.Day).Day == 22 ? sx.QtyThieu : item.D_22;
                        item.D_23 = DateTime.Parse(sx.Day).Day == 23 ? sx.QtyThieu : item.D_23;
                        item.D_24 = DateTime.Parse(sx.Day).Day == 24 ? sx.QtyThieu : item.D_24;

                        item.D_25 = DateTime.Parse(sx.Day).Day == 25 ? sx.QtyThieu : item.D_25;
                        item.D_26 = DateTime.Parse(sx.Day).Day == 26 ? sx.QtyThieu : item.D_26;
                        item.D_27 = DateTime.Parse(sx.Day).Day == 27 ? sx.QtyThieu : item.D_27;
                        item.D_28 = DateTime.Parse(sx.Day).Day == 28 ? sx.QtyThieu : item.D_28;
                        item.D_29 = DateTime.Parse(sx.Day).Day == 29 ? sx.QtyThieu : item.D_29;
                        item.D_30 = DateTime.Parse(sx.Day).Day == 30 ? sx.QtyThieu : item.D_30;
                        item.D_31 = DateTime.Parse(sx.Day).Day == 31 ? sx.QtyThieu : item.D_31;
                    }
                }
            }

            return lstSXXH;
        }
        #endregion

        #region SMT
        public List<ProcActualPlanModel> GetProcActualPlanSMTModel(string month, string dateto, string danhmuc, string display)
        {
            if (display == CommonConstants.Month)
            {
                int dayOfMonth = DateTime.Parse(month + "-01").AddMonths(1).AddDays(-1).Day;

                List<ProcActualPlanModel> result = new List<ProcActualPlanModel>();
                var lstGoc = _GocPlanSMTRepository.FindAll(x => x.DatePlan.Contains(month) && x.DanhMuc == danhmuc);

                // CHIP
                var lstGroupChip = (from p in lstGoc.AsEnumerable()
                                    group p by new
                                    {
                                        Day = p.DatePlan
                                    } into g
                                    select new QuantityByDay
                                    {
                                        DatePlan = g.Key.Day.Split("-")[2],
                                        QuantityPlan = (float)g.Sum(x => x.QuantityPlan),
                                        QuantityActual = (float)g.Sum(x => x.QuantityActual),
                                        QuantityGap = (float)(g.Sum(x => x.QuantityActual) - g.Sum(x => x.QuantityPlan)),
                                    }).OrderBy(x => int.Parse(x.DatePlan)).ToList();

                var lstChips = UpdateDataLost(lstGroupChip, dayOfMonth, 1, CommonConstants.Day);

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

                ProcActualPlanModel proc = new ProcActualPlanModel();
                proc.QuantityByDays = lstChips;
                proc.Month = month;
                proc.DayOfMonth = dayOfMonth;
                proc.CFAB = CommonConstants.CHIP;
                proc.DanhMuc = danhmuc;
                proc.Display = display;
                proc.DateFrom = DateTime.Parse(month + "-01").ToString("yyyy-MM-dd");
                proc.DateTo = DateTime.Parse(month + "-01").AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                result.Add(proc);

                return result;
            }
            else if (display == CommonConstants.Week)
            {
                int weekStart = DateTime.Parse(month).GetWeekOfYear() + 1;
                int weekEnd = DateTime.Parse(dateto).GetWeekOfYear() + 1;

                List<ProcActualPlanModel> result = new List<ProcActualPlanModel>();
                var lstGoc = _GocPlanSMTRepository.FindAll(x => x.DatePlan.CompareTo(month) >= 0 && x.DatePlan.CompareTo(dateto) <= 0 && x.DanhMuc == danhmuc);

                // CHIP
                var lstGroupChip = (from p in lstGoc.AsEnumerable()
                                    group p by new
                                    {
                                        Week = p.WeekPlan
                                    } into g
                                    select new QuantityByDay
                                    {
                                        DatePlan = g.Key.Week,
                                        QuantityPlan = (float)g.Sum(x => x.QuantityPlan),
                                        QuantityActual = (float)g.Sum(x => x.QuantityActual),
                                        QuantityGap = (float)(g.Sum(x => x.QuantityActual) - g.Sum(x => x.QuantityPlan)),
                                    }).OrderBy(x => x.DatePlan).ToList();

                var lstChips = UpdateDataLost(lstGroupChip, weekEnd, weekStart, CommonConstants.Week);

                if (lstChips.Count > 0)
                {
                    for (int i = 1; i <= lstChips.Count; i++)
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
                        if (item.QtyPlan_Ytd > 0)
                        {
                            item.Qty_Percen_Ytd = Math.Round((item.QtyActual_Ytd / item.QtyPlan_Ytd) * 100, 1);
                        }
                    }
                }

                ProcActualPlanModel proc = new ProcActualPlanModel();
                proc.QuantityByDays = lstChips;
                proc.Month = month;
                proc.DayOfMonth = weekEnd - weekStart + 1;
                proc.CFAB = CommonConstants.CHIP;
                proc.DanhMuc = danhmuc;
                proc.Display = display;
                proc.DateFrom = month;
                proc.DateTo = dateto;
                proc.WeekFrom = weekStart;
                proc.WeekTo = weekEnd;
                result.Add(proc);

                return result;
            }
            else
            {
                var data = _GocPlanSMTRepository.FindAll(x => x.DanhMuc == danhmuc && x.DatePlan.CompareTo(month) == 0).OrderBy(x => x.MesItemId).ToList();

                List<QuantityByDay> newData = new List<QuantityByDay>();
                foreach (var item in data)
                {
                    if (!newData.Exists(x => x.SapCode == item.MesItemId))
                    {
                        newData.Add(new QuantityByDay()
                        {
                            SapCode = item.MesItemId,
                            QuantityPlan = (float)item.QuantityPlan,
                            QuantityActual = (float)item.QuantityActual,
                            QuantityGap = (float)item.QuantityGap,
                            Qty_Percen_Ytd = item.QuantityPlan > 0 ? Math.Round(item.QuantityActual / item.QuantityPlan * 100, 1) : 0
                        });
                    }
                    else
                    {
                        QuantityByDay pl = newData.FirstOrDefault(x => x.SapCode == item.MesItemId);
                        pl.QuantityPlan += (float)item.QuantityPlan;
                        pl.QuantityActual += (float)item.QuantityActual;
                        pl.QuantityGap = (float)(pl.QuantityActual - pl.QuantityPlan);

                        if (pl.QuantityPlan > 0)
                        {
                            pl.Qty_Percen_Ytd = Math.Round(pl.QuantityActual / pl.QuantityPlan * 100, 1);
                        }
                    }
                }

                List<ProcActualPlanModel> result = new List<ProcActualPlanModel>();
                ProcActualPlanModel proc = new ProcActualPlanModel();
                proc.QuantityByDays = newData;
                proc.Month = month;
                proc.CFAB = CommonConstants.CHIP;
                proc.DanhMuc = danhmuc;
                proc.Display = display;
                proc.DateFrom = month;
                proc.DateTo = dateto;
                result.Add(proc);

                return result;
            }
        }

        public void DeleteGocModelSMT(string model, string fromDate, string toDate, string danhmuc)
        {
            var lst = _GocPlanSMTRepository.FindAll(x => x.MesItemId == model && x.DanhMuc == danhmuc && x.DatePlan.CompareTo(fromDate) >= 0 && x.DatePlan.CompareTo(toDate) <= 0).ToList();
            _GocPlanSMTRepository.RemoveMultiple(lst);
            _unitOfWork.Commit();
        }

        public ResultDB ImportExcel_SMT(string filePath, string param)
        {
            ResultDB resultDB = new ResultDB();

            try
            {
                if (param == CommonConstants.KHSX)
                {
                    using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                    {
                        ExcelWorksheet chipSheet = packet.Workbook.Worksheets[1];
                        List<GOC_PLAN_SMT> lstGocPlanSMT = new List<GOC_PLAN_SMT>();
                        List<GOC_PLAN_SMT> lstGocPlanSMTUpdate = new List<GOC_PLAN_SMT>();
                        Dictionary<string, string> DayNoPlan = new Dictionary<string, string>();

                        string beginDate = chipSheet.Cells["B1"].Text.NullString();
                        string endDate = "";

                        string _day = "";
                        for (int i = chipSheet.Dimension.Start.Row + 2; i <= chipSheet.Dimension.End.Row; i++)
                        {
                            if (chipSheet.Cells[i, 1].Text.NullString() == "")
                            {
                                break;
                            }

                            for (int k = 0; k < 40; k++)
                            {

                                if (chipSheet.Cells[2, k + 4].Text.NullString() == "" || !DateTime.TryParse(chipSheet.Cells[2, k + 4].Text.NullString(), out _))
                                {
                                    break;
                                }

                                _day = DateTime.Parse(chipSheet.Cells[2, k + 4].Text.NullString()).ToString("yyyy-MM-dd");

                                endDate = _day;

                                GOC_PLAN_SMT smtPlan = _GocPlanSMTRepository.FindSingle(x => x.MesItemId == chipSheet.Cells[i, 1].Text.NullString() && x.DatePlan == _day && x.DanhMuc == CommonConstants.KHSX);
                                if (smtPlan == null)
                                {
                                    smtPlan = new GOC_PLAN_SMT()
                                    {
                                        MesItemId = chipSheet.Cells[i, 1].Text.NullString(),
                                        UserCreated = GetUserId(),
                                        DatePlan = _day,
                                        MonthPlan = _day.Substring(0, 7) + "-01",
                                        QuantityPlan = double.Parse(chipSheet.Cells[i, k + 4].Value.IfNullIsZero()),
                                        DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                        DanhMuc = CommonConstants.KHSX,
                                        WeekPlan = "W" + (DateTime.Parse(_day).GetWeekOfYear() + 1)
                                    };
                                    lstGocPlanSMT.Add(smtPlan);
                                }
                                else
                                {
                                    smtPlan.UserModified = GetUserId();
                                    smtPlan.QuantityPlan = double.Parse(chipSheet.Cells[i, k + 4].Value.IfNullIsZero());
                                    smtPlan.DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    smtPlan.DanhMuc = CommonConstants.KHSX;
                                    smtPlan.MonthPlan = _day.Substring(0, 7) + "-01";
                                    smtPlan.WeekPlan = "W" + (DateTime.Parse(_day).GetWeekOfYear() + 1);
                                    lstGocPlanSMTUpdate.Add(smtPlan);
                                }

                                if (smtPlan.QuantityPlan == 0)
                                {
                                    if (!DayNoPlan.ContainsKey(smtPlan.DatePlan))
                                    {
                                        DayNoPlan.Add(smtPlan.DatePlan, "Y");
                                    }
                                }
                                else if (smtPlan.QuantityPlan > 0)
                                {
                                    if (DayNoPlan.ContainsKey(smtPlan.DatePlan) && DayNoPlan[smtPlan.DatePlan] == "Y")
                                    {
                                        DayNoPlan[smtPlan.DatePlan] = "N";
                                    }
                                }
                            }
                        }

                        if (lstGocPlanSMT.Count > 0)
                            _GocPlanSMTRepository.AddRange(lstGocPlanSMT);

                        if (lstGocPlanSMTUpdate.Count > 0)
                            _GocPlanSMTRepository.UpdateRange(lstGocPlanSMTUpdate);

                        // lưu ngày không kế hoạch 
                        DATE_OFF_LINE_SMT dateOff;

                        List<DATE_OFF_LINE_SMT> lstDateOffSMT = new List<DATE_OFF_LINE_SMT>();
                        foreach (KeyValuePair<string, string> item in DayNoPlan)
                        {
                            if (item.Value == "Y")
                            {
                                dateOff = new DATE_OFF_LINE_SMT()
                                {
                                    ItemValue = item.Key,
                                    UserCreated = GetUserId(),
                                    DanhMuc = CommonConstants.KHSX
                                };
                                lstDateOffSMT.Add(dateOff);
                            }
                        }

                        if (lstDateOffSMT.Count > 0)
                        {
                            var lstOff = _DateOffLineSMTRepository.FindAll(x => x.ItemValue.CompareTo(beginDate) >= 0 && x.ItemValue.CompareTo(endDate) <= 0 && x.DanhMuc == CommonConstants.KHSX).ToList();
                            _DateOffLineSMTRepository.RemoveMultiple(lstOff);
                            _DateOffLineSMTRepository.AddRange(lstDateOffSMT);
                        }

                        Save();

                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add("A_BEGIN_MONTH", beginDate);

                        // LẤY LẠI SẢN LƯỢNG THỰC TẾ trong thang LFEM
                        var rs = _GocPlanLfemRepository.ExecProceduce2("GET_QTY_ACTUAL_BY_DAY_MANUAL_SMT", dic);
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
        #endregion
    }
}
