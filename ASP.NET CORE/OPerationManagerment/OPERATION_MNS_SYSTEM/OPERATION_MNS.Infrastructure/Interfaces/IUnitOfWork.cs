using OPERATION_MNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace OPERATION_MNS.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Call save change from db context
        /// </summary>
        void Commit();
    }
}
