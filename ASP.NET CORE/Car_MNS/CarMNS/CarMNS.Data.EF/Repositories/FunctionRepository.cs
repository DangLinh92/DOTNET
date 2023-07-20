using CarMNS.Data.Entities;
using CarMNS.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarMNS.Data.EF.Repositories
{
    public class FunctionRepository : EFRepository<FUNCTION, string>, IFunctionRepository
    {
        AppDBContext _context;
        public FunctionRepository(AppDBContext context):base(context)
        {
            _context = context;
        }
    }
}
