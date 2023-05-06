using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels.Wlp2
{
    public class ThickNetModelWlp2ViewModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Material { get; set; }

        public float ThickNet { get; set; }

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
