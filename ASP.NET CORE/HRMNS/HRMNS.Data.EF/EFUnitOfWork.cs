using HRMS.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF
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
