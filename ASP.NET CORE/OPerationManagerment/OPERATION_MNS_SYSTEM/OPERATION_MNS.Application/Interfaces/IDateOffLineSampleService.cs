using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.Interfaces
{
    public interface IDateOffLineSampleService : IDisposable
    {
        List<DATE_OFF_LINE_SAMPLE> GetAllData();
        DATE_OFF_LINE_SAMPLE Update(DATE_OFF_LINE_SAMPLE model);
        DATE_OFF_LINE_SAMPLE Add(DATE_OFF_LINE_SAMPLE model);

        DATE_OFF_LINE_SAMPLE GetById(int id);
        void Delete(int Id);
        void Save();
    }
}
