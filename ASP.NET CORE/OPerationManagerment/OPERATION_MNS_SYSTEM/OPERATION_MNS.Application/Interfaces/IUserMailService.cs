using OPERATION_MNS.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.Interfaces
{
    public interface IUserMailService : IDisposable
    {
        List<CTQEmailReceivViewModel> GetListMail();

        void PutMail(CTQEmailReceivViewModel email);
        void PostMail(CTQEmailReceivViewModel email);
        void DeleteMail(string email);
        CTQEmailReceivViewModel GetMail(string email);
    }
}
