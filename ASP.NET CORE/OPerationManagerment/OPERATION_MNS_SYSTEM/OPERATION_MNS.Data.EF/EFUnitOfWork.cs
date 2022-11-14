using OPERATION_MNS.Utilities.Dtos;
using OPERATION_MNS.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace OPERATION_MNS.Data.EF
{
    public class EFUnitOfWork: IUnitOfWork
    {
        private readonly AppDBContext _context;
        public EFUnitOfWork(AppDBContext context)
        {
            _context = context;
        }
        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public AppDBContext DBContext()
        {
            return _context;
        }
    }
}
