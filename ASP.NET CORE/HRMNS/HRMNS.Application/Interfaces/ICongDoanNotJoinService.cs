using HRMNS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface ICongDoanNotJoinService : IDisposable
    {
        List<CONGDOAN_NOT_JOIN> GetAll();
        void Delete(int id);
        CONGDOAN_NOT_JOIN Add(CONGDOAN_NOT_JOIN item);
    }
}
