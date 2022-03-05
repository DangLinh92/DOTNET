using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMNS.Application.Interfaces
{
    public interface IChamCongService: IDisposable
    {
        List<ChamCongLogViewModel> GetAll(string keyword);

        List<ChamCongLogViewModel> Search(string condition,string param);

        ResultDB ImportExcel(string filePath, string param);

        void Save();
    }
}
