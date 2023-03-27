using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels.Wlp2
{
    public class SmtReturnWlp2ViewModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string SapCode { get; set; }

        public int SmtReturn { get; set; }

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
