using HRMNS.Data.Entities;
using HRMNS.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HRMNS.Data.EF.Repositories
{
    public class BoPhanRepository : EFRepository<BOPHAN, string>, IBoPhanRepository
    {
        AppDBContext _context;
        public BoPhanRepository(AppDBContext context) : base(context)
        {
            _context = context;
        }
    }
}
