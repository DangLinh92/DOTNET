using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.Interfaces
{
   public  interface IHasSoftDelete
    {
        bool IsDeleted { get; set; }

    }
}
