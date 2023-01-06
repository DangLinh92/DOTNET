using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels
{
   public  class DateOffLineViewModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string ItemValue { get; set; }

        [StringLength(10)]
        public string OnOff { get; set; }

        [StringLength(50)]
        public string WLP { get; set; }


        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }
    }
}
