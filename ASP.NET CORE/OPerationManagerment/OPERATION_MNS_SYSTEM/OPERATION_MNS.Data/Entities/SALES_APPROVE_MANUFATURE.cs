using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("SALES_APPROVE_MANUFATURE")]
    public class SALES_APPROVE_MANUFATURE : DomainEntity<int>, IDateTracking
    {
        [StringLength(50)]
        public string PlanID { get; set; }

        [StringLength(50)]
        public string MasterID { get; set; }

        [StringLength(50)]
        public string SizeID { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        [StringLength(50)]
        public string Size { get; set; }

        [StringLength(50)]
        public string SalesApprovalCode { get; set; }

        [StringLength(50)]
        public string ERPCode { get; set; }

        [StringLength(50)]
        public string MesCode { get; set; }

        public bool IsManufacture { get; set; }

        [StringLength(50)]
        public string ItemName { get; set; }

        [StringLength(50)]
        public string SMTPoint { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }
    }

    [Table("OPERATION_STANDARD_INFO")]
    public class OPERATION_STANDARD_INFO : DomainEntity<int>
    {
        public string OperationID { get; set; }
        public string OperationName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Description { get; set; }
    }

    [Table("PRODUCT_MIX_CAPA")]
    public class PRODUCT_MIX_CAPA : DomainEntity<int>, IDateTracking
    {
        [StringLength(50)]
        public string PlanID { get; set; }

        [StringLength(50)]
        public string MasterID { get; set; }

        [StringLength(50)]
        public string SizeID { get; set; }

        [StringLength(50)]
        public string OperationID { get; set; }

        [StringLength(50)]
        public string OperationName { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        [StringLength(50)]
        public string TyleValue { get; set; }

        [StringLength(150)]
        public string PMixID { get; set; }

        public double Qty { get; set; }

        [StringLength(50)]
        public string UOM { get; set; }

        [StringLength(50)]
        public string Bucket { get; set; }

        [StringLength(50)]
        public string Start { get; set; }

        [StringLength(50)]
        public string End { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }
    }

    [Table("SCP_PLAN_BOM")]
    public class SCP_PLAN_BOM : DomainEntity<int>, IDateTracking
    {
        [StringLength(50)]
        public string PlanID { get; set; }

        [StringLength(50)]
        public string MasterID { get; set; }

        [StringLength(50)]
        public string SizeID { get; set; }

        [StringLength(150)]
        public string Version { get; set; }

        [StringLength(50)]
        public string SalesApprovalCode { get; set; }

        [StringLength(50)]
        public string MesMaterialID { get; set; }

        [StringLength(50)]
        public string ProduceItem { get; set; }

        [StringLength(50)]
        public string ItemName { get; set; }

        [StringLength(50)]
        public string ConsumeItem { get; set; }

        [StringLength(150)]
        public string ConsumeName { get; set; }

        public double ConsumeRate { get; set; }

        [StringLength(50)]
        public string Route { get; set; }

        [StringLength(50)]
        public string Operation { get; set; }

        [StringLength(50)]
        public string OperationName { get; set; }

        [StringLength(5)]
        public string Valid { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }
    }

    [Table("OPERATION_FLOW")]
    public class OPERATION_FLOW : DomainEntity<int>, IDateTracking
    {
        [StringLength(50)]
        public string PlanID { get; set; }

        [StringLength(50)]
        public string MasterID { get; set; }

        [StringLength(50)]
        public string SizeID { get; set; }

        [StringLength(50)]
        public string MaterialId { get; set; }

        [StringLength(50)]
        public string Route { get; set; }

        public int OperationSEQ { get; set; }

        [StringLength(50)]
        public string Operation { get; set; }

        [StringLength(50)]
        public string OperationName { get; set; }

        public double? ActualYield { get; set; }
        public double? PreBufferTime { get; set; }
        public double? RuntTime { get; set; }
        public double? PostBufferTime { get; set; }

        [StringLength(50)]
        public string TimeUOM { get; set; }

        public double LeadTimeDay { get; set; }

        public bool? IsStock { get; set; }

        [StringLength(250)]
        public string Desc { get; set; }

        public bool Valid { get; set; }

        [StringLength(50)]
        public string ATTB1 { get; set; }

        [StringLength(50)]
        public string ATTB2 { get; set; }

        [StringLength(50)]
        public string ATTB3 { get; set; }

        [StringLength(50)]
        public string ATTB4 { get; set; }

        [StringLength(50)]
        public string ATTB5 { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }
    }

    [Table("SITE_CALENDAR")]
    public class SITE_CALENDAR : DomainEntity<int>, IDateTracking
    {
        [StringLength(50)]
        public string PlanID { get; set; }

        [StringLength(50)]
        public string MasterID { get; set; }

        [StringLength(50)]
        public string SizeID { get; set; }

        [StringLength(50)]
        public string WorkDate { get; set; }

        public double? OnTime { get; set; }

        [StringLength(50)]
        public string TimeOUM { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }
    }

    [Table("MATERIALS_PLAN")]
    public class MATERIALS_PLAN : DomainEntity<int>, IDateTracking
    {
        [StringLength(50)]
        public string PlanID { get; set; }

        [StringLength(50)]
        public string MasterID { get; set; }

        [StringLength(50)]
        public string SizeID { get; set; }

        [StringLength(50)]
        public string ItemID { get; set; }

        [StringLength(50)]
        public string ItemName { get; set; }

        [StringLength(50)]
        public string ItemGR { get; set; }

        [StringLength(50)]
        public string ControllerName { get; set; }

        [StringLength(500)]
        public string ModuleModel { get; set; }

        public double? OnHand { get; set; }

        public double? PreBuld { get; set; }

        public double AsumeBuld { get; set; }

        [StringLength(50)]
        public string CategoryCD { get; set; }

        [StringLength(50)]
        public string Year { get; set; }

        [StringLength(50)]
        public string Month { get; set; }

        [StringLength(50)]
        public string Week { get; set; }

        [StringLength(50)]
        public string Day { get; set; }

        public double Qty { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }
    }

    [Table("PRODUCTION_PLAN")]
    public class PRODUCTION_PLAN : DomainEntity<int>, IDateTracking
    {
        [StringLength(50)]
        public string PlanID { get; set; }

        [StringLength(50)]
        public string MasterID { get; set; }

        [StringLength(50)]
        public string SizeID { get; set; }

        [StringLength(50)]
        public string CustomerGR { get; set; }

        [StringLength(50)]
        public string Customer { get; set; }

        [StringLength(50)]
        public string MesItemID { get; set; }

        [StringLength(50)]
        public string ERPItemID { get; set; }

        [StringLength(50)]
        public string Group1 { get; set; }

        [StringLength(50)]
        public string Group2 { get; set; }

        [StringLength(50)]
        public string Group3 { get; set; }

        [StringLength(50)]
        public string Size { get; set; }

        [StringLength(50)]
        public string OperationID { get; set; }

        [StringLength(50)]
        public string OperationName { get; set; }

        [StringLength(50)]
        public string SalesApproval { get; set; }

        [StringLength(50)]
        public string ModuleModel { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string Year { get; set; }

        [StringLength(50)]
        public string Month { get; set; }

        [StringLength(50)]
        public string Week { get; set; }

        public double Qty { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }
    }
}
