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


        List<CTQEmailReceivViewModel> GetListMailWlp2();
        void PutMailWlp2(CTQEmailReceivViewModel email);
        void PostMailWlp2(CTQEmailReceivViewModel email);
        void DeleteMailWlp2(string email);
        CTQEmailReceivViewModel GetMailWlp2(string email);
    }
}
