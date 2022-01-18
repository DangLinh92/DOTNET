using HRMNS.Application.ViewModels.HR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMNS.Application.Interfaces
{
    public interface IChucDanhService : IDisposable
    {
        List<ChucDanhViewModel> GetAll(string filter);
    }
}
