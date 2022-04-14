using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VOC.Application.ViewModels.VOC
{
    public class VocOnsiteViewModel
    {
        public int Id { get; set; }
        public int Month { get; set; }

        [StringLength(50)]
        public string Week { get; set; }

        [StringLength(50)]
        public string Date { get; set; }

        [StringLength(50)]
        public string Part { get; set; }

        [StringLength(50)]
        public string Customer_Code { get; set; }

        [StringLength(50)]
        public string Wisol_Model { get; set; }

        [StringLength(50)]
        public string Customer { get; set; }
        public int Qty { get; set; }

        [StringLength(250)]
        public string Marking { get; set; }

        [StringLength(50)]
        public string SetModel { get; set; }

        [StringLength(10)]
        public string OK { get; set; }

        [StringLength(10)]
        public string NG { get; set; }

        [StringLength(10)]
        public string Not_Measure { get; set; }

        [StringLength(10)]
        public string Result { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        [StringLength(50)]
        public string ProductionDate { get; set; }

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
