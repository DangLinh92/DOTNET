using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("DAILY_PLAN_DATA_LFEM")]
    public class DAILY_PLAN_DATA_LFEM : DomainEntity<int>, IDateTracking
    {
        public DAILY_PLAN_DATA_LFEM()
        {

        }

        [StringLength(50)]
        public string Model { get; set; }

        [StringLength(50)]
        public string MesItemId { get; set; } // MATERIAL ID

        // TỒN KHO
        public double? Dam_WIP { get; set; }
        public double? Mold_WIP { get; set; }
        public double? Grinding_WIP { get; set; }
        public double? Marking_WIP { get; set; }
        public double? Dicing_WIP { get; set; }
        public double? Test_WIP { get; set; }
        public double? VisualInspection_WIP { get; set; }
        public double? OQC_WIP { get; set; }

        // Plan
        public double? Dam_KHSX { get; set; }
        public double? Mold_KHSX { get; set; }
        public double? Grinding_KHSX { get; set; }
        public double? Marking_KHSX { get; set; }
        public double? Dicing_KHSX { get; set; }
        public double? Test_KHSX { get; set; }
        public double? VisualInspection_KHSX { get; set; }
        public double? OQC_KHSX { get; set; }

        // Thưc tế sản xuất
        public double? Dam_PROD { get; set; }
        public double? Mold_PROD { get; set; }
        public double? Grinding_PROD { get; set; }
        public double? Marking_PROD { get; set; }
        public double? Dicing_PROD { get; set; }
        public double? Test_PROD { get; set; }
        public double? VisualInspection_PROD { get; set; }
        public double? OQC_PROD { get; set; }

        // Ngày
        [StringLength(50)]
        public string DateActual { get; set; }

        [StringLength(50)]
        public string WeekActual { get; set; }

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
