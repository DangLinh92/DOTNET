using HRMNS.Data.Entities;
using HRMNS.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF.Repositories
{
    public class ChucDanhRepository : EFRepository<HR_CHUCDANH, string>, IChucDanhRepository
    {
        AppDBContext _context;
        public ChucDanhRepository(AppDBContext context) : base(context)
        {
            _context = context;
        }
    }
}
