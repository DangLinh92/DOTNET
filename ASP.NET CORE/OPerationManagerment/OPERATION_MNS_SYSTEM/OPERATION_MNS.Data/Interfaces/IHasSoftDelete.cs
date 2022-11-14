using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Data.Interfaces
{
   public  interface IHasSoftDelete
    {
        bool IsDeleted { get; set; }

    }
}
