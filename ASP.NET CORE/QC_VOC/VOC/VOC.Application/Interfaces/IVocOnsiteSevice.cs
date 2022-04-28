using System;
using System.Collections.Generic;
using System.Text;
using VOC.Application.ViewModels.VOC;
using VOC.Utilities.Dtos;

namespace VOC.Application.Interfaces
{
    public interface IVocOnsiteSevice
    {
        ResultDB ImportExcel(string filePath, string param);
        List<VocOnsiteModel> SumDataOnsite(int year,string customer, string part);
        List<VocOnsiteViewModel> GetAllOnsiteByTime(string fromTime, string toTime);
        VocOnsiteViewModel UpdateVocOnsite(VocOnsiteViewModel vm);
        VocOnsiteViewModel AddVocOnsite(VocOnsiteViewModel vm);

        VocOnsiteViewModel FindById(int id);

        List<string> GetModel();
        void DeleteVocOnsite(int id);
        void Save();
    }
}
