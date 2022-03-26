using VOC.Application.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VOC.Utilities.Dtos;
using VOC.Application.ViewModels.VOC;

namespace VOC.Application.Interfaces
{
    public interface IVocMstService : IDisposable
    {
        ResultDB ImportExcel(string filePath, string param);

        VOC_MSTViewModel Add(VOC_MSTViewModel function);

        List<VOC_MSTViewModel> GetAll(string filter);

        List<VOC_MSTViewModel> SearchByTime(string fromTime, string toTime);

        List<VOCSiteModelByTimeLst> ReportByWeek(int fromWeek, int toWeek,string year);

        TotalVOCSiteModel ReportByYear(string year);

        List<VOCSiteModelByTimeLst> ReportInit();

        VOC_MSTViewModel GetById(int id);

        void Update(VOC_MSTViewModel function);

        void Delete(int id);

        void Save();
    }
}
