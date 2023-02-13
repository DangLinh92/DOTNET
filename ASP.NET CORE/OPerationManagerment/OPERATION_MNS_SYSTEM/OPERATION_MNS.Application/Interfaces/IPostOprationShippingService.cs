using OPERATION_MNS.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.Interfaces
{
    public interface IPostOprationShippingService : IDisposable
    {
        List<PostOpeationShippingViewModel> GetPostOpeationShipping(string fromTime,string toTime);
        List<XuatHangViewModel> GetXuatHangViewModel(string fromTime, string toTime);

        PostOpeationShippingViewModel Add(PostOpeationShippingViewModel model);
        PostOpeationShippingViewModel Update(PostOpeationShippingViewModel model);

        PostOpeationShippingViewModel FindItemXH1(string key);
        List<PostOpeationShippingViewModel> FindItemXH2(string key);

        void Delete(PostOpeationShippingViewModel model);

        void Save();
    }
}
