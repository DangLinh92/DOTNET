using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("ACTUAL_DAILY_VIEW")]
    public class ACTUAL_DAILY_VIEW : DomainEntity<int>, IDateTracking
    {
        public ACTUAL_DAILY_VIEW()
        {

        }

        public ACTUAL_DAILY_VIEW(int id,string model_goc, string material_mes, string department,
           string dateActual, float qty_ShippingWait, float qty_PostOperationShipping,string unit)
        {
            Id = id;
            Model_GOC = model_goc;
            Material_MES = material_mes;
            Department = department;
            DateActual = dateActual;
            Qty_ShippingWait = qty_ShippingWait;
            Qty_PostOperationShipping = qty_PostOperationShipping;
            Unit = unit;
        }

        [StringLength(50)]
        public string Model_GOC { get; set; }

        [StringLength(50)]
        public string Material_MES { get; set; }

        [StringLength(50)]
        public string Department { get; set; }

        [StringLength(50)]
        public string DateActual { get; set; }

        public float Qty_ShippingWait { get; set; }

        public float Qty_PostOperationShipping { get; set; }

        [StringLength(50)]
        public string Unit { get; set; } // Sheet , Kea

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
