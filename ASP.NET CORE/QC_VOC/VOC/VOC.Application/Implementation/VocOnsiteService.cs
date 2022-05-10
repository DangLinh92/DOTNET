using AutoMapper;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using VOC.Application.Interfaces;
using VOC.Application.ViewModels.VOC;
using VOC.Data.EF.Extensions;
using VOC.Data.Entities;
using VOC.Infrastructure.Interfaces;
using VOC.Utilities.Constants;
using VOC.Utilities.Dtos;

namespace VOC.Application.Implementation
{
    public class VocOnsiteService : BaseService, IVocOnsiteSevice
    {
        private IRespository<VOC_ONSITE, int> _vocRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VocOnsiteService(IRespository<VOC_ONSITE, int> vocRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _vocRepository = vocRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<VocOnsiteViewModel> GetAllOnsiteByTime(string fromTime, string toTime)
        {
            return _mapper.Map<List<VocOnsiteViewModel>>(_vocRepository.FindAll(x => string.Compare(x.Date, fromTime) >= 0 && string.Compare(x.Date, toTime) <= 0).OrderByDescending(x => x.DateModified));
        }

        public List<VocOnsiteModel> SumDataOnsite(int year, string customer, string part)
        {
            List<VocOnsiteViewModel> lstOnsite;
            List<VocOnsiteModel> lstVocOnsite = new List<VocOnsiteModel>();
            if (year > 0)
            {
                string _customer = customer;

                if (customer.Contains("result"))
                {
                    _customer = customer.Split('-')[0];
                }
                else if (customer.EndsWith("ALL"))
                {
                    _customer = customer.Split('_')[0];
                }

                if (part == CommonConstants.ALL)
                {
                    lstOnsite = _mapper.Map<List<VocOnsiteViewModel>>(_vocRepository.FindAll(x => x.Date.StartsWith(year + "") && x.Customer == _customer).ToList());
                }
                else
                {
                    lstOnsite = _mapper.Map<List<VocOnsiteViewModel>>(_vocRepository.FindAll(x => x.Date.StartsWith(year + "") && x.Part == part && x.Customer == _customer).ToList());
                }

                if (customer.Contains("result") || customer.EndsWith("ALL"))
                {
                    var lstGroup = lstOnsite.GroupBy(x => (x.Customer, x.Date, x.Week, x.Month, x.Part)).Select(gr => (gr, Total: gr.Sum(x => x.Qty)));

                    VocOnsiteModel vocOnsite;
                    List<VocOnsiteViewModel> itemOnsites;
                    foreach (var item in lstGroup)
                    {
                        vocOnsite = new VocOnsiteModel()
                        {
                            Customer = item.gr.Key.Customer,
                            Date = item.gr.Key.Date,
                            Week = item.gr.Key.Week,
                            Month = item.gr.Key.Month,
                            Part = item.gr.Key.Part,
                            Qty = item.Total
                        };

                        itemOnsites = new List<VocOnsiteViewModel>();
                        foreach (var onsite in item.gr)
                        {
                            if (onsite.Result == "OK")
                            {
                                vocOnsite.OK += onsite.Qty;
                            }
                            else if (onsite.Result == "NG")
                            {
                                vocOnsite.NG += onsite.Qty;
                            }
                            itemOnsites.Add(onsite);
                        }

                        vocOnsite.lstVocOnsite.AddRange(itemOnsites.OrderBy(x => x.ProductionDate));
                        lstVocOnsite.Add(vocOnsite);
                    }
                }
                else
                {
                    var lstGroup = lstOnsite.GroupBy(x => (x.Customer, x.Date, x.Week, x.Month, x.Part, x.Customer_Code, x.Wisol_Model)).Select(gr => (gr, Total: gr.Sum(x => x.Qty)));

                    VocOnsiteModel vocOnsite;
                    List<VocOnsiteViewModel> itemOnsites;
                    foreach (var item in lstGroup)
                    {
                        vocOnsite = new VocOnsiteModel()
                        {
                            Customer = item.gr.Key.Customer,
                            Date = item.gr.Key.Date,
                            Week = item.gr.Key.Week,
                            Month = item.gr.Key.Month,
                            Part = item.gr.Key.Part,
                            Qty = item.Total,
                            Wisol_Model = item.gr.Key.Wisol_Model,
                            Customer_Code = item.gr.Key.Customer_Code,
                            OK = 0,
                            NG = 0
                        };

                        itemOnsites = new List<VocOnsiteViewModel>();
                        foreach (var onsite in item.gr)
                        {
                            itemOnsites.Add(onsite);
                            if (onsite.Result == "OK")
                            {
                                vocOnsite.OK += onsite.Qty;
                            }
                            else if (onsite.Result == "NG")
                            {
                                vocOnsite.NG += onsite.Qty;
                            }
                        }

                        vocOnsite.lstVocOnsite.AddRange(itemOnsites.OrderBy(x => x.ProductionDate));
                        lstVocOnsite.Add(vocOnsite);
                    }
                }
            }
            return lstVocOnsite.OrderBy(x => x.Date).ToList();
        }

        public VocOnsiteViewModel UpdateVocOnsite(VocOnsiteViewModel vm)
        {
            VOC_ONSITE entity = _mapper.Map<VOC_ONSITE>(vm);
            entity.UserModified = GetUserId();
            entity.Month = DateTime.Parse(vm.Date).Month;
            int w = (DateTime.Parse(vm.Date).GetWeekOfYear() - 1);
            entity.Week = "W" + (w <= 9 ? "0" + w : "" + w);

            if (entity.Result == "OK")
            {
                entity.OK = "OK";
                entity.NG = "";
                entity.Not_Measure = "";
            }
            else if (entity.Result == "NG")
            {
                entity.NG = "NG";
                entity.OK = "";
                entity.Not_Measure = "";
            }
            else
            {
                entity.Not_Measure = "NM";
                entity.NG = "";
                entity.OK = "";
            }

            _vocRepository.Update(entity);
            return vm;
        }

        public VocOnsiteViewModel AddVocOnsite(VocOnsiteViewModel vm)
        {
            VOC_ONSITE entity = _mapper.Map<VOC_ONSITE>(vm);
            entity.UserModified = GetUserId();
            entity.UserCreated = GetUserId();
            entity.Month = DateTime.Parse(vm.Date).Month;
            int w = (DateTime.Parse(vm.Date).GetWeekOfYear() - 1);
            entity.Week = "W" + (w <= 9 ? "0" + w : "" + w);

            if (entity.Result == "OK")
            {
                entity.OK = "OK";
                entity.NG = "";
                entity.Not_Measure = "";
            }
            else if (entity.Result == "NG")
            {
                entity.NG = "NG";
                entity.OK = "";
                entity.Not_Measure = "";
            }
            else
            {
                entity.Not_Measure = "NM";
                entity.NG = "";
                entity.OK = "";
            }

            _vocRepository.Add(entity);
            return vm;
        }

        public void DeleteVocOnsite(int id)
        {
            _vocRepository.Remove(id);
        }

        public VocOnsiteViewModel FindById(int id)
        {
            return _mapper.Map<VocOnsiteViewModel>(_vocRepository.FindById(id));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public ResultDB ImportExcel(string filePath, string param)
        {
            ResultDB resultDB = new ResultDB();
            try
            {
                using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = packet.Workbook.Worksheets[1];

                    DataTable table = new DataTable();
                    table.Columns.Add("Month");
                    table.Columns.Add("Week");
                    table.Columns.Add("Date");
                    table.Columns.Add("Customer");
                    table.Columns.Add("Part");
                    table.Columns.Add("Qty");
                    table.Columns.Add("Wisol_Model");
                    table.Columns.Add("Customer_Code");
                    table.Columns.Add("Marking");
                    table.Columns.Add("ProductionDate");
                    table.Columns.Add("SetModel");
                    table.Columns.Add("Result");
                    table.Columns.Add("Note");
                    table.Columns.Add("OK");
                    table.Columns.Add("NG");
                    table.Columns.Add("Not_Measure");

                    DataRow row = null;
                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++) // Start.Row = 1
                    {
                        row = table.NewRow();

                        if (worksheet.Cells[i, 1].Text.NullString() == "" ||
                            worksheet.Cells[i, 2].Text.NullString() == "" ||
                            worksheet.Cells[i, 3].Text.NullString() == "" ||
                            worksheet.Cells[i, 4].Text.NullString() == "" ||
                            worksheet.Cells[i, 5].Text.NullString() == "" ||
                            worksheet.Cells[i, 6].Text.NullString() == "")
                        {
                            resultDB.ReturnInt = -1;
                            resultDB.ReturnString = "Data is not empty";
                            return resultDB;
                        }

                        if (!worksheet.Cells[i, 2].Text.NullString().Contains("W"))
                        {
                            resultDB.ReturnInt = -1;
                            resultDB.ReturnString = "Add 'W' to week";
                            return resultDB;
                        }

                        row["Month"] = worksheet.Cells[i, 1].Text.NullString();
                        row["Week"] = worksheet.Cells[i, 2].Text.NullString();

                        if (!DateTime.TryParse(worksheet.Cells[i, 3].Text.NullString(), out _))
                        {
                            resultDB.ReturnInt = -1;
                            resultDB.ReturnString = "Received date is format (YYYY-MM-dd) : " + worksheet.Cells[i, 3].Text.NullString();
                            return resultDB;
                        }

                        DateTime.TryParse(worksheet.Cells[i, 3].Text.NullString(), out DateTime dt);
                        row["Date"] = dt.ToString("yyyy-MM-dd");

                        row["Customer"] = worksheet.Cells[i, 4].Text.NullString();
                        row["Part"] = worksheet.Cells[i, 5].Text.NullString();
                        row["Qty"] = worksheet.Cells[i, 6].Text.NullString();
                        row["Wisol_Model"] = worksheet.Cells[i, 7].Text.NullString();
                        row["Customer_Code"] = worksheet.Cells[i, 8].Text.NullString();
                        row["Marking"] = worksheet.Cells[i, 9].Text.NullString();
                        row["ProductionDate"] = worksheet.Cells[i, 10].Text.NullString();
                        row["SetModel"] = worksheet.Cells[i, 11].Text.NullString();
                        row["Result"] = worksheet.Cells[i, 12].Text.NullString();
                        row["Note"] = worksheet.Cells[i, 13].Text.NullString();

                        if (worksheet.Cells[i, 12].Text.NullString() == "OK")
                        {
                            row["OK"] = "OK";
                            row["NG"] = "";
                            row["Not_Measure"] = "";
                        }
                        else if (worksheet.Cells[i, 12].Text.NullString() == "NG")
                        {
                            row["NG"] = "NG";
                            row["OK"] = "";
                            row["Not_Measure"] = "";
                        }
                        else
                        {
                            row["Not_Measure"] = "NM";
                            row["NG"] = "";
                            row["OK"] = "";
                        }

                        table.Rows.Add(row);
                    }

                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add("A_USER", GetUserId());
                    resultDB = _vocRepository.ExecProceduce("PKG_BUSINESS.PUT_VOC_ONSITE", dic, "A_DATA", table);
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

        public List<string> GetModel()
        {
           var lst = _vocRepository.FindAll().Select(x => x.Wisol_Model).Distinct().ToList();
            return lst;
        }
    }
}
