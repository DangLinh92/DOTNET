using HRMNS.Utilities.Dtos;
using HRMS.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

    public class EFPayrollUnitOfWork : IPayrollUnitOfWork
    {
        private readonly PayrollDBContext _context;
        public EFPayrollUnitOfWork(PayrollDBContext context)
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

        public PayrollDBContext DBContext()
        {
            return _context;
        }
    }
}
