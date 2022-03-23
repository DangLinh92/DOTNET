using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VOC.Application.ViewModels.VOC
{
    public class VOC_DefectTypeViewModel
    {
        public int Id { get; set; }
        
        [StringLength(250)]
        public string EngsNotation { get; set; }

        [StringLength(250)]
        public string KoreanNotation { get; set; }

        public string DateCreated { get; set; }
        public string DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }
    }
}
