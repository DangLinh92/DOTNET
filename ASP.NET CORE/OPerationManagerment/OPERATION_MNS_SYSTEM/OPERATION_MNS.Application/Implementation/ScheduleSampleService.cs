using AutoMapper;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels.Sameple;
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
    public class ScheduleSampleService : BaseService, IScheduleSampleService
    {
        private IRespository<TINH_HINH_SAN_XUAT_SAMPLE, int> _scheduleSampleRepository;
        private IRespository<PHAN_LOAI_MODEL_SAMPLE, string> _phanLoaiModelRepository;
        private IRespository<PHAN_LOAI_HANG_SAMPLE, string> _phanLoaiHangSampleRepository;
        private IRespository<TCARD_SAMPLE, int> _tCardleRepository;
        private IRespository<DELAY_COMMENT_SAMPLE, int> _delayCommentSampleRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ScheduleSampleService
            (
                IRespository<TINH_HINH_SAN_XUAT_SAMPLE, int> scheduleSampleRepository,
                IRespository<PHAN_LOAI_MODEL_SAMPLE, string> phanLoaiModelRepository,
                IRespository<PHAN_LOAI_HANG_SAMPLE, string> phanLoaiHangSampleRepository,
                IRespository<TCARD_SAMPLE, int> tCardleRepository,
                IUnitOfWork unitOfWork,
                IHttpContextAccessor httpContextAccessor,
                IMapper mapper,
                IRespository<DELAY_COMMENT_SAMPLE, int> delayCommentSampleRepository
            )
        {
            _scheduleSampleRepository = scheduleSampleRepository;
            _phanLoaiModelRepository = phanLoaiModelRepository;
            _phanLoaiHangSampleRepository = phanLoaiHangSampleRepository;
            _tCardleRepository = tCardleRepository;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _delayCommentSampleRepository = delayCommentSampleRepository;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public TinhHinhSanXuatSampleViewModel FindByCassetId(string cassetId)
        {
            return _mapper.Map<TinhHinhSanXuatSampleViewModel>(_scheduleSampleRepository.FindAll(x => x.LotNo == cassetId, x => x.DELAY_COMMENT_SAMPLES).FirstOrDefault());
        }

        public TinhHinhSanXuatSampleViewModel FindById(int id)
        {
            return _mapper.Map<TinhHinhSanXuatSampleViewModel>(_scheduleSampleRepository.FindById(id, x => x.DELAY_COMMENT_SAMPLES));
        }

        public List<TinhHinhSanXuatSampleViewModel> GetByTime(string fromTime, string toTime)
        {
            fromTime = fromTime.Replace("-", "");
            toTime = toTime.Replace("-", "");
            var lst = _mapper.Map<List<TinhHinhSanXuatSampleViewModel>>(_scheduleSampleRepository.FindAll(x => x.InputDate.CompareTo(fromTime) >= 0 && x.InputDate.CompareTo(toTime) <= 0, x => x.DELAY_COMMENT_SAMPLES).ToList());
            foreach (var item in lst)
            {
                item.MucDoKhanCap = 99;
            }
            return RemoveHoldLotList(lst);
        }

        public List<TinhHinhSanXuatSampleViewModel> GetOpens()
        {
            var lst = _mapper.Map<List<TinhHinhSanXuatSampleViewModel>>(_scheduleSampleRepository.FindAll(x => x.DeleteFlg != CommonConstants.Y, x => x.DELAY_COMMENT_SAMPLES).ToList());
            foreach (var item in lst)
            {
                item.MucDoKhanCap = 99;
            }
            return RemoveHoldLotList(lst);
        }

        public TinhHinhSanXuatSampleViewModel Update(TinhHinhSanXuatSampleViewModel en)
        {
            en.UserModified = GetUserId();
            TINH_HINH_SAN_XUAT_SAMPLE newEn = _mapper.Map<TINH_HINH_SAN_XUAT_SAMPLE>(en);
            _scheduleSampleRepository.Update(newEn);
            Save();
            return en;
        }

        public ResultDB ImportExcel(string filePath, string param)
        {
            ResultDB resultDB = new ResultDB();

            try
            {
                using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ExcelWorksheet Sheet = packet.Workbook.Worksheets[1];
                    TINH_HINH_SAN_XUAT_SAMPLE en = null;
                    List<TINH_HINH_SAN_XUAT_SAMPLE> lstSapUpdate = new List<TINH_HINH_SAN_XUAT_SAMPLE>();

                    string lotNo = "";

                    for (int i = Sheet.Dimension.Start.Row + 1; i <= Sheet.Dimension.End.Row; i++)
                    {
                        if (Sheet.Cells[i, 1].Text.NullString() == "")
                        {
                            break;
                        }

                        lotNo = Sheet.Cells[i, 1].Text.NullString();

                        en = _scheduleSampleRepository.FindSingle(x => x.LotNo == lotNo);

                        if (en != null)
                        {
                            if (param == "Plan")
                            {
                                en.PlanInputDate = Sheet.Cells[i, 2].Text.NullString() != "" ? DateTime.Parse(Sheet.Cells[i, 2].Text.NullString()).ToString("yyyyMMdd") : "";
                                en.Wall_Plan_Date = Sheet.Cells[i, 3].Text.NullString() != "" ? DateTime.Parse(Sheet.Cells[i, 3].Text.NullString()).ToString("yyyyMMdd") : "";
                                en.Roof_Plan_Date = Sheet.Cells[i, 4].Text.NullString() != "" ? DateTime.Parse(Sheet.Cells[i, 4].Text.NullString()).ToString("yyyyMMdd") : "";
                                en.Seed_Plan_Date = Sheet.Cells[i, 5].Text.NullString() != "" ? DateTime.Parse(Sheet.Cells[i, 5].Text.NullString()).ToString("yyyyMMdd") : "";
                                en.PlatePR_Plan_Date = Sheet.Cells[i, 6].Text.NullString() != "" ? DateTime.Parse(Sheet.Cells[i, 6].Text.NullString()).ToString("yyyyMMdd") : "";
                                en.Plate_Plan_Date = Sheet.Cells[i, 7].Text.NullString() != "" ? DateTime.Parse(Sheet.Cells[i, 7].Text.NullString()).ToString("yyyyMMdd") : "";
                                en.PreProbe_Plan_Date = Sheet.Cells[i, 8].Text.NullString() != "" ? DateTime.Parse(Sheet.Cells[i, 8].Text.NullString()).ToString("yyyyMMdd") : "";
                                en.PreDicing_Plan_Date = Sheet.Cells[i, 9].Text.NullString() != "" ? DateTime.Parse(Sheet.Cells[i, 9].Text.NullString()).ToString("yyyyMMdd") : "";
                                en.AllProbe_Plan_Date = Sheet.Cells[i, 10].Text.NullString() != "" ? DateTime.Parse(Sheet.Cells[i, 10].Text.NullString()).ToString("yyyyMMdd") : "";
                                en.BG_Plan_Date = Sheet.Cells[i, 11].Text.NullString() != "" ? DateTime.Parse(Sheet.Cells[i, 11].Text.NullString()).ToString("yyyyMMdd") : "";
                                en.Dicing_Plan_Date = Sheet.Cells[i, 12].Text.NullString() != "" ? DateTime.Parse(Sheet.Cells[i, 12].Text.NullString()).ToString("yyyyMMdd") : "";
                                en.ChipIns_Plan_Date = Sheet.Cells[i, 13].Text.NullString() != "" ? DateTime.Parse(Sheet.Cells[i, 13].Text.NullString()).ToString("yyyyMMdd") : "";
                                en.Packing_Plan_Date = Sheet.Cells[i, 14].Text.NullString() != "" ? DateTime.Parse(Sheet.Cells[i, 14].Text.NullString()).ToString("yyyyMMdd") : "";
                                en.OQC_Plan_Date = Sheet.Cells[i, 15].Text.NullString() != "" ? DateTime.Parse(Sheet.Cells[i, 15].Text.NullString()).ToString("yyyyMMdd") : "";
                                en.Shipping_Plan_Date = Sheet.Cells[i, 16].Text.NullString() != "" ? DateTime.Parse(Sheet.Cells[i, 16].Text.NullString()).ToString("yyyyMMdd") : "";
                                en.UserModified = GetUserId();
                            }
                            else
                            {
                                en.PreProbe_Actual_Date = Sheet.Cells[i, 2].Text.NullString() != "" ? DateTime.Parse(Sheet.Cells[i, 2].Text.NullString()).ToString("yyyyMMdd") : "";
                                en.PreDicing_Actual_Date = Sheet.Cells[i, 3].Text.NullString() != "" ? DateTime.Parse(Sheet.Cells[i, 3].Text.NullString()).ToString("yyyyMMdd") : "";
                                en.UserModified = GetUserId();
                            }
                            lstSapUpdate.Add(en);
                        }
                    }

                    if (lstSapUpdate.Count > 0)
                    {
                        _scheduleSampleRepository.UpdateRange(lstSapUpdate);
                    }
                    else
                    {
                        resultDB.ReturnInt = -1;
                        resultDB.ReturnString = "Not found data";
                        return resultDB;
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

        private List<TinhHinhSanXuatSampleViewModel> RemoveHoldLotList(List<TinhHinhSanXuatSampleViewModel> lstTinhHinhSX)
        {
            ResultDB resultDB = _scheduleSampleRepository.ExecProceduce2("PKG_BUSINESS@GET_STAY_LOT_LIST_SAMPLE_2", new Dictionary<string, string>());

            if (resultDB.ReturnInt == 0)
            {
                if (resultDB.ReturnDataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable data = resultDB.ReturnDataSet.Tables[0];
                    int count = 0;
                    int count1 = 0;
                    foreach (var item in lstTinhHinhSX.ToList())
                    {
                        count = data.AsEnumerable().Where(x => x["Cassette ID"].NullString() == item.LotNo && x["Hold Flag"].NullString() == CommonConstants.Y).Count();
                        count1 = data.AsEnumerable().Where(x => x["Cassette ID"].NullString() == item.LotNo).Count();

                        // tất cả lot đang hold
                        if (count == count1 && count > 0)
                        {
                            lstTinhHinhSX.Remove(item);
                        }
                    }

                }
            }

            return lstTinhHinhSX;
        }

        /// <summary>
        /// Get LeadTime data for chart
        /// </summary>
        /// <param name="fromTime"></param>
        /// <param name="toTime"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public LeadTimeSampleModel GetDataLeadTimeChart(string year, string month, string week, string code, string nguoiphutrach, string gap)
        {
            month = month.NullString();
            week = week.NullString();
            code = code.NullString();

            List<LeadtimeViewModel> lstLeadtimeVM = new List<LeadtimeViewModel>();
            List<LeadtimeViewModel> lstLeadtimeVM_Year = new List<LeadtimeViewModel>();
            var lstLeadTime = _scheduleSampleRepository.FindAll(x => x.OutputDate != null && x.OutputDate.Length == 8 && x.OutputDate.Substring(0, 4) == year).Distinct().ToList();
            var lstLeadTime2 = lstLeadTime.ToList();

            lstLeadTime = GetByMonth(lstLeadTime, month);
            lstLeadTime = GetByWeek(lstLeadTime, week);
            lstLeadTime = GetByCode(lstLeadTime, code);
            lstLeadTime = GetByPhuTrach(lstLeadTime, nguoiphutrach);

            LeadtimeViewModel leadtime;
            foreach (var item in lstLeadTime)
            {
                leadtime = new LeadtimeViewModel()
                {
                    LotNo = item.LotNo,
                    KeHoachIn = item.PlanInputDate,
                    ThucTeIn = item.InputDate,
                    KeHoachOut = item.PlanOutputDate,
                    ThucTeOut = item.OutputDate,
                    Week = WeekOfYear(item.OutputDate),
                    Month = int.Parse(item.OutputDate.Substring(4, 2)),
                    Year = int.Parse(item.OutputDate.Substring(0, 4)),
                    PLCode = item.Code.NullString(),
                    ModelRutGon = item.ModelDonLinhKien.Substring(0, 7),
                    Model = item.ModelDonLinhKien,
                    SoTam = item.OutPutWafer,
                    Code_R = item.Code.NullString().ToUpper() == "R" ? item.OutPutWafer : 0,
                    Code_P = item.Code.NullString().ToUpper() == "P" ? item.OutPutWafer : 0,
                    Code_H = item.Code.NullString().ToUpper() == "H" ? item.OutPutWafer : 0,
                    Code_Z = item.Code.NullString().ToUpper() == "Z" ? item.OutPutWafer : 0,
                    Code_M = item.Code.NullString().ToUpper() == "M" ? item.OutPutWafer : 0,
                    LeadTimeActual = item.LeadTime > 0 ? item.LeadTime : 0,
                    LTCode_PRZ = (float)((item.LeadTime > 0 ? item.LeadTime : 0) + 1.5),
                    NguoiChiuTrachNhiem = item.NguoiChiuTrachNhiem,
                    LeadTimePlan = item.LeadTimePlan > 0 ? item.LeadTimePlan : 0,
                    Gap = (item.LeadTime > 0 ? item.LeadTime : 0) - (item.LeadTimePlan > 0 ? item.LeadTimePlan : 0),
                    LyDoDelay = item.GhiChu,
                    DonViTinh = 1,
                    PLHang = "",
                };

                lstLeadtimeVM.Add(leadtime);
            }

            foreach (var item in lstLeadTime2)
            {
                leadtime = new LeadtimeViewModel()
                {
                    LotNo = item.LotNo,
                    KeHoachIn = item.PlanInputDate,
                    ThucTeIn = item.InputDate,
                    KeHoachOut = item.PlanOutputDate,
                    ThucTeOut = item.OutputDate,
                    Week = WeekOfYear(item.OutputDate),
                    Month = int.Parse(item.OutputDate.Substring(4, 2)),
                    Year = int.Parse(item.OutputDate.Substring(0, 4)),
                    PLCode = item.Code.NullString(),
                    ModelRutGon = item.ModelDonLinhKien.Substring(0, 7),
                    Model = item.ModelDonLinhKien,
                    SoTam = item.OutPutWafer,
                    Code_R = item.Code.NullString().ToUpper() == "R" ? item.OutPutWafer : 0,
                    Code_P = item.Code.NullString().ToUpper() == "P" ? item.OutPutWafer : 0,
                    Code_H = item.Code.NullString().ToUpper() == "H" ? item.OutPutWafer : 0,
                    Code_Z = item.Code.NullString().ToUpper() == "Z" ? item.OutPutWafer : 0,
                    Code_M = item.Code.NullString().ToUpper() == "M" ? item.OutPutWafer : 0,
                    LeadTimeActual = item.LeadTime > 0 ? item.LeadTime : 0,
                    LTCode_PRZ = (float)((item.LeadTime > 0 ? item.LeadTime : 0) + 1.5),
                    NguoiChiuTrachNhiem = item.NguoiChiuTrachNhiem,
                    LeadTimePlan = item.LeadTimePlan > 0 ? item.LeadTimePlan : 0,
                    Gap = (item.LeadTime > 0 ? item.LeadTime : 0) - (item.LeadTimePlan > 0 ? item.LeadTimePlan : 0),
                    LyDoDelay = item.GhiChu,
                    DonViTinh = 1,
                    PLHang = "",
                };
                lstLeadtimeVM_Year.Add(leadtime);
            }

            if (gap.NullString() != "" && int.TryParse(gap, out _) && gap != "-9999")
            {
                lstLeadtimeVM = lstLeadtimeVM.Where(x => x.Gap == float.Parse(gap)).ToList();
            }

            LeadTimeSampleModel result = new LeadTimeSampleModel(year);
            result.Month = month;
            result.Week = week == "" ? 0 : int.Parse(week);
            result.NguoiPhuTrach = nguoiphutrach;
            result.Total_Code_P = lstLeadtimeVM.Sum(x => x.Code_P);
            result.Total_Code_H = lstLeadtimeVM.Sum(x => x.Code_H);
            result.Total_Code_R = lstLeadtimeVM.Sum(x => x.Code_R);
            result.Total_Code_Z = lstLeadtimeVM.Sum(x => x.Code_Z);
            result.Total_Code_M = lstLeadtimeVM.Sum(x => x.Code_M);
            result.Weeks = GetWeeksByMonth(year, month.NullString());
            result.Weeks_Lable = GetWeeks(year);
            result.Code = code;
            result.Codes = lstLeadtimeVM.Select(x => x.PLCode).Where(x => x != "").Distinct().ToList();
            result.NguoiPhuTrachs = lstLeadtimeVM.Select(x => x.NguoiChiuTrachNhiem).Where(x => x != "").Distinct().ToList();

            result.Gap = int.Parse(gap);

            List<string> Codes = new List<string>() { "H", "P", "R", "Z", "M" };

            var monthGroup = lstLeadtimeVM_Year.GroupBy(x => new { x.PLCode, x.Month }).Select(x => new { x.Key.PLCode, x.Key.Month, avg = x.Average(p => p.LTCode_PRZ) }).OrderBy(x => x.Month);

            ChartDataSampleItem sampleItem;
            foreach (var item in monthGroup)
            {
                sampleItem = new ChartDataSampleItem()
                {
                    Label_x = item.Month + "",
                    Target_P = 5.5,
                    Target_R = 9.5
                };

                sampleItem.Code_H = item.PLCode.NullString().ToUpper() == "H" ? Math.Round(item.avg, 1) : 0;
                sampleItem.Code_P = item.PLCode.NullString().ToUpper() == "P" ? Math.Round(item.avg, 1) : 0;
                sampleItem.Code_R = item.PLCode.NullString().ToUpper() == "R" ? Math.Round(item.avg, 1) : 0;
                sampleItem.Code_Z = item.PLCode.NullString().ToUpper() == "Z" ? Math.Round(item.avg, 1) : 0;
                sampleItem.Code_M = item.PLCode.NullString().ToUpper() == "M" ? Math.Round(item.avg, 1) : 0;
                sampleItem.Value = item.avg;
                sampleItem.Legend = item.PLCode.NullString().ToUpper();

                result.Sample_LeadTimeByMonth.Add(sampleItem);
            }

            // add code thiếu trong tháng
            AddMonthLost(month, year, Codes, true, ref result.Sample_LeadTimeByMonth);

            // sắp xếp theo month
            result.Sample_LeadTimeByMonth = result.Sample_LeadTimeByMonth.OrderBy(x => int.Parse(x.Label_x)).ToList();

            // leadtime by week
            var weekGroup = lstLeadtimeVM.Where(x => result.Weeks.Contains(x.Week + "")).GroupBy(x => new { x.PLCode, x.Week }).Select(x => new { x.Key.PLCode, x.Key.Week, avg = x.Average(p => p.LTCode_PRZ) }).OrderBy(x => x.Week);
            foreach (var item in weekGroup)
            {
                sampleItem = new ChartDataSampleItem()
                {
                    Label_x = item.Week + "",
                    Target_P = 5.5,
                    Target_R = 9.5
                };

                sampleItem.Code_H = item.PLCode.NullString().ToUpper() == "H" ? Math.Round(item.avg, 1) : 0;
                sampleItem.Code_P = item.PLCode.NullString().ToUpper() == "P" ? Math.Round(item.avg, 1) : 0;
                sampleItem.Code_R = item.PLCode.NullString().ToUpper() == "R" ? Math.Round(item.avg, 1) : 0;
                sampleItem.Code_Z = item.PLCode.NullString().ToUpper() == "Z" ? Math.Round(item.avg, 1) : 0;
                sampleItem.Code_M = item.PLCode.NullString().ToUpper() == "M" ? Math.Round(item.avg, 1) : 0;
                sampleItem.Value = item.avg;
                sampleItem.Legend = item.PLCode.NullString().ToUpper();

                result.Sample_LeadTimeByWeek.Add(sampleItem);
            }

            // add code thiếu trong tuần
            AddWeekLost(week, result.Weeks, Codes, ref result.Sample_LeadTimeByWeek);
            result.Sample_LeadTimeByWeek = result.Sample_LeadTimeByWeek.OrderBy(x => int.Parse(x.Label_x)).ToList();

            // tổng số tấm theo tháng
            var monthWaferGroup = lstLeadtimeVM.GroupBy(x => new { x.PLCode, x.Month }).Select(x => new { x.Key.PLCode, x.Key.Month, sum = x.Sum(p => p.SoTam) }).OrderBy(x => x.Month);

            foreach (var item in monthWaferGroup)
            {
                sampleItem = new ChartDataSampleItem()
                {
                    Label_x = item.Month + "",
                    Target_P = 5.5,
                    Target_R = 9.5
                };

                sampleItem.Code_H = item.PLCode.NullString().ToUpper() == "H" ? item.sum : 0;
                sampleItem.Code_P = item.PLCode.NullString().ToUpper() == "P" ? item.sum : 0;
                sampleItem.Code_R = item.PLCode.NullString().ToUpper() == "R" ? item.sum : 0;
                sampleItem.Code_Z = item.PLCode.NullString().ToUpper() == "Z" ? item.sum : 0;
                sampleItem.Code_M = item.PLCode.NullString().ToUpper() == "M" ? item.sum : 0;
                sampleItem.Value = item.sum;
                sampleItem.Legend = item.PLCode.NullString().ToUpper();

                result.Sample_WaferByMonth.Add(sampleItem);
            }

            // add code thiếu trong tháng
            AddMonthLost(month, year, Codes, false, ref result.Sample_WaferByMonth);
            result.Sample_WaferByMonth = result.Sample_WaferByMonth.OrderBy(x => int.Parse(x.Label_x)).ToList();

            // tổng số tấm theo tuần
            var weekWaferGroup = lstLeadtimeVM.Where(x => result.Weeks.Contains(x.Week + "")).GroupBy(x => new { x.PLCode, x.Week }).Select(x => new { x.Key.PLCode, x.Key.Week, sum = x.Sum(p => p.SoTam) }).OrderBy(x => x.Week);

            foreach (var item in weekWaferGroup)
            {
                sampleItem = new ChartDataSampleItem()
                {
                    Label_x = item.Week + "",
                    Target_P = 5.5,
                    Target_R = 9.5
                };

                sampleItem.Code_H = item.PLCode.NullString().ToUpper() == "H" ? item.sum : 0;
                sampleItem.Code_P = item.PLCode.NullString().ToUpper() == "P" ? item.sum : 0;
                sampleItem.Code_R = item.PLCode.NullString().ToUpper() == "R" ? item.sum : 0;
                sampleItem.Code_Z = item.PLCode.NullString().ToUpper() == "Z" ? item.sum : 0;
                sampleItem.Code_M = item.PLCode.NullString().ToUpper() == "M" ? item.sum : 0;
                sampleItem.Value = item.sum;
                sampleItem.Legend = item.PLCode.NullString().ToUpper();

                result.Sample_WaferByWeek.Add(sampleItem);
            }

            // add code thiếu trong tuần
            AddWeekLost(week, result.Weeks, Codes, ref result.Sample_WaferByWeek);
            result.Sample_WaferByWeek = result.Sample_WaferByWeek.OrderBy(x => int.Parse(x.Label_x)).ToList();

            // chịu TN theo tg
            var sampleChiuTN = lstLeadtimeVM.GroupBy(x => new { x.ModelRutGon, x.NguoiChiuTrachNhiem }).Select(x => new { x.Key.ModelRutGon, x.Key.NguoiChiuTrachNhiem, sum = x.Sum(p => p.SoTam) });
            int totalSoTam = sampleChiuTN.Sum(x => x.sum);
            foreach (var item in sampleChiuTN)
            {
                sampleItem = new ChartDataSampleItem()
                {
                    Legend = item.NguoiChiuTrachNhiem + " " + item.ModelRutGon,
                };

                sampleItem.Value = item.sum;
                sampleItem.Rate = (double)Math.Round((decimal)(item.sum * 100) / totalSoTam, 2);

                result.SampleChiuTN.Add(sampleItem);
            }

            //  Số lượng theo Gap
            var sampleGaps = lstLeadtimeVM.GroupBy(x => x.Gap).Select(x => new { gap = x.Key, numberGap = x.Count(p => p.LotNo != "") }).OrderBy(x => x.gap);
            var sumNumberGap = sampleGaps.Sum(x => x.numberGap);
            foreach (var item in sampleGaps)
            {
                sampleItem = new ChartDataSampleItem()
                {
                    Label_x = item.gap.ToString(),
                    Value = item.numberGap,
                    Rate = Math.Round(100.0 * item.numberGap / sumNumberGap, 2)
                };
                result.SampleGapLeadtime.Add(sampleItem);
            }

            result.SampleDetail = lstLeadtimeVM;
            foreach (var item in result.SampleGapLeadtime)
            {
                if (!result.Gaps.Contains(int.Parse(item.Label_x)))
                {
                    result.Gaps.Add(int.Parse(item.Label_x));
                }
            }

            return result;
        }

        private List<TINH_HINH_SAN_XUAT_SAMPLE> GetByMonth(List<TINH_HINH_SAN_XUAT_SAMPLE> lst, string month)
        {
            if (month.NullString() == "")
            {
                return lst;
            }
            else
            {
                return lst.Where(x => x.OutputDate != "" && int.Parse(x.OutputDate.Substring(4, 2)) == int.Parse(month)).ToList();
            }
        }

        private List<TINH_HINH_SAN_XUAT_SAMPLE> GetByPhuTrach(List<TINH_HINH_SAN_XUAT_SAMPLE> lst, string nguoiphutrach)
        {
            if (nguoiphutrach.NullString() == "")
            {
                return lst;
            }
            else
            {
                return lst.Where(x => x.NguoiChiuTrachNhiem == nguoiphutrach).ToList();
            }
        }

        private List<TINH_HINH_SAN_XUAT_SAMPLE> GetByWeek(List<TINH_HINH_SAN_XUAT_SAMPLE> lst, string week)
        {
            if (week.NullString() == "")
            {
                return lst;
            }
            else
            {
                return lst.Where(x => x.OutputDate != "" && WeekOfYear(x.OutputDate) == int.Parse(week)).ToList();
            }
        }

        private List<TINH_HINH_SAN_XUAT_SAMPLE> GetByCode(List<TINH_HINH_SAN_XUAT_SAMPLE> lst, string code)
        {
            if (code.NullString() == "")
            {
                return lst;
            }
            else
            {
                return lst.Where(x => x.Code == code).ToList();
            }
        }

        public int WeekOfYear(string datetime)
        {
            int w;
            if (datetime.Contains("-"))
            {
                w = DateTime.Parse(datetime).GetWeekOfYear();
            }
            else
            {
                w = DateTime.Parse(datetime.Substring(0, 4) + "-" + datetime.Substring(4, 2) + "-" + datetime.Substring(6, 2)).GetWeekOfYear();
            }

            return w == 0 ? 1 : w + 1;
        }

        public List<string> GetWeeks(string Year)
        {
            List<string> result = new List<string>();
            string beginYear = Year + "-01-01";
            string endYear = DateTime.Parse(beginYear).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");

            int weekOfYear = DateTime.Parse(endYear).GetWeekOfYear() + 1;

            if (Year == DateTime.Now.Year.ToString())
            {
                weekOfYear = DateTime.Now.GetWeekOfYear() + 1;
            }

            for (int i = 1; i <= weekOfYear; i++)
            {
                result.Add(i + "");
            }
            return result;
        }

        public List<string> GetWeeksByMonth(string year, string month)
        {
            if (string.IsNullOrEmpty(month))
                return GetWeeks(year);

            List<string> result = new List<string>();
            string beginMonth = year + "-" + month + "-01";
            string endMonth = DateTime.Parse(beginMonth).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");

            int weekOfYear;
            foreach (var day in EachDay.EachDays(DateTime.Parse(beginMonth), DateTime.Parse(endMonth)))
            {
                weekOfYear = day.GetWeekOfYear() + 1;
                if (!result.Contains(weekOfYear + "") && weekOfYear > 0)
                {
                    result.Add(weekOfYear + "");
                }
            }
            result.Sort((a, b) => int.Parse(a).CompareTo(int.Parse(b)));
            return result;
        }

        public List<DELAY_COMMENT_SAMPLE> GetDelayOpen()
        {
            var lst = _delayCommentSampleRepository.FindAll(x => x.TINH_HINH_SAN_XUAT_SAMPLE.DeleteFlg != CommonConstants.Y, x => x.TINH_HINH_SAN_XUAT_SAMPLE).ToList();
            return lst;
        }

        public List<DELAY_COMMENT_SAMPLE> GetDelayByTime(string fromTime, string toTime)
        {
            fromTime = fromTime.Replace("-", "");
            toTime = toTime.Replace("-", "");
            var lst = _delayCommentSampleRepository.FindAll(x => x.TINH_HINH_SAN_XUAT_SAMPLE.InputDate.CompareTo(fromTime) >= 0 && x.TINH_HINH_SAN_XUAT_SAMPLE.InputDate.CompareTo(toTime) <= 0, x => x.TINH_HINH_SAN_XUAT_SAMPLE).ToList();
            return lst;
        }

        public ResultDB ImportDelayExcel(string filePath, string param)
        {
            ResultDB resultDB = new ResultDB();

            try
            {
                using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ExcelWorksheet Sheet = packet.Workbook.Worksheets[1];
                    DELAY_COMMENT_SAMPLE en = null;
                    List<DELAY_COMMENT_SAMPLE> lstSapUpdate = new List<DELAY_COMMENT_SAMPLE>();

                    string lotNo = "";

                    for (int i = Sheet.Dimension.Start.Row + 1; i <= Sheet.Dimension.End.Row; i++)
                    {
                        if (Sheet.Cells[i, 1].Text.NullString() == "")
                        {
                            break;
                        }

                        lotNo = Sheet.Cells[i, 1].Text.NullString();

                        en = _delayCommentSampleRepository.FindSingle(x => x.TINH_HINH_SAN_XUAT_SAMPLE.LotNo == lotNo, x => x.TINH_HINH_SAN_XUAT_SAMPLE);

                        if (en != null)
                        {
                            en.Wall = Sheet.Cells[i, 2].Text.NullString();
                            en.Roof = Sheet.Cells[i, 3].Text.NullString();
                            en.Seed = Sheet.Cells[i, 4].Text.NullString();
                            en.PlatePR = Sheet.Cells[i, 5].Text.NullString();
                            en.Plate = Sheet.Cells[i, 6].Text.NullString();
                            en.PreProbe = Sheet.Cells[i, 7].Text.NullString();
                            en.PreDicing = Sheet.Cells[i, 8].Text.NullString();
                            en.AllProbe = Sheet.Cells[i, 9].Text.NullString();
                            en.BG = Sheet.Cells[i, 10].Text.NullString();
                            en.Dicing = Sheet.Cells[i, 11].Text.NullString();
                            en.ChipIns = Sheet.Cells[i, 12].Text.NullString();
                            en.Packing = Sheet.Cells[i, 13].Text.NullString();
                            en.OQC = Sheet.Cells[i, 14].Text.NullString();
                            en.Shipping = Sheet.Cells[i, 15].Text.NullString();
                            en.UserModified = GetUserId();

                            lstSapUpdate.Add(en);
                        }
                    }

                    if (lstSapUpdate.Count > 0)
                    {
                        _delayCommentSampleRepository.UpdateRange(lstSapUpdate);
                    }
                    else
                    {
                        resultDB.ReturnInt = -1;
                        resultDB.ReturnString = "Not found data";
                        return resultDB;
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

        public DELAY_COMMENT_SAMPLE UpdateComment(DELAY_COMMENT_SAMPLE en)
        {
            _delayCommentSampleRepository.Update(en);
            Save();
            return en;
        }

        public DELAY_COMMENT_SAMPLE FindCommentById(int id)
        {
            DELAY_COMMENT_SAMPLE en = _delayCommentSampleRepository.FindById(id, x => x.TINH_HINH_SAN_XUAT_SAMPLE);
            return en;
        }


        private void AddWeekLost(string week, List<string> Weeks, List<string> Codes, ref List<ChartDataSampleItem> Sample_LeadTimeWeek)
        {
            List<string> _weeks = new List<string>();
            if (week == "")
                _weeks.AddRange(Weeks);
            else
                _weeks.Add(week);

            List<string> isExistW;
            foreach (var w in _weeks.OrderBy(x => int.Parse(x)))
            {
                isExistW = new List<string>();
                foreach (var item in Sample_LeadTimeWeek.ToList())
                {
                    if (int.Parse(item.Label_x) == int.Parse(w))
                    {
                        isExistW.Add(item.Legend);
                    }
                }

                // Code không có trong tuần
                foreach (var _code in Codes.Except(isExistW))
                {
                    Sample_LeadTimeWeek.Add(new ChartDataSampleItem()
                    {
                        Label_x = w + "",
                        Target_P = 5.5,
                        Target_R = 9.5,
                        Code_H = 0,
                        Code_R = 0,
                        Code_P = 0,
                        Code_Z = 0,
                        Code_M = 0,
                        Legend = _code
                    });
                }
            }
        }

        private void AddMonthLost(string month, string year, List<string> Codes, bool isGetFromOne, ref List<ChartDataSampleItem> Sample_LeadTimeByMonth)
        {
            int _month = 0;
            // thêm tháng còn thiếu
            if (month.NullString() == "" && DateTime.Now.Year + "" == year)
            {
                _month = DateTime.Now.Month;
            }
            else if (month.NullString() == "" && DateTime.Now.Year < int.Parse(year))
            {
                _month = 12;
            }
            else if (month.NullString() != "")
            {
                _month = int.Parse(month);
            }

            List<string> isExistM;
            for (int i = 1; i <= _month; i++)
            {
                if (isGetFromOne || month == "")
                {
                    isExistM = new List<string>();
                    foreach (var item in Sample_LeadTimeByMonth.ToList())
                    {
                        if (int.Parse(item.Label_x) == i)
                        {
                            isExistM.Add(item.Legend);
                        }
                    }

                    // Code không có trong tháng
                    foreach (var _code in Codes.Except(isExistM))
                    {
                        Sample_LeadTimeByMonth.Add(new ChartDataSampleItem()
                        {
                            Label_x = i + "",
                            Target_P = 5.5,
                            Target_R = 9.5,
                            Code_H = 0,
                            Code_R = 0,
                            Code_P = 0,
                            Code_Z = 0,
                            Code_M = 0,
                            Legend = _code
                        });
                    }
                }
                else if (!isGetFromOne && month != "")
                {
                    if (i == _month)
                    {
                        isExistM = new List<string>();
                        foreach (var item in Sample_LeadTimeByMonth.ToList())
                        {
                            if (int.Parse(item.Label_x) == i)
                            {
                                isExistM.Add(item.Legend);
                            }
                        }

                        // Code không có trong tháng
                        foreach (var _code in Codes.Except(isExistM))
                        {
                            Sample_LeadTimeByMonth.Add(new ChartDataSampleItem()
                            {
                                Label_x = i + "",
                                Target_P = 5.5,
                                Target_R = 9.5,
                                Code_H = 0,
                                Code_R = 0,
                                Code_P = 0,
                                Code_Z = 0,
                                Code_M = 0,
                                Legend = _code
                            });
                        }
                    }
                }

            }
        }
    }
}
