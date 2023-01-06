using OPERATION_MNS.Data.Enums;
using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("YIELD_OF_MODEL")]
    public class YIELD_OF_MODEL : DomainEntity<int>, IDateTracking
    {
        public YIELD_OF_MODEL(string model, decimal yieldPlan, decimal yieldActual, decimal yieldGap, string month)
        {
            Model = model;
            YieldPlan = yieldPlan;
            YieldActual = yieldActual;
            YieldGap = yieldGap;
            Month = month;
        }

        [Required]
        [StringLength(50)]
        public string Model { get; set; }

        public decimal YieldPlan { get; set; }
        public decimal YieldActual { get; set; }
        public decimal YieldGap { get; set; }

        [StringLength(50)]
        public string Month { get; set; }

        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }
    }
}
