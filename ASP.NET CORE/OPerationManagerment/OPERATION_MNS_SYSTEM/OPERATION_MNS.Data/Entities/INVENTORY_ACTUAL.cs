using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("INVENTORY_ACTUAL")]
    public class INVENTORY_ACTUAL : DomainEntity<int>, IDateTracking
    {
        public INVENTORY_ACTUAL()
        {

        }

        public INVENTORY_ACTUAL(int id, string unit)
        {
            Id = id;
            Unit = unit;
        }

        [StringLength(50)]
        public string Category { get; set; }
        [StringLength(50)]
        public string Series { get; set; }
        [StringLength(50)]
        public string Model { get; set; }
        [StringLength(50)]
        public string Material { get; set; }
        public float Total { get; set; }
        public float CassetteInputStock_Pre { get; set; }
        public float PreOperationWaiting { get; set; }
        public float WaitMarkingIDCHK { get; set; }
        public float Input_wafer_inspection { get; set; }
        public float Wall_PR_Wafer_inspection { get; set; }
        public float Wall_PR { get; set; }
        public float EBR { get; set; }
        public float Wall_Mask_Cleaning { get; set; }
        public float Wall_Photo { get; set; }
        public float Wall_Develop { get; set; }
        public float Wall_Oven { get; set; }
        public float Wall_PR_Measure { get; set; }
        public float Wall_Ashing_Waiting { get; set; }
        public float Wall_Inspection { get; set; }
        public float Wall_Ashing { get; set; }
        public float Before_Roof_Lami_Wafer_Inspection { get; set; }
        public float Roof_Laminating { get; set; }
        public float After_Roof_Lami_Visual_Inspection { get; set; }
        public float Roof_Hardening { get; set; }
        public float Roof_Mask_Cleaning { get; set; }
        public float Roof_Photo { get; set; }
        public float Roof_Remover { get; set; }
        public float Roof_Oven_PEB { get; set; }
        public float Roof_Develop { get; set; }
        public float After_Roof_Develop_Visual_Inspection { get; set; }
        public float Roof_Ashing { get; set; }
        public float Roof_QDR { get; set; }
        public float Roof_Oven { get; set; }
        public float Wafer_Sorting { get; set; }
        public float Roof_Measure { get; set; }
        public float Roof_BST { get; set; }
        public float Roof_Inspection { get; set; }
        public float Seed_Deposition { get; set; }
        public float Before_Plate_PR_Wafer_Inspection { get; set; }
        public float Plate_Patterning_PR { get; set; }
        public float Plate_Patterning_Mask_Cleaning { get; set; }
        public float Plate_Patterning_Photo { get; set; }
        public float Plate_Patterning_Develop { get; set; }
        public float After_Plate_Develop_Visual_Inspection { get; set; }
        public float Plate_Patterning_Measure { get; set; }
        public float Plate_Patterning_PR_Ashing { get; set; }
        public float Plate_Patterning_Inspection { get; set; }
        public float Plating_Input_Waiting { get; set; }
        public float Cu_Sn_Plating { get; set; }
        public float St_Plate_Visual_Inspection { get; set; }
        public float SN_Plate_Measure { get; set; }
        public float PR_Strip_Cu_Etching { get; set; }
        public float Nd_Plate_Visual_Inspection { get; set; }
        public float Ti_ething { get; set; }
        public float Plate_Measure { get; set; }
        public float Plate_BST { get; set; }
        public float Plate_Inspection_Wait { get; set; }
        public float Plate_Inspection { get; set; }
        public float Probe_Waite { get; set; }
        public float Wafer_Probe_RF { get; set; }
        public float Wafer_Probe_IR { get; set; }
        public float Shipping_Wait { get; set; }
        public float Post_Operation_Shipping { get; set; }

        [StringLength(50)]
        public string Unit { get; set; } // chip , wafe

        [StringLength(50)]
        public string Date { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [StringLength(50)]
        public string Material_SAP_CODE { get; set; }
    }
}
