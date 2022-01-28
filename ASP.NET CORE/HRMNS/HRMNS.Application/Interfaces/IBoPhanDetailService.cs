using HRMNS.Application.ViewModels.HR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMNS.Application.Interfaces
{
    public interface IBoPhanDetailService : IDisposable
    {
        List<BoPhanDetailViewModel> GetAll(string filter);
    }
}
