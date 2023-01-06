using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels
{
    public class InventoryActualViewModel
    {
        public string Category { get; set; }
        public string Series { get; set; }
        public string Model { get; set; }
        public string Material { get; set; }
        public decimal Total { get; set; }
        public decimal CassetteInputStock_Pre { get; set; }
        public decimal PreOperationWaiting { get; set; }
        public decimal WaitMarkingIDCHK { get; set; }
        public decimal Input_wafer_inspection { get; set; }
        public decimal Wall_PR_Wafer_inspection { get; set; }
        public decimal Wall_PR { get; set; }
        public decimal EBR { get; set; }
        public decimal Wall_Mask_Cleaning { get; set; }
        public decimal Wall_Photo { get; set; }
        public decimal Wall_Develop { get; set; }
        public decimal Wall_Oven { get; set; }
        public decimal Wall_PR_Measure { get; set; }
        public decimal Wall_Ashing_Waiting { get; set; }
        public decimal Wall_Inspection { get; set; }
        public decimal Wall_Ashing { get; set; }
        public decimal Before_Roof_Lami_Wafer_Inspection { get; set; }
        public decimal Roof_Laminating { get; set; }
        public decimal After_Roof_Lami_Visual_Inspection { get; set; }
        public decimal Roof_Hardening { get; set; }
        public decimal Roof_Mask_Cleaning { get; set; }
        public decimal Roof_Photo { get; set; }
        public decimal Roof_Remover { get; set; }
        public decimal Roof_Oven_PEB { get; set; }
        public decimal Roof_Develop { get; set; }
        public decimal After_Roof_Develop_Visual_Inspection { get; set; }
        public decimal Roof_Ashing { get; set; }
        public decimal Roof_QDR { get; set; }
        public decimal Roof_Oven { get; set; }
        public decimal Wafer_Sorting { get; set; }
        public decimal Roof_Measure { get; set; }
        public decimal Roof_BST { get; set; }
        public decimal Roof_Inspection { get; set; }
        public decimal Seed_Deposition { get; set; }
        public decimal Before_Plate_PR_Wafer_Inspection { get; set; }
        public decimal Plate_Patterning_PR { get; set; }
        public decimal Plate_Patterning_Mask_Cleaning { get; set; }
        public decimal Plate_Patterning_Photo { get; set; }
        public decimal Plate_Patterning_Develop { get; set; }
        public decimal After_Plate_Develop_Visual_Inspection { get; set; }
        public decimal Plate_Patterning_Measure { get; set; }
        public decimal Plate_Patterning_PR_Ashing { get; set; }
        public decimal Plate_Patterning_Inspection { get; set; }
        public decimal Plating_Input_Waiting { get; set; }
        public decimal Cu_Sn_Plating { get; set; }
        public decimal St_Plate_Visual_Inspection { get; set; }
        public decimal SN_Plate_Measure { get; set; }
        public decimal PR_Strip_Cu_Etching { get; set; }
        public decimal Nd_Plate_Visual_Inspection { get; set; }
        public decimal Ti_ething { get; set; }
        public decimal Plate_Measure { get; set; }
        public decimal Plate_BST { get; set; }
        public decimal Plate_Inspection_Wait { get; set; }
        public decimal Plate_Inspection { get; set; }
        public decimal Probe_Waite { get; set; }
        public decimal Wafer_Probe_RF { get; set; }
        public decimal Wafer_Probe_IR { get; set; }
        public decimal Shipping_Wait { get; set; }
        public decimal Post_Operation_Shipping { get; set; }
        public string Unit { get; set; } // chip , wafe
        public string Material_SAP_CODE { get; set; }
        public string LastUpdate { get; set; }
    }
}
