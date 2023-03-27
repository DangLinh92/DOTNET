using OPERATION_MNS.Application.ViewModels.Wlp2;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.Interfaces
{
    public interface ISMTReturnWlp2Service : IDisposable
    {
        List<SmtReturnWlp2ViewModel> GetSmtReturn();

        SmtReturnWlp2ViewModel Insert(SmtReturnWlp2ViewModel model);
        SmtReturnWlp2ViewModel Update(SmtReturnWlp2ViewModel model);
        SmtReturnWlp2ViewModel FindSingle(string sapcode);

        void Save();
    }
}
