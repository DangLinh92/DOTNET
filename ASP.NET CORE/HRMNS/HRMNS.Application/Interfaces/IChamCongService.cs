using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace HRMNS.Application.Interfaces
{
    public interface IChamCongService: IDisposable
    {
        List<ChamCongLogViewModel> GetAll(string keyword);
        List<ChamCongLogViewModel> GetByTime(string fromTime,string toTime);

        ChamCongLogViewModel Update(ChamCongLogViewModel model);
        ChamCongLogViewModel UpdateRequest(ChamCongLogViewModel model);

        List<ChamCongLogViewModel> Search(string result,string dept,ref string timeFrom,ref string timeTo);

        ResultDB ImportExcel(string filePath, DataTable employees);

        string GetMaxDate();

        ResultDB InsertLogData(DataTable data);

        ResultDB GetLogDataCurrentDay();

        void Save();

        void SetDepartment(string dept);
    }
}
