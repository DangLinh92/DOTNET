using OPERATION_MNS.Data.Enums;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("MATERIAL_TO_SAP")]
    public class MATERIAL_TO_SAP : DomainEntity<int>
    {
        public MATERIAL_TO_SAP()
        {

        }

        public MATERIAL_TO_SAP(string material,string sapcode,string department)
        {
            Material = material;
            SAP_Code = sapcode;
            Department = department;
        }

        [Required]
        [StringLength(50)]
        public string Material { get; set; }

        [Required]
        [StringLength(50)]
        public string SAP_Code { get; set; }

        [Required]
        [StringLength(50)]
        public string Department { get; set; }
    }
}
