using HRMNS.Data.Entities;
using HRMNS.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HRMNS.Data.EF.Repositories
{
    public class BoPhanDetailRepository : EFRepository<HR_BO_PHAN_DETAIL, int>, IBoPhanDetailRepository
    {
        AppDBContext _context;
        public BoPhanDetailRepository(AppDBContext context) : base(context)
        {
            _context = context;
        }
    }
}
