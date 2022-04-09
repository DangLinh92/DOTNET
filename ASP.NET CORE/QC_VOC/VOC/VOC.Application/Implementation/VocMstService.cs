using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
using VOC.Utilities.Constants;
using VOC.Utilities.Dtos;

namespace VOC.Application.Implementation
{
    public class VocMstService : BaseService, IVocMstService
    {
        private IRespository<VOC_MST, int> _vocRepository;
        private IRespository<VOC_DEFECT_TYPE, int> _vocDefectTypeRepository;
        private IRespository<VOC_PPM, int> _vocPPMRepository;
        private IRespository<VOC_PPM_YEAR, int> _vocPPMYearRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VocMstService(IRespository<VOC_MST, int> vocRepository, IRespository<VOC_PPM_YEAR, int> vocPPMYearRepository, IRespository<VOC_PPM, int> vocPPMRepository, IRespository<VOC_DEFECT_TYPE, int> vocDefectTypeRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _vocRepository = vocRepository;
            _vocDefectTypeRepository = vocDefectTypeRepository;
            _vocPPMRepository = vocPPMRepository;
            _vocPPMYearRepository = vocPPMYearRepository;
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

                        if (!DateTime.TryParse(worksheet.Cells[i, 6].Text.NullString(), out _))
                        {
                            resultDB.ReturnInt = -1;
                            resultDB.ReturnString = "Received date is format (YYYY-MM-DD) : " + worksheet.Cells[i, 6].Text.NullString();
                            return resultDB;
                        }

                        row["ReceivedDate"] = worksheet.Cells[i, 6].Text.NullString();
                        row["SPLReceivedDate"] = worksheet.Cells[i, 7].Text.NullString();

                        if (!DateTime.TryParse(worksheet.Cells[i, 7].Text.NullString(), out _))
                        {
                            row["SPLReceivedDateWeek"] = DateTime.Parse(worksheet.Cells[i, 6].Text.NullString()).GetWeekOfYear() - 1;
                        }
                        else
                        {
                            if (!int.TryParse(worksheet.Cells[i, 8].Text.NullString().ToUpper().Substring(1), out _))
                            {
                                resultDB.ReturnInt = -1;
                                resultDB.ReturnString = "SPL Received date (week) Invalid : " + worksheet.Cells[i, 8].Text.NullString().ToUpper();
                                return resultDB;
                            }

                            row["SPLReceivedDateWeek"] = worksheet.Cells[i, 8].Text.NullString().ToUpper();
                        }


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
            return lst;
        }

