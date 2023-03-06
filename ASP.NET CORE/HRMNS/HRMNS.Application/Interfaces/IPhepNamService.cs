using HRMNS.Application.ViewModels.HR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IPhepNamService
    {
        List<PhepNamViewModel> GetList(string year);
        PhepNamViewModel Add(PhepNamViewModel phepNamVm);
        PhepNamViewModel Update(PhepNamViewModel phepNamVm);
        PhepNamViewModel UpdateSingle(PhepNamViewModel phepNamVm);
        void Delete(int id);
        List<PhepNamViewModel> GetAll(string keyword);
        PhepNamViewModel GetById(int id);
        void Save();
        void ImportExcel(string filePath);
    }
}
