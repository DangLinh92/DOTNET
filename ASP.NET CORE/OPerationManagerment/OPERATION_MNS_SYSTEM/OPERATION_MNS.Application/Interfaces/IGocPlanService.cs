﻿using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.Interfaces
{
    public interface IGocPlanService : IDisposable
    {
        void Add(GocPlanViewModel model);

        List<GocPlanViewModel> GetAll();
        List<string> DateOffLine(string year, string owner, string wlp,string danhmuc="");

        List<GocPlanViewModelEx> GetByTime(string unit,string fromDate,string toDate,string wlp = "WLP1",string danhmuc="");
        List<GocPlanViewModelEx> GetByTime_fab(string unit,string fromDate,string toDate);

        ViewControlChartDataModel GetDataControlChart(string date,string toDate, string operation, string mattertial);

        GocPlanViewModel GetById(int id);

        List<ProcActualPlanModel> GetProcActualPlanModel(string month);
        List<ProcActualPlanModel> GetProcActualPlanWlp2Model(string month,string danhmuc);

        void DeleteGocModel(int Id, string fromDate, string toDate,string wlp);
        void DeleteGocModelWlp2(string model, string fromDate, string toDate,string danhmuc);

        ResultDB ImportExcel(string filePath, string param);
        ResultDB ImportExcel_Wlp2(string filePath, string param);

        void Update(GocPlanViewModel model);

        void Delete(int id);

        void Save();

        List<CTQSettingViewModel> GetCTQ();
        CTQSettingViewModel PutCTQ(CTQSettingViewModel ctq);
        CTQSettingViewModel PostCTQ(CTQSettingViewModel ctq);
        CTQSettingViewModel DeleteCTQ(CTQSettingViewModel ctq);

        CTQSettingViewModel GetCTQ_Id(int Id);
    }
}
