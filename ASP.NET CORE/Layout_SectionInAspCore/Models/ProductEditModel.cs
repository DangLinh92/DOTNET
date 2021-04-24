using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Layout_SectionInAspCore.Models
{
    public class ProductEditModel
    {
        [Required(AllowEmptyStrings = false,ErrorMessage = "ID is empty")]
        public int ID { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Enter Name Please!")]
        public string Name { get; set; }
        public decimal Rate { get; set; }
        public int Rating { get; set; }
    }
}
