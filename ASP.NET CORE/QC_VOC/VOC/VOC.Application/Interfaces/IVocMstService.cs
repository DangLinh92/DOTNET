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

        List<TotalVOCSiteModel> ReportDefectByYear(string year, string classification, string customer, string side);

        TotalVOCSiteModel ReportByYear(string year, string customer);
        List<VOCSiteModelByTimeLst> ReportByMonth(string year, string customer, string side);
        PPMDataChartAll ReportPPMByYear(string year,string module, out List<VOCPPM_Ex> pMDataCharts);

        List<VOCSiteModelByTimeLst> ReportInit();

        VOC_MSTViewModel GetById(int id);

        List<VOC_DefectTypeViewModel> GetDefectType();

        GmesDataViewModel GetGmesData(int year, int month);

        VocPPMYearViewModel UpdatePPMByYear(bool isAdd, VocPPMYearViewModel model);

        VocPPMYearViewModel GetPPMByYear(int id);

        double GetTargetPPMByYear (int year);

        VocPPMViewModel UpdatePPMByYearMonth(bool isAdd, VocPPMViewModel model);
        VocPPMViewModel GetPPMByYearMonth(int id);

        VocProgessInfo GetProgressInfo(int year,string customer,string side);

        List<string> GetCustomer();

        void DeletePPMByYear(int Id);
        void DeletePPMByYearMonth(int Id);

        void Update(VOC_MSTViewModel function);

        void Delete(int id);

        void Save();
    }
}
