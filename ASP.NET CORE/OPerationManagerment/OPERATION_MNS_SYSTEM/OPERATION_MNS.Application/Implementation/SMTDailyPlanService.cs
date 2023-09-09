using AutoMapper;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels.SMT;
using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Infrastructure.Interfaces;
using OPERATION_MNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OPERATION_MNS.Application.Implementation
{
    public class SMTDailyPlanService : BaseService, ISMTDailyPlanService
    {
        private IRespository<GOC_PLAN_SMT, int> _gocSmtRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SMTDailyPlanService(IRespository<GOC_PLAN_SMT, int> gocSmtRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _gocSmtRepository = gocSmtRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void Dispose()
        {
           GC.SuppressFinalize(this);
        }

        public List<DailyPlanSMTViewModel> GetDailyPlanSMT(string date)
        {
            List<DailyPlanSMTViewModel> SmtDailys = new List<DailyPlanSMTViewModel>();

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("A_DAY_INPUT", date);
            ResultDB resultDB = _gocSmtRepository.ExecProceduce2("GET_DAILY_PLAN_SMT", dic);

            if (resultDB.ReturnInt == 0)
            {
                // LUY KẾ KẾ HOACH T0->T+5
                DataTable tblSmtPlan = resultDB.ReturnDataSet.Tables[0];
                DataTable tblSmtPlan1 = resultDB.ReturnDataSet.Tables[1];
                DataTable tblSmtPlan2 = resultDB.ReturnDataSet.Tables[2];
                DataTable tblSmtPlan3 = resultDB.ReturnDataSet.Tables[3];
                DataTable tblSmtPlan4 = resultDB.ReturnDataSet.Tables[4];
                DataTable tblSmtPlan5 = resultDB.ReturnDataSet.Tables[5];

                // luy ke sxtt
                DataTable tblSmtActual = resultDB.ReturnDataSet.Tables[6];

                // Ton thuc te hien tai
                DataTable tblSmtInventory = resultDB.ReturnDataSet.Tables[7];

                DailyPlanSMTViewModel daily;
                string filterEx = "";
                foreach (DataRow row in tblSmtPlan.Rows)
                {
                    daily = new DailyPlanSMTViewModel();
                    daily.MesCode = row["MesItemId"].NullString();
                    daily.Model = row["MesItemId"].NullString().Substring(3, 4);
                    daily.GocPlanToday = double.Parse(row["QTY"].IfNullIsZero());

                    filterEx = "MesItemId = '" + daily.MesCode + "'";

                    daily.Inventory = 0;
                    if (tblSmtInventory.Select(filterEx).Length > 0)
                    {
                        daily.Inventory = double.Parse(tblSmtInventory.Select(filterEx)[0]["ChipQty"].IfNullIsZero());
                    }

                    daily.GocPlanToday_1 = 0;
                    daily.GocPlanToday_2 = 0;
                    daily.GocPlanToday_3 = 0;
                    daily.GocPlanToday_4 = 0;
                    daily.GocPlanToday_5 = 0;
                    daily.ActualToday = 0;

                    // thuc tế 
                    if (tblSmtActual.Select(filterEx).Length > 0)
                    {
                        daily.ActualToday = double.Parse(tblSmtActual.Select(filterEx)[0]["QTY"].IfNullIsZero());
                    }

                    // Luy kế goc plan

                    if (tblSmtPlan1.Select(filterEx).Length > 0)
                    {
                        daily.GocPlanToday_1 = double.Parse(tblSmtPlan1.Select(filterEx)[0]["QTY"].IfNullIsZero());
                    }

                    if (tblSmtPlan2.Select(filterEx).Length > 0)
                    {
                        daily.GocPlanToday_2 = double.Parse(tblSmtPlan2.Select(filterEx)[0]["QTY"].IfNullIsZero());
                    }

                    if (tblSmtPlan3.Select(filterEx).Length > 0)
                    {
                        daily.GocPlanToday_3 = double.Parse(tblSmtPlan3.Select(filterEx)[0]["QTY"].IfNullIsZero());
                    }

                    if (tblSmtPlan4.Select(filterEx).Length > 0)
                    {
                        daily.GocPlanToday_4 = double.Parse(tblSmtPlan4.Select(filterEx)[0]["QTY"].IfNullIsZero());
                    }

                    if (tblSmtPlan5.Select(filterEx).Length > 0)
                    {
                        daily.GocPlanToday_5 = double.Parse(tblSmtPlan5.Select(filterEx)[0]["QTY"].IfNullIsZero());
                    }

                    // ke hoach

                    daily.PlanToday = daily.ActualToday - daily.GocPlanToday;
                    daily.PlanToday_1 = daily.ActualToday - daily.GocPlanToday_1;
                    daily.PlanToday_2 = daily.ActualToday - daily.GocPlanToday_2;
                    daily.PlanToday_3 = daily.ActualToday - daily.GocPlanToday_3;
                    daily.PlanToday_4 = daily.ActualToday - daily.GocPlanToday_4;
                    daily.PlanToday_5 = daily.ActualToday - daily.GocPlanToday_5;
                    daily.LastUpdate = DateTime.Now.ToString("HH:mm:ss");

                    SmtDailys.Add(daily);
                }
            }

            return SmtDailys;
        }

        public  NextDay_SMT GetNexDaySMT(string datePlan)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
           
            dic.Add("A_DATE_PLAN", datePlan);
            ResultDB resultDB = _gocSmtRepository.ExecProceduce2("GET_NEXT_5_DAY_SMT", dic);
            NextDay_SMT nextDay = new NextDay_SMT();
            if (resultDB.ReturnInt == 0)
            {
                DataTable dt = resultDB.ReturnDataSet.Tables[0];
                nextDay.Day1 = dt.Rows[0]["NEXT1"].NullString();
                nextDay.Day2 = dt.Rows[0]["NEXT2"].NullString();
                nextDay.Day3 = dt.Rows[0]["NEXT3"].NullString();
                nextDay.Day4 = dt.Rows[0]["NEXT4"].NullString();
                nextDay.Day5 = dt.Rows[0]["NEXT5"].NullString();
            }

            return nextDay;
        }
    }
}
