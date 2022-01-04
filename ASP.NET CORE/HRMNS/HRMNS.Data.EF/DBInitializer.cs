using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.EF
{
    public class DBInitializer
    {
        private readonly AppDBContext _context;
        public DBInitializer(AppDBContext context)
        {
            _context = context;
        }

        public void Seed()
        {

        }
    }
}
