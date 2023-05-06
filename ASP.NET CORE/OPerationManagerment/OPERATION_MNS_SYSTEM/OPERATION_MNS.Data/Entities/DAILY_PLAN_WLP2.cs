using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("DAILY_PLAN_WLP2")]
    public class DAILY_PLAN_WLP2 : DomainEntity<int>, IDateTracking
    {
        // SAP CODE
        [StringLength(50)]
        public string Model { get; set; }

        [StringLength(50)]
        public string DatePlan { get; set; }

        public float BackGrinding { get; set; }
        public float WaferOven { get; set; }
        public float Dicing { get; set; }
        public float ChipInspection { get; set; }
        public float Packing { get; set; }
        public float ReelInspection { get; set; }
        public float QC_Pass { get; set; }

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
