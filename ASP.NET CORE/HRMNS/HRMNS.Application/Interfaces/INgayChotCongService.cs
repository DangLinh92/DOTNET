using HRMNS.Application.ViewModels.HR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface INgayChotCongService : IDisposable
    {
        HR_NgayChotCongViewModel Update(HR_NgayChotCongViewModel model);
        HR_NgayChotCongViewModel FindItem(string chotCongChoThang);
        HR_NgayChotCongViewModel FinLastItem();
    }
}