        public List<VOCSiteModelByTimeLst> ReportByMonth(string year, string customer, string side)
        {
            string startTime = year + "-01-01";
            string endTime = DateTime.Parse(startTime).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");

            List<VOC_MSTViewModel> lstVoc = _mapper.Map<List<VOC_MSTViewModel>>(
                _vocRepository.FindAll(x => x.VOCCount == "Y" &&
                string.Compare(x.ReceivedDate, startTime) >= 0 &&
                string.Compare(x.ReceivedDate, endTime) <= 0).OrderBy(x => x.ReceivedDate));

            if (!string.IsNullOrEmpty(customer))
            {
                lstVoc = lstVoc.Where(x => x.Customer == customer).ToList();
            }

            if (string.IsNullOrEmpty(side))
            {
                lstVoc = lstVoc.Where(x => x.PlaceOfOrigin == CommonConstants.WHC).ToList();
            }
            else
            {
                lstVoc = lstVoc.Where(x => x.PlaceOfOrigin == side).ToList();
            }

            List<string> Classifications = new List<string>();

            // GET SAW, MODULE,...
            foreach (var item in lstVoc)
            {
                if (!Classifications.Contains(item.PartsClassification.NullString()))
                {
                    Classifications.Add(item.PartsClassification.NullString());
                }
            }

            var groupList = lstVoc.GroupBy(x => (x.PlaceOfOrigin, x.SPLReceivedDateMonth, x.PartsClassification)).Select(gr => (gr, Total: gr.Count()));

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
                                if (!voc.vOCSiteModelByTimes.Any(x => x.Classification == c && x.Time == item.SPLReceivedDateMonth))
                                {
                                    voc.vOCSiteModelByTimes.Add(new VOCSiteModelByTime()
                                    {
                                        Classification = item.PartsClassification,
                                        Qty = group.Total.IfNullIsZero(),
                                        Time = item.SPLReceivedDateMonth
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
                for (int i = 0; i < 12; i++)
                {
                    if (!item.TimeHeader.Contains(DateTime.Parse(startTime).AddMonths(i).ToString("yyMM")))
                    {
                        item.TimeHeader.Add(DateTime.Parse(startTime).AddMonths(i).ToString("yyMM"));
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

                item.vOCSiteModelByTimes = lstModuleByTime;
            }

            return results.FindAll(x => x.DivisionLst == side).ToList();
        }

        #region report by week
        //public List<VOCSiteModelByTimeLst> ReportByWeek(int fromWeek, int toWeek, string year)
        //{
        //    string startTime = year + "-01-01";
        //    string endTime = DateTime.Parse(startTime).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");
        //    int fromW = 0;
        //    int toW = DateTime.Parse(endTime).GetWeekOfYear() - 1;

        //    List<VOC_MSTViewModel> lstVoc = _mapper.Map<List<VOC_MSTViewModel>>(_vocRepository.FindAll(x => string.Compare(x.ReceivedDate, startTime) >= 0 && string.Compare(x.ReceivedDate, endTime) <= 0).OrderBy(x => x.ReceivedDate));
        //    lstVoc = lstVoc.FindAll(x => x.VOCCount.NullString().ToUpper() == "Y" && DateTime.Parse(x.SPLReceivedDate.NullOtherString(x.ReceivedDate)).GetWeekOfYear() - 1 >= fromW &&
        //                                 DateTime.Parse(x.SPLReceivedDate.NullOtherString(x.ReceivedDate)).GetWeekOfYear() - 1 <= toW);

        //    List<string> Classifications = new List<string>();

        //    // GET SAW, MODULE,...
        //    foreach (var item in lstVoc)
        //    {
        //        if (!Classifications.Contains(item.PartsClassification))
        //        {
        //            Classifications.Add(item.PartsClassification);
        //        }
        //    }

        //    var groupList = lstVoc.FindAll(x => x.VOCCount.NullString().ToUpper() == "Y").GroupBy(x => (x.PlaceOfOrigin, x.SPLReceivedDateWeek, x.PartsClassification)).Select(gr => (gr, Total: gr.Count()));

        //    List<VOCSiteModelByTimeLst> results = new List<VOCSiteModelByTimeLst>();
        //    VOCSiteModelByTimeLst voc = null;

        //    foreach (var c in Classifications)
        //    {
        //        foreach (var group in groupList)
        //        {
        //            if (results.Any(x => x.DivisionLst == group.gr.Key.PlaceOfOrigin))
        //            {
        //                voc = results.FindLast(x => x.DivisionLst == group.gr.Key.PlaceOfOrigin);
        //            }
        //            else
        //            {
        //                voc = new VOCSiteModelByTimeLst();
        //                voc.DivisionLst = group.gr.Key.PlaceOfOrigin;
        //                voc.PartsClassifications.AddRange(Classifications);
        //            }

        //            if (voc != null && voc.DivisionLst == group.gr.Key.PlaceOfOrigin) // WHC, WTC, HQ
        //            {
        //                foreach (var item in group.gr)
        //                {
        //                    if (item.PartsClassification.NullString() == c) // SAW, MODULE
        //                    {
        //                        if (!voc.vOCSiteModelByTimes.Any(x => x.Classification == c && x.Time == item.SPLReceivedDateWeek))
        //                        {
        //                            voc.vOCSiteModelByTimes.Add(new VOCSiteModelByTime()
        //                            {
        //                                Classification = item.PartsClassification,
        //                                Qty = group.Total.IfNullIsZero(),
        //                                Time = item.SPLReceivedDateWeek
        //                            });
        //                        }
        //                    }
        //                }
        //            }

        //            if (!results.Any(x => x.DivisionLst == group.gr.Key.PlaceOfOrigin))
        //            {
        //                results.Add(voc);
        //            }
        //        }
        //    }

        //    foreach (var item in results)
        //    {
        //        for (int i = fromW; i <= toW; i++)
        //        {
        //            if (!item.TimeHeader.Contains("W" + i))
        //            {
        //                item.TimeHeader.Add("W" + i);
        //            }
        //        }

        //        item.TimeHeader.Sort(delegate (string x, string y)
        //        {
        //            if (x == null && y == null) return 0;
        //            else if (x == null) return -1;
        //            else if (y == null) return 1;
        //            else return int.Parse(x.Substring(1)).CompareTo(int.Parse(y.Substring(1)));
        //        });

        //        item.vOCSiteModelByTimes.Sort(delegate (VOCSiteModelByTime x, VOCSiteModelByTime y)
        //        {
        //            if (x.Time == null && y.Time == null) return 0;
        //            else if (x.Time == null) return -1;
        //            else if (y.Time == null) return 1;
        //            else return int.Parse(x.Time.Substring(1)).CompareTo(int.Parse(y.Time.Substring(1)));
        //        });
        //    }

        //    foreach (var item in results)
        //    {
        //        foreach (var cl in item.PartsClassifications)
        //        {
        //            var qtys = item.vOCSiteModelByTimes.GroupBy(x => x.Classification).Select(gr => (gr, Qty: gr.Sum(x => int.Parse(x.Qty.IfNullIsZero()))));

        //            foreach (var q in qtys)
        //            {
        //                foreach (var k in q.gr)
        //                {
        //                    if (k.Classification == cl)
        //                    {
        //                        item.vOCSiteModelByTimes.Insert(0, new VOCSiteModelByTime()
        //                        {
        //                            Time = year + "년 누적",
        //                            Classification = cl,
        //                            Qty = q.Qty.IfNullIsZero()
        //                        });
        //                        break;
        //                    }
        //                }
        //            }

        //            if (!item.TimeHeader.Contains(year + "년 누적"))
        //            {
        //                item.TimeHeader.Insert(0, year + "년 누적");
        //            }
        //        }
        //    }

        //    List<VOCSiteModelByTime> lstModuleByTime;
        //    VOCSiteModelByTime vOC;
        //    foreach (var item in results)
        //    {
        //        lstModuleByTime = new List<VOCSiteModelByTime>();
        //        for (int k = 0; k < item.PartsClassifications.Count; k++)
        //        {
        //            for (int i = 0; i < item.TimeHeader.Count; i++)
        //            {
        //                vOC = item.vOCSiteModelByTimes.FirstOrDefault(x => x.Classification == item.PartsClassifications[k] && x.Time == item.TimeHeader[i]);
        //                if (vOC != null)
        //                {
        //                    lstModuleByTime.Add(vOC);
        //                }
        //                else
        //                {
        //                    lstModuleByTime.Add(new VOCSiteModelByTime()
        //                    {
        //                        Classification = item.PartsClassifications[k],
        //                        Qty = "0",
        //                        Time = item.TimeHeader[i]
        //                    });
        //                }
        //            }
        //        }
        //        item.vOCSiteModelByTimes = lstModuleByTime.FindAll(x => x.Time.NullString().Contains(year) || (int.Parse(x.Time.NullString().Substring(1)) >= fromWeek && int.Parse(x.Time.NullString().Substring(1)) <= toWeek));
        //    }

        //    foreach (var item in results)
        //    {
        //        item.TimeHeader = item.TimeHeader.FindAll(x => x.NullString().Contains(year) || (int.Parse(x.Substring(1)) >= fromWeek && int.Parse(x.Substring(1)) <= toWeek));
        //    }

        //    return results.OrderBy(x => x.DivisionLst).ToList();
        //}
        #endregion

        public List<VOCSiteModelByTimeLst> ReportInit()
        {
            return ReportByMonth(DateTime.Now.Year.ToString(), "", CommonConstants.WHC);
        }

        /// <summary>
        /// ▶ 22년 Total VOC 건수그래프 (제품 생산 Site 기준)
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public TotalVOCSiteModel ReportByYear(string year, string customer)
        {
            string startTime = year + "-01-01";
            string endTime = DateTime.Parse(startTime).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");

            List<VOC_MSTViewModel> lstVoc = _mapper.Map<List<VOC_MSTViewModel>>(_vocRepository.FindAll(x => string.Compare(x.ReceivedDate, startTime) >= 0 && string.Compare(x.ReceivedDate, endTime) <= 0).OrderBy(x => x.ReceivedDate));

            if (!string.IsNullOrEmpty(customer))
            {
                lstVoc = lstVoc.Where(x => x.Customer == customer).ToList();
            }

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

        /// <summary>
        /// Ve bieu do pareto theo defect 
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<TotalVOCSiteModel> ReportDefectByYear(string year, string classification, string customer, string side)
        {
            string startTime = year + "-01-01";
            string endTime = DateTime.Parse(startTime).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");

            List<VOC_MSTViewModel> lstVoc = _mapper.Map<List<VOC_MSTViewModel>>(
                _vocRepository.FindAll(x => x.VOCCount == "Y" && x.PartsClassification == classification &&
                 string.Compare(x.ReceivedDate, startTime) >= 0 && string.Compare(x.ReceivedDate, endTime) <= 0).OrderBy(x => x.ReceivedDate));

            if (!string.IsNullOrEmpty(customer))
            {
                lstVoc = lstVoc.Where(x => x.Customer == customer).ToList();
            }

            var groupList = lstVoc.GroupBy(x => (x.PlaceOfOrigin, x.AnalysisResult)).Select(gr => (gr, Total: gr.Count()));

            TotalVOCSiteModel totalVOCSite = new TotalVOCSiteModel()
            {
                Year = year
            };

            foreach (var item in lstVoc)
            {
                if (!totalVOCSite.PartsClassification.Contains(item.AnalysisResult))
                {
                    totalVOCSite.PartsClassification.Add(item.AnalysisResult);
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
                    Classification = group.gr.Key.AnalysisResult,
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

            totalVOCSite.totalVOCSiteModelItems.Sort(delegate (TotalVOCSiteModelItem x, TotalVOCSiteModelItem y)
            {
                int dv = string.Compare(x.Division, y.Division);

                if (dv != 0)
                {
                    return dv;
                }

                if (x.Qty == null && y.Qty == null) return 0;
                else if (x.Qty == null) return 1;
                else if (y.Qty == null) return -1;
                else return -int.Parse(x.Qty).CompareTo(int.Parse(y.Qty));
            });

            List<TotalVOCSiteModel> result = new List<TotalVOCSiteModel>();
            TotalVOCSiteModel totalVOC = new TotalVOCSiteModel()
            {
                Year = year
            };

            foreach (var div in totalVOCSite.Divisions)
            {
                if (div == side)
                {
                    totalVOC = new TotalVOCSiteModel()
                    {
                        Year = year
                    };
                    totalVOC.Divisions.Add(div);

                    foreach (var sub in totalVOCSite.totalVOCSiteModelItems)
                    {
                        if (sub.Division == div && int.Parse(sub.Qty.IfNullIsZero()) > 0)
                        {
                            totalVOC.totalVOCSiteModelItems.Add(sub);
                            if (!totalVOC.PartsClassification.Contains(sub.Classification))
                            {
                                totalVOC.PartsClassification.Add(sub.Classification);
                            }

                        }
                    }

                    result.Add(totalVOC);
                }
            }

            return result;
        }

        public PPMDataChartAll ReportPPMByYear(string year, out List<VOCPPM_Ex> pMDataCharts)
        {
            int iyear = int.Parse(year);

            List<VOCPPM_Ex> lstVocPPMex = new List<VOCPPM_Ex>();

            var lst = _vocPPMRepository.FindAll(x => x.Year == iyear);
            var lstPPM = _mapper.Map<List<VocPPMViewModel>>(lst);
            var lstGroup = lstPPM.GroupBy(x => (x.Module, x.Customer)).Select(gr => (gr, Total: gr.Count()));
            VOCPPM_Ex vOCPPM_Ex = new VOCPPM_Ex();
            VOCPPM_Customer vOCPPM_Customer;
            foreach (var group in lstGroup)
            {
                if (!lstVocPPMex.Any(x => x.Module == group.gr.Key.Module))
                {
                    vOCPPM_Ex = new VOCPPM_Ex()
                    {
                        Module = group.gr.Key.Module, // csp, lfem
                        Year = year
                    };
                }
                else
                {
                    vOCPPM_Ex = lstVocPPMex.FirstOrDefault(x => x.Module == group.gr.Key.Module);
                }

                foreach (var item in group.gr)
                {
                    if (!vOCPPM_Ex.vOCPPM_Customers.Any(x => x.Customer == item.Customer))
                    {
                        vOCPPM_Customer = new VOCPPM_Customer()
                        {
                            Customer = item.Customer,
                        };
                    }
                    else
                    {
                        vOCPPM_Customer = vOCPPM_Ex.vOCPPM_Customers.FirstOrDefault(x => x.Customer == item.Customer);
                    }

                    vOCPPM_Customer.vocPPMModels.Add(item);
                    var lstGr = vOCPPM_Customer.vocPPMModels.GroupBy(x => x.Type).Select(gr => (gr, ToTal: gr.Sum(g => g.Value)));
                    foreach (var subGr in lstGr)
                    {
                        if (subGr.gr.Key == "Input")
                        {
                            vOCPPM_Customer.ToTal_Input = subGr.ToTal;
                        }
                        else
                        {
                            vOCPPM_Customer.ToTal_Defect = subGr.ToTal;
                        }
                    }
                    if (vOCPPM_Customer.ToTal_Input > 0)
                    {
                        vOCPPM_Customer.ToTal_PPM = Math.Round((vOCPPM_Customer.ToTal_Defect / vOCPPM_Customer.ToTal_Input) * Math.Pow(10, 6), 0);
                    }
                    else
                    {
                        vOCPPM_Customer.ToTal_PPM = 0;
                    }

                    vOCPPM_Customer.ToTal_PPM_Target = item.TargetValue;

                    if (!vOCPPM_Ex.vOCPPM_Customers.Any(x => x.Customer == item.Customer))
                    {
                        vOCPPM_Ex.vOCPPM_Customers.Add(vOCPPM_Customer);
                    }
                }

                if (!lstVocPPMex.Any(x => x.Module == vOCPPM_Ex.Module))
                {
                    lstVocPPMex.Add(vOCPPM_Ex);
                }
            }

            VocPPMViewModel vocPPM;
            List<string> types;
            lstVocPPMex.Sort(delegate (VOCPPM_Ex x, VOCPPM_Ex y)
            {

                if (x.Module == null && y.Module == null) return 0;
                else if (x.Module == null) return -1;
                else if (y.Module == null) return 1;
                else return x.Module.CompareTo(y.Module);
            });

            foreach (var item in lstVocPPMex)
            {
                item.vOCPPM_Customers.Sort(delegate (VOCPPM_Customer x, VOCPPM_Customer y)
                {

                    if (x.Customer == null && y.Customer == null) return 0;
                    else if (x.Customer == null) return -1;
                    else if (y.Customer == null) return 1;
                    else return x.Customer.CompareTo(y.Customer);
                });

                foreach (var sub in item.vOCPPM_Customers)
                {
                    types = new List<string>();
                    types.AddRange(sub.vocPPMModels.Select(x => x.Type).Distinct());

                    for (int month = 1; month <= 12; month++)
                    {
                        foreach (var type in types)
                        {
                            if (!sub.vocPPMModels.Any(x => x.Month == month && x.Type == type))
                            {
                                vocPPM = new VocPPMViewModel();
                                vocPPM.CopyPropertiesFrom(sub.vocPPMModels.FirstOrDefault(x => x.Type == type), new List<string>() { });
                                vocPPM.Value = 0;
                                vocPPM.Month = month;
                                vocPPM.Year = int.Parse(item.Year.IfNullIsZero());
                                sub.vocPPMModels.Add(vocPPM);
                            }
                        }

                    }

                    sub.vocPPMModels.Sort(delegate (VocPPMViewModel x, VocPPMViewModel y)
                    {
                        return x.Month.CompareTo(y.Month);
                    });
                }
            }

            PPMByMonth pPMByMonth;
            foreach (var item in lstVocPPMex)
            {
                foreach (var sub in item.vOCPPM_Customers)
                {
                    sub.pPMByMonths.Clear();

                    for (int month = 1; month <= 12; month++)
                    {
                        pPMByMonth = new PPMByMonth()
                        {
                            Month = month,
                            Year = int.Parse(item.Year.IfNullIsZero()),
                            Target = sub.ToTal_PPM_Target,
                        };

                        if (sub.vocPPMModels.FirstOrDefault(x => x.Month == month && x.Type == "Input")?.Value > 0)
                        {
                            pPMByMonth.PPM = Math.Round(Math.Pow(10, 6) * sub.vocPPMModels.FirstOrDefault(x => x.Month == month && x.Type == "Defect").Value / sub.vocPPMModels.FirstOrDefault(x => x.Month == month && x.Type == "Input").Value, 0);
                        }
                        else
                        {
                            pPMByMonth.PPM = 0;
                        }

                        sub.pPMByMonths.Add(pPMByMonth);
                    }
                    pPMByMonth = new PPMByMonth()
                    {
                        Month = 0,// TOTAL
                        Year = int.Parse(item.Year.IfNullIsZero()),
                        Target = sub.ToTal_PPM_Target,
                        PPM = sub.ToTal_PPM
                    };
                    sub.pPMByMonths.Insert(0, pPMByMonth);
                }
            }

            pMDataCharts = lstVocPPMex;

            Dictionary<string, List<PPMDataChart>> dic = new Dictionary<string, List<PPMDataChart>>();
            PPMDataChart pPMDataChart;
            List<PPMDataChart> pPMDataCharts;
            List<double> lstTotalInput = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<double> lstTotalDefect = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<double> lstTotalPPM = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<double> dataTargetAll = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            double totalPPM;
            string customer = "";
            foreach (var item in lstVocPPMex)
            {
                customer = "";
                pPMDataCharts = new List<PPMDataChart>();

                lstTotalInput = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                lstTotalDefect = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                lstTotalPPM = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                dataTargetAll = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                foreach (var cus in item.vOCPPM_Customers)
                {
                    foreach (var pM in cus.pPMByMonths)
                    {
                        if (pM.Month >= 0 && pM.Month <= 12)
                        {
                            dataTargetAll[pM.Month] = pM.Target;
                        }
                    }
                    break;
                }

                foreach (var cus in item.vOCPPM_Customers)
                {
                    customer += cus.Customer + "+";
                    pPMDataChart = new PPMDataChart()
                    {
                        Module = item.Module,
                        Customer = cus.Customer,
                        Year = iyear,
                        dataTargetAll = dataTargetAll
                    };
                    pPMDataChart.lstData = cus.pPMByMonths.Select(x => x.PPM).ToList();
                    pPMDataCharts.Add(pPMDataChart);

                    for (int month = 1; month <= 12; month++)
                    {
                        if (cus.vocPPMModels.FirstOrDefault(x => x.Type == "Input" && x.Month == month) != null)
                        {
                            lstTotalInput[month - 1] += cus.vocPPMModels.FirstOrDefault(x => x.Type == "Input" && x.Month == month).Value;
                        }
                        else
                        {
                            lstTotalInput[month - 1] += 0;
                        }

                        if (cus.vocPPMModels.FirstOrDefault(x => x.Type == "Defect" && x.Month == month) != null)
                        {
                            lstTotalDefect[month - 1] += cus.vocPPMModels.FirstOrDefault(x => x.Type == "Defect" && x.Month == month).Value;
                        }
                        else
                        {
                            lstTotalDefect[month - 1] += 0;
                        }
                    }
                }

                if (lstTotalInput.Sum() > 0)
                {
                    totalPPM = Math.Round(Math.Pow(10, 6) * (lstTotalDefect.Sum() / lstTotalInput.Sum()), 0);
                }
                else
                {
                    totalPPM = 0;
                }
                lstTotalPPM[0] = totalPPM;
                for (int i = 1; i <= 12; i++)
                {
                    lstTotalPPM[i] = lstTotalInput[i - 1] > 0 ? Math.Round(Math.Pow(10, 6) * (lstTotalDefect[i - 1] / lstTotalInput[i - 1]), 0) : 0;
                }

                pPMDataChart = new PPMDataChart()
                {
                    Module = item.Module,
                    Customer = customer.Substring(0, customer.Length - 1),
                    lstData = lstTotalPPM,
                    Year = iyear,
                    dataTargetAll = dataTargetAll
                };
                pPMDataCharts.Insert(0, pPMDataChart);

                dic.Add(item.Module, pPMDataCharts);
            }

            List<List<PPMDataChart>> result = new List<List<PPMDataChart>>();
            List<PPMDataChart> lstChartAll = new List<PPMDataChart>();
            PPMDataChart pPMData;
            foreach (var item in dic.Values)
            {
                result.Add(item);
                foreach (var sub in item)
                {
                    if (sub.Customer != null && sub.Customer.Contains("+"))
                    {
                        pPMData = new PPMDataChart()
                        {
                            Customer = sub.Customer,
                            Module = sub.Module,
                            Year = sub.Year,
                            dataTargetAll = sub.dataTargetAll.ToList(),
                            lstData = sub.lstData.ToList()
                        };
                        lstChartAll.Add(pPMData);
                    }
                }
            }

            PPMDataChartAll pPMDataChartAll = new PPMDataChartAll()
            {
                dataChartsAll = lstChartAll,
                dataChartsItem = result,
                Year = iyear
            };

            VOC_PPM_YEAR entity;
            if (year == DateTime.Now.Year.ToString())
            {
                foreach (var item in pPMDataChartAll.dataChartsAll)
                {
                    if (_vocPPMYearRepository.FindAll(x => x.Year == iyear && x.Module == item.Module).Any())
                    {
                        entity = _vocPPMYearRepository.FindAll(x => x.Year == iyear && x.Module == item.Module).FirstOrDefault();
                        entity.ValuePPM = item.lstData[0];
                        _vocPPMYearRepository.Update(entity);
                    }
                    else
                    {
                        entity = new VOC_PPM_YEAR()
                        {
                            Year = int.Parse(year),
                            Module = item.Module,
                            ValuePPM = item.lstData[0]
                        };
                        _vocPPMYearRepository.Add(entity);
                    }
                }

                _unitOfWork.Commit();
            }

            foreach (var item in pPMDataChartAll.dataChartsAll)
            {
                var lstPPMYear = _vocPPMYearRepository.FindAll(x => x.Year >= iyear - 3 && x.Module == item.Module && x.Year < iyear).OrderByDescending(x => x.Year);

                foreach (var sub in lstPPMYear)
                {
                    item.dataTargetAll.Insert(0, sub.TargetPPM);
                    item.lstData.Insert(0, sub.ValuePPM);
                }
            }

            return pPMDataChartAll;
        }

        public GmesDataViewModel GetGmesData()
        {
            GmesDataViewModel gmes = new GmesDataViewModel();
            gmes.vocPPMViewModels = _mapper.Map<List<VocPPMViewModel>>(_vocPPMRepository.FindAll().OrderByDescending(x => x.Year).OrderByDescending(x => x.Month));
            gmes.vocPPMYearViewModels = _mapper.Map<List<VocPPMYearViewModel>>(_vocPPMYearRepository.FindAll().OrderByDescending(x => x.Year));
            return gmes;
        }

        public VocPPMYearViewModel UpdatePPMByYear(bool isAdd, VocPPMYearViewModel model)
        {
            if (isAdd)
            {
                var entity = _mapper.Map<VOC_PPM_YEAR>(model);
                entity.UserCreated = GetUserId();
                _vocPPMYearRepository.Add(entity);
            }
            else
            {
                var entity = _vocPPMYearRepository.FindById(model.Id);
                entity.CopyPropertiesFrom(model, new List<string>() { "Id", "DateCreated", "DateModified", "UserCreated", "UserModified" });
                entity.UserModified = GetUserId();
                _vocPPMYearRepository.Update(entity);
            }

            return model;
        }

        public void DeletePPMByYear(int Id)
        {
            _vocPPMYearRepository.Remove(Id);
        }

        public VocPPMViewModel UpdatePPMByYearMonth(bool isAdd, VocPPMViewModel model)
        {
            if (isAdd)
            {
                var entity = _mapper.Map<VOC_PPM>(model);
                entity.UserCreated = GetUserId();
                _vocPPMRepository.Add(entity);
            }
            else
            {
                var entity = _vocPPMRepository.FindById(model.Id);
                entity.CopyPropertiesFrom(model, new List<string>() { "Id", "DateCreated", "DateModified", "UserCreated", "UserModified" });
                entity.UserModified = GetUserId();
                _vocPPMRepository.Update(entity);
            }
            return model;
        }

        public void DeletePPMByYearMonth(int Id)
        {
            _vocPPMRepository.Remove(Id);
        }

        public VocPPMYearViewModel GetPPMByYear(int id)
        {
            return _mapper.Map<VocPPMYearViewModel>(_vocPPMYearRepository.FindById(id));
        }

        public VocPPMViewModel GetPPMByYearMonth(int id)
        {
            return _mapper.Map<VocPPMViewModel>(_vocPPMRepository.FindById(id));
        }

        public double GetTargetPPMByYear(int year)
        {
            var entity = _vocPPMYearRepository.FindAll(x => x.Year == year).FirstOrDefault();
            if (entity != null)
            {
                return entity.TargetPPM;
            }
            return 0;
        }

        public VocProgessInfo GetProgressInfo(int year, string customer, string side)
        {
            string startTime = year + "-01-01";
            string endTime = DateTime.Parse(startTime).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");

            List<VOC_MSTViewModel> lstVoc = _mapper.Map<List<VOC_MSTViewModel>>(_vocRepository.FindAll(x => string.Compare(x.ReceivedDate, startTime) >= 0 && string.Compare(x.ReceivedDate, endTime) <= 0).OrderBy(x => x.ReceivedDate));

            if (!string.IsNullOrEmpty(customer))
            {
                lstVoc = lstVoc.Where(x => x.Customer == customer).ToList();
            }

            if (string.IsNullOrEmpty(side))
            {
                lstVoc = lstVoc.Where(x => x.PlaceOfOrigin == CommonConstants.WHC).ToList();
            }
            else
            {
                lstVoc = lstVoc.Where(x => x.PlaceOfOrigin == side).ToList();
            }

            VocProgessInfo info = new VocProgessInfo();
            info.ReceiveCount = lstVoc.Count();
            info.CloseCount = lstVoc.Where(x => x.VOCState == "Close").Count();
            info.ProgressCount = info.ReceiveCount - info.CloseCount;
            return info;
        }

        public List<string> GetCustomer()
        {
            List<string> lst = _vocRepository.FindAll(x => x.Customer != "").Select(x => x.Customer).Distinct().ToList();
            return lst;
        }
    }
}
