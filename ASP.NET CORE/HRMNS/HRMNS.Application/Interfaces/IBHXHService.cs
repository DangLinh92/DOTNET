using HRMNS.Application.ViewModels.HR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IBHXHService
    {
        List<BHXHViewModel> GetAll(string filter);
        BHXHViewModel Add(BHXHViewModel bhxhVm);
        void Update(BHXHViewModel bhxhVm);
        BHXHViewModel GetById(string id);
        void Delete(string id);
        void Save();

        string GetUserLogin();
    }
}
