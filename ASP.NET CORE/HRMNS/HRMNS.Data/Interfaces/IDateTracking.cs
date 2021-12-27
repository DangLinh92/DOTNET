using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.Interfaces
{
    public interface IDateTracking
    {
        DateTime DateCreated { get; set; }
        DateTime DateModified { get; set; }
        string UserCreated { get; set; }
        string UserModified { get; set; }
    }
}
