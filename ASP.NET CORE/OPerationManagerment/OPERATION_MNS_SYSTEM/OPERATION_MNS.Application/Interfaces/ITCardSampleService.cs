using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.Interfaces
{
    public interface ITCardSampleService : IDisposable
    {
        List<TCARD_SAMPLE> GetAllData();
        TCARD_SAMPLE Update(TCARD_SAMPLE model);
        TCARD_SAMPLE Add(TCARD_SAMPLE model);

        TCARD_SAMPLE GetById(int id);
        ResultDB ImportExcel(string filePath, string param);
        void Delete(int Id);
        void Save();
    }
}
