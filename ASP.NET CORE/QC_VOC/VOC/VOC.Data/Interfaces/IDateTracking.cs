using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VOC.Data.Interfaces
{
    public interface IDateTracking
    {
        [StringLength(50)]
        string DateCreated { get; set; }

        [StringLength(50)]
        string DateModified { get; set; }

        [StringLength(50)]
        string UserCreated { get; set; }

        [StringLength(50)]
        string UserModified { get; set; }
    }
}
