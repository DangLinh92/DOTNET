using AutoMapper;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using VOC.Application.Interfaces;
using VOC.Application.ViewModels.System;
using VOC.Application.ViewModels.VOC;
using VOC.Data.EF.Extensions;
using VOC.Data.Entities;
using VOC.Infrastructure.Interfaces;
using VOC.Utilities.Dtos;

namespace VOC.Application.Implementation
{
    public class VocMstService : BaseService, IVocMstService
    {
        private IRespository<VOC_MST, int> _vocRepository;
        private IRespository<VOC_DEFECT_TYPE, int> _vocDefectTypeRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VocMstService(IRespository<VOC_MST, int> vocRepository, IRespository<VOC_DEFECT_TYPE, int> vocDefectTypeRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _vocRepository = vocRepository;
            _vocDefectTypeRepository = vocDefectTypeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public VOC_MSTViewModel Add(VOC_MSTViewModel voc)
        {
            voc.UserCreated = GetUserId();
            var entity = _mapper.Map<VOC_MST>(voc);
            _vocRepository.Add(entity);
            return voc;
        }

        public void Delete(int id)
        {
            _vocRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<VOC_MSTViewModel> GetAll(string filter)
        {
            throw new NotImplementedException();
        }

        public VOC_MSTViewModel GetById(int id)
        {
            return _mapper.Map<VOC_MSTViewModel>(_vocRepository.FindById(id));
        }

        public ResultDB ImportExcel(string filePath, string param)
        {
            ResultDB resultDB = new ResultDB();
            try
            {
                using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = packet.Workbook.Worksheets[1];
                    List<VOC_MST> lstVocMst = new List<VOC_MST>();

                    DataTable table = new DataTable();
                    table.Columns.Add("Received_site");
                    table.Columns.Add("PlaceOfOrigin");
                    table.Columns.Add("ReceivedDept");
                    table.Columns.Add("ReceivedDate");
                    table.Columns.Add("SPLReceivedDate");
                    table.Columns.Add("SPLReceivedDateWeek");
                    table.Columns.Add("Customer");
                    table.Columns.Add("SETModelCustomer");
                    table.Columns.Add("ProcessCustomer");
                    table.Columns.Add("ModelFullname");
                    table.Columns.Add("DefectNameCus");
                    table.Columns.Add("DefectRate");
                    table.Columns.Add("PBA_FAE_Result");
                    table.Columns.Add("PartsClassification");
                    table.Columns.Add("PartsClassification2");
                    table.Columns.Add("ProdutionDateMarking");
                    table.Columns.Add("AnalysisResult");
                    table.Columns.Add("VOCCount");
                    table.Columns.Add("DefectCause");
                    table.Columns.Add("DefectClassification");
                    table.Columns.Add("CustomerResponse");
                    table.Columns.Add("Report_FinalApprover");
                    table.Columns.Add("Report_Sender");
                    table.Columns.Add("Rport_sentDate");
                    table.Columns.Add("VOCState");
                    table.Columns.Add("VOCFinishingDate");
                    table.Columns.Add("VOC_TAT");

                    DataRow row = null;
                    for (int i = worksheet.Dimension.Start.Row + 3; i <= worksheet.Dimension.End.Row; i++) // Start.Row = 2
                    {
                        row = table.NewRow();

                        if (worksheet.Cells[i, 3].Text.NullString() == "" || worksheet.Cells[i, 4].Text.NullString() == "")
                        {
                            continue;
                        }

                        row["Received_site"] = worksheet.Cells[i, 3].Text.NullString();
                        row["PlaceOfOrigin"] = worksheet.Cells[i, 4].Text.NullString();
                        row["ReceivedDept"] = worksheet.Cells[i, 5].Text.NullString();
                        row["ReceivedDate"] = worksheet.Cells[i, 6].Text.NullString();

                        if (!DateTime.TryParse(worksheet.Cells[i, 7].Text.NullString(), out _))
                        {
                            resultDB.ReturnInt = -1;
                            resultDB.ReturnString = "Received date is format (YYYY-MM-DD) : " + worksheet.Cells[i, 7].Text.NullString();
                            return resultDB;
                        }

                        row["SPLReceivedDate"] = worksheet.Cells[i, 7].Text.NullString();

                        if (!int.TryParse(worksheet.Cells[i, 8].Text.NullString().ToUpper().Substring(1), out _))
                        {
                            resultDB.ReturnInt = -1;
                            resultDB.ReturnString = "SPL Received date (week) Invalid : " + worksheet.Cells[i, 8].Text.NullString().ToUpper();
                            return resultDB;
                        }

                        row["SPLReceivedDateWeek"] = worksheet.Cells[i, 8].Text.NullString().ToUpper();
                        row["Customer"] = worksheet.Cells[i, 9].Text.NullString();
                        row["SETModelCustomer"] = worksheet.Cells[i, 10].Text.NullString();
                        row["ProcessCustomer"] = worksheet.Cells[i, 11].Text.NullString();
                        row["ModelFullname"] = worksheet.Cells[i, 12].Text.NullString();
                        row["DefectNameCus"] = worksheet.Cells[i, 13].Text.NullString();
                        row["DefectRate"] = worksheet.Cells[i, 14].Text.NullString();
                        row["PBA_FAE_Result"] = worksheet.Cells[i, 15].Text.NullString();
                        row["PartsClassification"] = worksheet.Cells[i, 16].Text.NullString();
                        row["PartsClassification2"] = worksheet.Cells[i, 17].Text.NullString();
                        row["ProdutionDateMarking"] = worksheet.Cells[i, 18].Text.NullString();
                        row["AnalysisResult"] = worksheet.Cells[i, 19].Text.NullString();
                        row["VOCCount"] = worksheet.Cells[i, 20].Text.NullString().ToUpper();
                        row["DefectCause"] = worksheet.Cells[i, 21].Text.NullString();
                        row["DefectClassification"] = worksheet.Cells[i, 22].Text.NullString();
                        row["CustomerResponse"] = worksheet.Cells[i, 23].Text.NullString();
                        row["Report_FinalApprover"] = worksheet.Cells[i, 24].Text.NullString();
                        row["Report_Sender"] = worksheet.Cells[i, 25].Text.NullString();
                        row["Rport_sentDate"] = worksheet.Cells[i, 26].Text.NullString();
                        row["VOCState"] = worksheet.Cells[i, 27].Text.NullString();
                        row["VOCFinishingDate"] = worksheet.Cells[i, 28].Text.NullString();
                        row["VOC_TAT"] = worksheet.Cells[i, 29].Text.NullString();
                        table.Rows.Add(row);
                    }

                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add("A_USER", GetUserId());
                    resultDB = _vocRepository.ExecProceduce("PKG_BUSINESS.PUT_VOC", dic, "A_DATA", table);
                }
                return resultDB;
            }
            catch (Exception ex)
            {
                resultDB.ReturnInt = -1;
                resultDB.ReturnString = ex.Message;
                return resultDB;
            }
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(VOC_MSTViewModel model)
        {
            model.UserModified = GetUserId();
            var entity = _mapper.Map<VOC_MST>(model);
            _vocRepository.Update(entity);
        }

        public List<VOC_MSTViewModel> SearchByTime(string fromTime, string toTime)
        {
            var lst = _mapper.Map<List<VOC_MSTViewModel>>(_vocRepository.FindAll(x => string.Compare(x.ReceivedDate, fromTime) >= 0 && string.Compare(x.ReceivedDate, toTime) <= 0).OrderBy(x => x.ReceivedDate));
            var lstType = _mapper.Map<List<VOC_DefectTypeViewModel>>(_vocDefectTypeRepository.FindAll());
            foreach (var item in lst)
            {
                foreach (var type in lstType)
                {
                    if (item.AnalysisResult.NullString().Contains(type.EngsNotation.NullString()))
                    {
                        item.AnalysisResult += " [" + type.KoreanNotation + "]";
                    }
                }
            }
            return lst;
        }

        public List<VOCSiteModelByTimeLst> ReportByWeek(int fromWeek, int toWeek, string year)
        {
            string startTime = year + "-01-01";
            string endTime = DateTime.Parse(startTime).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");
            int fromW = 0;
            int toW = DateTime.Parse(endTime).GetWeekOfYear() - 1;

            List<VOC_MSTViewModel> lstVoc = _mapper.Map<List<VOC_MSTViewModel>>(_vocRepository.FindAll(x => string.Compare(x.ReceivedDate, startTime) >= 0 && string.Compare(x.ReceivedDate, endTime) <= 0).OrderBy(x => x.ReceivedDate));
            lstVoc = lstVoc.FindAll(x => x.VOCCount.NullString().ToUpper() == "Y" && DateTime.Parse(x.SPLReceivedDate.NullOtherString(x.ReceivedDate)).GetWeekOfYear() - 1 >= fromW &&
                                         DateTime.Parse(x.SPLReceivedDate.NullOtherString(x.ReceivedDate)).GetWeekOfYear() - 1 <= toW);

            List<string> Classifications = new List<string>();

            // GET SAW, MODULE,...
            foreach (var item in lstVoc)
            {
                if (!Classifications.Contains(item.PartsClassification))
                {
                    Classifications.Add(item.PartsClassification);
                }
            }

            var groupList = lstVoc.FindAll(x => x.VOCCount.NullString().ToUpper() == "Y").GroupBy(x => (x.PlaceOfOrigin, x.SPLReceivedDateWeek, x.PartsClassification)).Select(gr => (gr, Total: gr.Count()));

            List<VOCSiteModelByTimeLst> results = new List<VOCSiteModelByTimeLst>();
            VOCSiteModelByTimeLst voc = null;

            foreach (var c in Classifications)
            {
                foreach (var group in groupList)
                {
                    if (results.Any(x => x.DivisionLst == group.gr.Key.PlaceOfOrigin))
                    {
                        voc = results.FindLast(x => x.DivisionLst == group.gr.Key.PlaceOfOrigin);
                    }
                    else
                    {
                        voc = new VOCSiteModelByTimeLst();
                        voc.DivisionLst = group.gr.Key.PlaceOfOrigin;
                        voc.PartsClassifications.AddRange(Classifications);
                    }

                    if (voc != null && voc.DivisionLst == group.gr.Key.PlaceOfOrigin) // WHC, WTC, HQ
                    {
                        foreach (var item in group.gr)
                        {
                            if (item.PartsClassification.NullString() == c) // SAW, MODULE
                            {
                                if (!voc.vOCSiteModelByTimes.Any(x => x.Classification == c && x.Time == item.SPLReceivedDateWeek))
                                {
                                    voc.vOCSiteModelByTimes.Add(new VOCSiteModelByTime()
                                    {
                                        Classification = item.PartsClassification,
                                        Qty = group.Total.IfNullIsZero(),
                                        Time = item.SPLReceivedDateWeek
                                    });
                                }
                            }
                        }
                    }

                    if (!results.Any(x => x.DivisionLst == group.gr.Key.PlaceOfOrigin))
                    {
                        results.Add(voc);
                    }
                }
            }

            foreach (var item in results)
            {
                for (int i = fromW; i <= toW; i++)
                {
                    if (!item.TimeHeader.Contains("W" + i))
                    {
                        item.TimeHeader.Add("W" + i);
                    }
                }

                item.TimeHeader.Sort(delegate (string x, string y)
                {
                    if (x == null && y == null) return 0;
                    else if (x == null) return -1;
                    else if (y == null) return 1;
                    else return int.Parse(x.Substring(1)).CompareTo(int.Parse(y.Substring(1)));
                });

                item.vOCSiteModelByTimes.Sort(delegate (VOCSiteModelByTime x, VOCSiteModelByTime y)
                {
                    if (x.Time == null && y.Time == null) return 0;
                    else if (x.Time == null) return -1;
                    else if (y.Time == null) return 1;
                    else return int.Parse(x.Time.Substring(1)).CompareTo(int.Parse(y.Time.Substring(1)));
                });
            }

            foreach (var item in results)
            {
                foreach (var cl in item.PartsClassifications)
                {
                    var qtys = item.vOCSiteModelByTimes.GroupBy(x => x.Classification).Select(gr => (gr, Qty: gr.Sum(x => int.Parse(x.Qty.IfNullIsZero()))));

                    foreach (var q in qtys)
                    {
                        foreach (var k in q.gr)
                        {
                            if (k.Classification == cl)
                            {
                                item.vOCSiteModelByTimes.Insert(0, new VOCSiteModelByTime()
                                {
                                    Time = year + "년 누적",
                                    Classification = cl,
                                    Qty = q.Qty.IfNullIsZero()
                                });
                                break;
                            }
                        }
                    }

                    if (!item.TimeHeader.Contains(year + "년 누적"))
                    {
                        item.TimeHeader.Insert(0, year + "년 누적");
                    }
                }
            }

            List<VOCSiteModelByTime> lstModuleByTime;
            VOCSiteModelByTime vOC;
            foreach (var item in results)
            {
                lstModuleByTime = new List<VOCSiteModelByTime>();
                for (int k = 0; k < item.PartsClassifications.Count; k++)
                {
                    for (int i = 0; i < item.TimeHeader.Count; i++)
                    {
                        vOC = item.vOCSiteModelByTimes.FirstOrDefault(x => x.Classification == item.PartsClassifications[k] && x.Time == item.TimeHeader[i]);
                        if (vOC != null)
                        {
                            lstModuleByTime.Add(vOC);
                        }
                        else
                        {
                            lstModuleByTime.Add(new VOCSiteModelByTime()
                            {
                                Classification = item.PartsClassifications[k],
                                Qty = "0",
                                Time = item.TimeHeader[i]
                            });
                        }
                    }
                }
                item.vOCSiteModelByTimes = lstModuleByTime.FindAll(x => x.Time.NullString().Contains(year) || (int.Parse(x.Time.NullString().Substring(1)) >= fromWeek && int.Parse(x.Time.NullString().Substring(1)) <= toWeek));
            }

            foreach (var item in results)
            {
                item.TimeHeader = item.TimeHeader.FindAll(x => x.NullString().Contains(year) || (int.Parse(x.Substring(1)) >= fromWeek && int.Parse(x.Substring(1)) <= toWeek));
            }

            return results.OrderBy(x => x.DivisionLst).ToList();
        }

        /// <summary>
        /// US calendar
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        //internal int GetWeekOfYear(DateTime date)
        //{
        //    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        //    var calendar = CultureInfo.CurrentCulture.Calendar;
        //    var formatRules = CultureInfo.CurrentCulture.DateTimeFormat;
        //    int week = calendar.GetWeekOfYear(date, formatRules.CalendarWeekRule, formatRules.FirstDayOfWeek);
        //    return week;
        //}

        public List<VOCSiteModelByTimeLst> ReportInit()
        {
            int fromW = DateTime.Now.GetWeekOfYear() - 2 >= 0 ? DateTime.Now.GetWeekOfYear() - 2 : 0;
            int toW = DateTime.Now.GetWeekOfYear() - 1 >= 0 ? DateTime.Now.GetWeekOfYear() - 1 : 0;
            return ReportByWeek(fromW, toW, DateTime.Now.Year.ToString());
        }

        /// <summary>
        /// ▶ 22년 Total VOC 건수그래프 (제품 생산 Site 기준)
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public TotalVOCSiteModel ReportByYear(string year)
        {
            string startTime = year + "-01-01";
            string endTime = DateTime.Parse(startTime).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");

            List<VOC_MSTViewModel> lstVoc = _mapper.Map<List<VOC_MSTViewModel>>(_vocRepository.FindAll(x => string.Compare(x.ReceivedDate, startTime) >= 0 && string.Compare(x.ReceivedDate, endTime) <= 0).OrderBy(x => x.ReceivedDate));

            var groupList = lstVoc.GroupBy(x => (x.PlaceOfOrigin, x.PartsClassification)).Select(gr => (gr, Total: gr.Count()));

            TotalVOCSiteModel totalVOCSite = new TotalVOCSiteModel()
            {
                Year = year
            };

            foreach (var item in lstVoc)
            {
                if (!totalVOCSite.PartsClassification.Contains(item.PartsClassification))
                {
                    totalVOCSite.PartsClassification.Add(item.PartsClassification);
                }

                if (!totalVOCSite.Divisions.Contains(item.PlaceOfOrigin))
                {
                    totalVOCSite.Divisions.Add(item.PlaceOfOrigin);
                }
            }

            TotalVOCSiteModelItem modelItem = null;
            foreach (var group in groupList)
            {
                modelItem = new TotalVOCSiteModelItem()
                {
                    Classification = group.gr.Key.PartsClassification,
                    Division = group.gr.Key.PlaceOfOrigin,
                    Qty = group.Total.IfNullIsZero()
                };

                totalVOCSite.totalVOCSiteModelItems.Add(modelItem);
            }

            foreach (var item in totalVOCSite.Divisions)
            {
                foreach (var sub in totalVOCSite.PartsClassification)
                {
                    if (!totalVOCSite.totalVOCSiteModelItems.Any(x => x.Classification == sub && x.Division == item))
                    {
                        modelItem = new TotalVOCSiteModelItem()
                        {
                            Classification = sub,
                            Division = item,
                            Qty = "0"
                        };

                        totalVOCSite.totalVOCSiteModelItems.Add(modelItem);
                    }
                }
            }

            totalVOCSite.Divisions.Sort();
            totalVOCSite.totalVOCSiteModelItems.Sort(delegate (TotalVOCSiteModelItem x, TotalVOCSiteModelItem y)
            {
                if (x.Division == null && y.Division == null) return 0;
                else if (x.Division == null) return -1;
                else if (y.Division == null) return 1;
                else return x.Division.CompareTo(y.Division);
            });

            return totalVOCSite;
        }

        public List<VOC_DefectTypeViewModel> GetDefectType()
        {
            return _mapper.Map<List<VOC_DefectTypeViewModel>>(_vocDefectTypeRepository.FindAll());
        }
    }
}
