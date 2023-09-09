using OPERATION_MNS.Application.ViewModels.Sameple;
using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.Interfaces
{
    public interface IScheduleSampleService : IDisposable
    {
        List<TinhHinhSanXuatSampleViewModel> GetOpens();
        List<TinhHinhSanXuatSampleViewModel> GetByTime(string from,string to);
        TinhHinhSanXuatSampleViewModel FindById(int id);
        TinhHinhSanXuatSampleViewModel FindByCassetId(string cassetId);
        TinhHinhSanXuatSampleViewModel Update(TinhHinhSanXuatSampleViewModel en);
        ResultDB ImportExcel(string filePath, string param);

        LeadTimeSampleModel GetDataLeadTimeChart(string year,string month,string week,string code,string nguoiphutrach,string gap);

        List<DELAY_COMMENT_SAMPLE> GetDelayOpen();
        List<DELAY_COMMENT_SAMPLE> GetDelayByTime(string from, string to);
        ResultDB ImportDelayExcel(string filePath, string param);
        DELAY_COMMENT_SAMPLE UpdateComment(DELAY_COMMENT_SAMPLE en);
        DELAY_COMMENT_SAMPLE FindCommentById(int id);

        ResultDB ImportPlanSampleExcel(string filePath, string param);
        List<SamplePlanViewModel> GetActualPlanSample(string month);
        List<SamplePlanViewModel> GetActualPlanSampleTotal(string month);

        void Save();
    }
}
