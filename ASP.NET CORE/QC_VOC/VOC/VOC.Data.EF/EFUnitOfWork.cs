using VOC.Utilities.Dtos;
using VOC.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace VOC.Data.EF
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
