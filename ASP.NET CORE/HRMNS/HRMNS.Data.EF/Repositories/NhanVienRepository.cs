using HRMNS.Data.Entities;
using HRMNS.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HRMNS.Data.EF.Repositories
{
    public class NhanVienRepository : EFRepository<HR_NHANVIEN,string>, INhanVienRepository
    {
        AppDBContext _context;

        public NhanVienRepository(AppDBContext context) : base(context)
        {
            _context = context;
        }

        //public List<ProductCategory> GetByAlias(string alias)
        //{
        //    return _context.ProductCategories.Where(x => x.SeoAlias == alias).ToList();
        //}
    }
}
