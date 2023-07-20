using System;
using System.Collections.Generic;
using System.Text;

namespace CarMNS.Data.Interfaces
{
   public  interface IHasSoftDelete
    {
        bool IsDeleted { get; set; }

    }
}
