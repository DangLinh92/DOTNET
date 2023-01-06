using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("GOC_STANDAR_QTY")]
    public class GOC_STANDAR_QTY : DomainEntity<int>, IDateTracking
    {
        public GOC_STANDAR_QTY()
        {

        }

        public GOC_STANDAR_QTY(int id,string module,string model,string material,string division,float standarQty,string dept,string unit)
        {
            Id = id;
            Module = module;
            Model = model;
            Material = material;
            Division = division;
            StandardQtyForMonth = standarQty;
            Department = dept;
            Unit = unit;
        }

        [StringLength(50)]
        public string Module { get; set; }

        // SAP CODE
        [StringLength(50)]
        public string Model { get; set; }

        [StringLength(50)]
        public string Material { get; set; }

        [StringLength(50)]
        public string Division { get; set; }

        public float StandardQtyForMonth { get; set; }

        [StringLength(50)]
        public string MonthBegin { get; set; }

        [StringLength(50)]
        public string Department { get; set; }

        [StringLength(50)]
        public string Unit { get; set; } // chip , wafe

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
