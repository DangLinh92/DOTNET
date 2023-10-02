using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("GOC_PRODUCTION_PLAN_LFEM")]
    public class GOC_PRODUCTION_PLAN_LFEM : DomainEntity<int>, IDateTracking
    {
        public GOC_PRODUCTION_PLAN_LFEM()
        {

        }

        [StringLength(50)]
        public string MesItemId { get; set; } // MATERIAL ID

        [StringLength(50)]
        public string MonthPlan { get; set; }

        [StringLength(50)]
        public string DatePlan { get; set; }

        [StringLength(50)]
        public string WeekPlan { get; set; }

        // KHSX
        public double QuantityPlan_KHSX { get; set; }

        public double QuantityActual_KHSX { get; set; }

        public double QuantityGap_KHSX { get; set; }

        // KHXH/DEMAND
        public double QuantityPlan_DEMAND { get; set; }

        public double QuantityActual_DEMAND { get; set; }

        public double QuantityGap_DEMAND { get; set; }

        // MODULE STOCK
        public double QuantityPlan_STOCK { get; set; }

        public double QuantityActual_STOCK { get; set; }

        public double QuantityGap_STOCK { get; set; }

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
