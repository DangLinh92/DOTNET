using AutoMapper;
using Microsoft.AspNetCore.Http;
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
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public LeadTimeService(IRespository<INVENTORY_ACTUAL, int> InventoryActualRepository,
            IRespository<SETTING_ITEMS, string> SettingItemRepository,
            IRespository<DATE_OFF_LINE, int> DateOffRespository,
                              IUnitOfWork unitOfWork, IMapper mapper,
                              IHttpContextAccessor httpContextAccessor)
        {
            _DateOffRespository = DateOffRespository;
            _InventoryActualRepository = InventoryActualRepository;
            _SettingItemRepository = SettingItemRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<LeadTimeViewModel> GetLeadTime(string year, string month, string week, string day, string ox)
        {
            List<LeadTimeViewModel> result = new List<LeadTimeViewModel>();
            Dictionary<string, string> dic = new Dictionary<string, string>();

            dic.Add("A_YEAR", year.NullString());
            dic.Add("A_MONTH", month.NullString());
            dic.Add("A_WEEK", week.NullString());
            dic.Add("A_DAY", day.NullString());
            dic.Add("A_OX", ox.NullString());

            ResultDB resultDB = _InventoryActualRepository.ExecProceduce2("PKG_BUSINESS@GET_LEADTIME", dic);

            if (resultDB.ReturnInt == 0)
            {
                DataTable data = resultDB.ReturnDataSet.Tables[0];
                if(data.Rows.Count > 0)
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

                        // khong có kế hoach thì bỏ qua
                        if (_DateOffRespository.FindAll(x=>x.ItemValue.Replace("-","") == row["WorkDate"].NullString() && x.WLP == "WLP1").ToList().Count > 0)
                        {
                            leadTime.WaitTime =0;
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
            if(_SettingItemRepository.FindById("WLP1_LEAD_TIME_" + year) != null && _SettingItemRepository.FindById("WLP2_LEAD_TIME_" + year) !=  null)
            {
                double targetWLP1 = double.Parse(_SettingItemRepository.FindById("WLP1_LEAD_TIME_" + year)?.ItemValue.IfNullIsZero());
                double targetWLP2 = double.Parse(_SettingItemRepository.FindById("WLP2_LEAD_TIME_" + year)?.ItemValue.IfNullIsZero());

                return targetWLP1 + targetWLP2;
            }

            return 0;
        }
    }
}
