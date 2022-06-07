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

        ChamCongLogViewModel Update(ChamCongLogViewModel model);

        List<ChamCongLogViewModel> Search(string result,string dept,string timeFrom,string timeTo);

        ResultDB ImportExcel(string filePath, string param);

        string GetMaxDate();

        ResultDB InsertLogData(DataTable data);

        void Save();
    }
}
