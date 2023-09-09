using AutoMapper;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Infrastructure.Interfaces;
using OPERATION_MNS.Utilities.Common;
using OPERATION_MNS.Utilities.Constants;
using OPERATION_MNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace OPERATION_MNS.Application.Implementation
{
    public class LeadTimeService : BaseService, ILeadTimeService
    {
        private IRespository<INVENTORY_ACTUAL, int> _InventoryActualRepository;
        private IRespository<SETTING_ITEMS, string> _SettingItemRepository;
        private IRespository<DATE_OFF_LINE, int> _DateOffRespository;
        private IRespository<DATE_OFF_LINE_LFEM, int> _DateOffLFEMRespository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public LeadTimeService(IRespository<INVENTORY_ACTUAL, int> InventoryActualRepository,
            IRespository<SETTING_ITEMS, string> SettingItemRepository,
            IRespository<DATE_OFF_LINE, int> DateOffRespository,
            IRespository<DATE_OFF_LINE_LFEM, int> DateOffLFEMRespository,
                              IUnitOfWork unitOfWork, IMapper mapper,
                              IHttpContextAccessor httpContextAccessor)
        {
            _DateOffRespository = DateOffRespository;
            _InventoryActualRepository = InventoryActualRepository;
            _SettingItemRepository = SettingItemRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _DateOffLFEMRespository = DateOffLFEMRespository;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<LeadTimeViewModel> GetLeadTime(string year, string month, string week, string day, string ox)
        {
            List<LeadTimeViewModel> result = new List<LeadTimeViewModel>();
            Dictionary<string, string> dic = new Dictionary<string, string>
            {
                { "A_YEAR", year.NullString() },
                { "A_MONTH", month.NullString() },
                { "A_WEEK", week.NullString() },
                { "A_DAY", day.NullString() },
                { "A_OX", ox.NullString() }
            };

            ResultDB resultDB = _InventoryActualRepository.ExecProceduce2("PKG_BUSINESS@GET_LEADTIME", dic);

            if (resultDB.ReturnInt == 0)
            {
                DataTable data = resultDB.ReturnDataSet.Tables[0];
                if (data.Rows.Count > 0)
                {
                    LeadTimeViewModel leadTime;

                    double targetWLP1 = 0;
                    double targetWLP2 = 0;

                    if (_SettingItemRepository.FindById("WLP1_LEAD_TIME_" + year) != null && _SettingItemRepository.FindById("WLP2_LEAD_TIME_" + year) != null)
                    {
                        targetWLP1 = double.Parse(_SettingItemRepository.FindById("WLP1_LEAD_TIME_" + year)?.ItemValue.IfNullIsZero());
                        targetWLP2 = double.Parse(_SettingItemRepository.FindById("WLP2_LEAD_TIME_" + year)?.ItemValue.IfNullIsZero());
                    }

                    foreach (DataRow row in data.Rows)
                    {
                        leadTime = new LeadTimeViewModel();
                        leadTime.WorkDate = row["WorkDate"].NullString();
                        leadTime.WorkMonth = row["WorkMonth"].NullString();
                        leadTime.WorkYear = row["WorkYear"].NullString();
                        leadTime.WorkWeek = row["WorkWeek"].NullString();
                        leadTime.HoldTime = double.Parse(row["HoldTime"].IfNullIsZero());

                        if (row["WLP"].NullString() == "WLP1")
                        {
                            // khong có kế hoach thì bỏ qua
                            if (_DateOffRespository.FindAll(x => x.ItemValue.Replace("-", "") == row["WorkDate"].NullString() && x.WLP == "WLP1").ToList().Count > 0)
                            {
                                leadTime.WaitTime = 0;
                                leadTime.RunTime = 0;
                                leadTime.LeadTime = 0;
                                continue;
                            }
                            else
                            {
                                leadTime.WaitTime = double.Parse(row["WaitTime"].IfNullIsZero());
                                leadTime.RunTime = double.Parse(row["RunTime"].IfNullIsZero());
                                leadTime.LeadTime = double.Parse(row["LeadTime"].IfNullIsZero());
                            }
                        }
                        else if (row["WLP"].NullString() == "WLP2")
                        {
                            // khong có kế hoach thì bỏ qua
                            if (_DateOffRespository.FindAll(x => x.ItemValue.Replace("-", "") == row["WorkDate"].NullString() && x.WLP == "WLP2" && x.DanhMuc == "SAN_XUAT").ToList().Count > 0)
                            {
                                leadTime.WaitTime = 0;
                                leadTime.RunTime = 0;
                                leadTime.LeadTime = 0;
                                continue;
                            }
                            else
                            {
                                leadTime.WaitTime = double.Parse(row["WaitTime"].IfNullIsZero());
                                leadTime.RunTime = double.Parse(row["RunTime"].IfNullIsZero());
                                leadTime.LeadTime = double.Parse(row["LeadTime"].IfNullIsZero());
                            }
                        }


                        leadTime.LeadTimeMax = double.Parse(row["LeadTimeMax"].IfNullIsZero());
                        leadTime.WLP = row["WLP"].NullString();
                        leadTime.Ox = row["Ox"].NullString();
                        leadTime.Target = leadTime.WLP == "WLP1" ? targetWLP1 : targetWLP2;
                        result.Add(leadTime);
                    }
                }
            }
            return result;
        }

        public double GetTargetWLP(string year)
        {
            if (_SettingItemRepository.FindById("WLP1_LEAD_TIME_" + year) != null && _SettingItemRepository.FindById("WLP2_LEAD_TIME_" + year) != null)
            {
                double targetWLP1 = double.Parse(_SettingItemRepository.FindById("WLP1_LEAD_TIME_" + year)?.ItemValue.IfNullIsZero());
                double targetWLP2 = double.Parse(_SettingItemRepository.FindById("WLP2_LEAD_TIME_" + year)?.ItemValue.IfNullIsZero());

                return targetWLP1 + targetWLP2;
            }

            return 0;
        }

        #region LFEM
        public List<LeadTimeViewModel> GetLeadTimeLFEM(string year, string month, string week, string day, string ox, string model)
        {
            string fromDate = "";
            string toDate = "";
            if (year.NullString() != "")
            {
                fromDate = year + "0101";
                toDate = DateTime.Parse(year+"-01-01").AddYears(1).AddDays(-1).ToString("yyyyMMdd");
            }

            if (month.NullString() != "")
            {
                fromDate = DateTime.Parse(year + "-" + month + "-01").ToString("yyyyMMdd");
                toDate = DateTime.Parse(year + "-" + month + "-01").AddMonths(1).AddDays(-1).ToString("yyyyMMdd");
            }

            List<LeadTimeViewModel> result = new List<LeadTimeViewModel>();
            Dictionary<string, string> dic = new Dictionary<string, string>
            {
                { "A_TIME_FROM", fromDate.NullString() },
                { "A_TIME_TO", toDate.NullString() }
            };

            string spName = "";
            if (model.NullString() == "EX_R8Y0_L2A0")
            {
                spName = "GET_LEADTIME_DATA_LFEM_EX_L2A0_L8Y0";
            }
            else if (model.NullString() == "R8Y0")
            {
                spName = "GET_LEADTIME_DATA_LFEM_R8Y0";
            }
            else if (model.NullString() == "L2A0")
            {
                spName = "GET_LEADTIME_DATA_LFEM_L2A0";
            }
            else
            {
                spName = "GET_LEADTIME_DATA_LFEM_ALL";
            }

            ResultDB resultDB = _InventoryActualRepository.ExecProceduce2(spName, dic);

            List<DATE_OFF_LINE_LFEM> lstDateOff = _DateOffLFEMRespository.FindAll(x => x.ItemValue.StartsWith(year)).OrderBy(x => x.ItemValue).ToList();

            if (resultDB.ReturnInt == 0)
            {
                DataTable data = resultDB.ReturnDataSet.Tables[0];
                if (data.Rows.Count > 0)
                {
                    LeadTimeViewModel leadTime;

                    string beforeDate = "";
                    foreach (DataRow row in data.Rows)
                    {
                        leadTime = new LeadTimeViewModel();
                        leadTime.WorkDate = row["WORK_DATE"].NullString();
                        leadTime.WorkMonth = row["WORK_MONTH"].NullString();
                        leadTime.WorkYear = row["WORK_YEAR"].NullString();
                        leadTime.WorkWeek = row["WORK_WEEK"].NullString();
                        leadTime.Operation = row["OPERATION_SHORT_NAME"].NullString();
                        leadTime.OperationID = row["OPERATION_ID"].NullString();
                        leadTime.LeadTimeStartEnd = double.Parse(row["LEAD_TIME_START_END"].IfNullIsZero());
                        leadTime.DisplayOrder = double.Parse(row["DISPLAY_ORDER"].IfNullIsZero());

                        // khong có kế hoach thì bỏ qua
                        if (lstDateOff.FindAll(x => x.ItemValue.NullString() == row["WORK_DATE"].NullString() && x.DanhMuc == "KHSX").ToList().Count > 0)
                        {
                            leadTime.WaitTime = 0;
                            leadTime.RunTime = 0;
                            leadTime.LeadTime = 0;

                            continue;
                        }
                        else
                        {
                            leadTime.WaitTime = double.Parse(row["WAIT_TIME"].IfNullIsZero());
                            leadTime.RunTime = double.Parse(row["RUN_TIME"].IfNullIsZero());
                        }

                        beforeDate = DateTime.Parse(leadTime.WorkDate).AddDays(-1).ToString("yyyy-MM-dd");

                        // set giá trị X cho ngày sau ngày k có kế hoạch 
                        if (lstDateOff.FindAll(x => x.ItemValue.NullString() == leadTime.WorkDate && x.DanhMuc == "KHSX").ToList().Count == 0 &&
                            lstDateOff.FindAll(x => x.ItemValue.NullString() == beforeDate && x.DanhMuc == "KHSX").ToList().Count > 0)
                        {
                            leadTime.Ox = "X";
                        }

                        result.Add(leadTime);
                    }
                }
            }

            if (week.NullString() != "" && day.NullString() == "")
            {
                result = result.Where(x => int.Parse(x.WorkWeek) == int.Parse(week)).ToList();
            }
            else
            if (day.NullString() != "" && month.NullString() != "" && year.NullString() != "")
            {
                string date = year + "-" + month + "-" + day;
                result = result.Where(x => x.WorkDate == date).ToList();
            }

            if (ox.NullString() != "")
            {
                result = result.Where(x => x.Ox != ox).ToList();
            }

            return result;
        }
        #endregion
    }
}
