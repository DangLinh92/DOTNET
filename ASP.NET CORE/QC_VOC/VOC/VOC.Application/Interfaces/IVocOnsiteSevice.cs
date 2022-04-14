using System;
using System.Collections.Generic;
using System.Text;
using VOC.Application.ViewModels.VOC;

namespace VOC.Application.Interfaces
{
    public interface IVocOnsiteSevice
    {
        List<VocOnsiteModel> SumDataOnsite(int year,string customer, string part);
    }
}
