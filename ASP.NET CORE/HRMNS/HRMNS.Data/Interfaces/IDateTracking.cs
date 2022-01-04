using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.Interfaces
{
    public interface IDateTracking
    {
        string DateCreated { get; set; }
        string DateModified { get; set; }
        string UserCreated { get; set; }
        string UserModified { get; set; }
    }
}
