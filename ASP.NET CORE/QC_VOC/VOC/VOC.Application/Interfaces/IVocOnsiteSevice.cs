using System;
using System.Collections.Generic;
using System.Text;
using VOC.Application.ViewModels.VOC;

namespace VOC.Application.Interfaces
{
    public interface IVocOnsiteSevice
    {
        List<VocOnsiteList> SumDataOnsite(int year,string timeWeek);
    }
}
