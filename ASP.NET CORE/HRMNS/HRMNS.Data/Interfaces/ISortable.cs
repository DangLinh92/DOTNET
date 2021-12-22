using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.Interfaces
{
    public interface ISortable
    {
        int SortOrder { get; set; }
    }
}
