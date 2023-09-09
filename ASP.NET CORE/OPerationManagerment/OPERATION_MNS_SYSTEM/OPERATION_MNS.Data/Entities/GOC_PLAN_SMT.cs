using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("GOC_PLAN_SMT")]
    public class GOC_PLAN_SMT : DomainEntity<int>, IDateTracking
    {
        public GOC_PLAN_SMT()
        {

        }

        [StringLength(50)]
        public string MesItemId { get; set; } // MATERIAL ID

        [StringLength(50)]
        public string MasterialName { get; set; }

        [StringLength(50)]
        public string Erp_Item_Id { get; set; }

        [StringLength(50)]
        public string Group1 { get; set; }

        [StringLength(50)]
        public string Group2 { get; set; }

        [StringLength(50)]
        public string Group3 { get; set; }

        [StringLength(50)]
        public string Size { get; set; }

        [StringLength(50)]
        public string OperationId { get; set; }

        [StringLength(50)]
        public string OperationName { get; set; }

        [StringLength(50)]
        public string Sales_approval { get; set; }

        [StringLength(50)]
        public string Module_model { get; set; }

        [StringLength(50)]
        public string MonthPlan { get; set; }

        [StringLength(50)]
        public string DatePlan { get; set; }

        [StringLength(50)]
        public string WeekPlan { get; set; }

        public double QuantityPlan { get; set; }

        public double QuantityActual { get; set; }

        public double QuantityGap { get; set; }

        [StringLength(50)]
        public string Unit { get; set; } // chip , wafe

        [StringLength(50)]
        public string DanhMuc { get; set; } // KHSX , DEMAND 

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
