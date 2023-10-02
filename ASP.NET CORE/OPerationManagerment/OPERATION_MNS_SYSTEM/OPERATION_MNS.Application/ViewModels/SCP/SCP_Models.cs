using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels.SCP
{
    public class CSPBaseResult
    {
        public string sessionAcquired { get; set; }
        public string sessionExpired { get; set; }      
        public string authtoken { get; set; }
        public string sessionRemainder { get; set; }
        public string sessionCurrent { get; set; }
    }

    public class MasterIdModel : CSPBaseResult
    {
        public string code { get; set; }
      
        public string name { get; set; }
    }

    public class UnitCodeItem : CSPBaseResult
    {
        public int code { get; set; }

        public int name { get; set; }
    }

    public class PlanIdModel : MasterIdModel
    {
        public string planEndDt { get; set; }
        public string planStartDt { get; set; }
        public int sortOrder { get; set; }
    }

    public class PlowItem
    {
        public double actualYield { get; set; }
        public double leadTime { get; set; }
        public string sessionCurrent { get; set; }
        public double operationSeq { get; set; }
        public object flowAttb2 { get; set; }
        public string routeId { get; set; }
        public object flowAttb1 { get; set; }
        public object flowAttb4 { get; set; }
        public object flowAttb3 { get; set; }
        public object flowAttb5 { get; set; }
        public string operationId { get; set; }
        public string planId { get; set; }
        public double postBufferTime { get; set; }
        public double runTime { get; set; }
        public string sessionRemainder { get; set; }
        public string flowId { get; set; }
        public string corporation { get; set; }
        public int isValid { get; set; }
        public string timeUom { get; set; }
        public string authtoken { get; set; }
        public string operationName { get; set; }
        public double preBufferTime { get; set; }
        public string masterId { get; set; }
        public object descr { get; set; }
        public string sessionAcquired { get; set; }
        public string plant { get; set; }
        public string sessionExpired { get; set; }
        public string siteId { get; set; }
        public int isStock { get; set; }
    }

    public class SalesApprovalItem
    {
        public string salesApprovalCode { get; set; }
        public string mesItemId { get; set; }
        public string productSiteId { get; set; }
        public string updateDttm { get; set; }
        public string authtoken { get; set; }
        public string insertDttm { get; set; }
        public string sessionCurrent { get; set; }
        public string masterId { get; set; }
        public object descr { get; set; }
        public string updateId { get; set; }
        public string erpItemId { get; set; }
        public int isManufacture { get; set; }
        public string sessionAcquired { get; set; }
        public string sessionExpired { get; set; }
        public string planId { get; set; }
        public string sessionRemainder { get; set; }
        public string insertId { get; set; }
    }

    public class ProductMixItem
    {
        public string endDate { get; set; }
        public string orgStartDate { get; set; }
        public string orgProductMixId { get; set; }
        public string type { get; set; }
        public string sessionCurrent { get; set; }
        public string updateId { get; set; }
        public string uom { get; set; }
        public string operationId { get; set; }
        public string planId { get; set; }
        public string sessionRemainder { get; set; }
        public string productMixId { get; set; }
        public string corporation { get; set; }
        public object isValid { get; set; }
        public object updateDttm { get; set; }
        public string authtoken { get; set; }
        public string operationName { get; set; }
        public object insertDttm { get; set; }
        public string bucket { get; set; }
        public string masterId { get; set; }
        public string orgSiteId { get; set; }
        public string sessionAcquired { get; set; }
        public string plant { get; set; }
        public double qty { get; set; }
        public string sessionExpired { get; set; }
        public string siteId { get; set; }
        public string typeValue { get; set; }
        public string startDate { get; set; }
        public string orgOperationId { get; set; }
        public string insertId { get; set; }
    }

    public class SCPPlanBomItem
    {
        public string childItemNm { get; set; }
        public string sessionCurrent { get; set; }
        public string isMoveTimeLocked { get; set; }
        public string updateId { get; set; }
        public string routeId { get; set; }
        public string parentItemId { get; set; }
        public string operationId { get; set; }
        public string planId { get; set; }
        public string sessionRemainder { get; set; }
        public string childItemId { get; set; }
        public string mesMaterialId { get; set; }
        public object salesApprovalCode { get; set; }
        public double productionRate { get; set; }
        public string corporation { get; set; }
        public string isValid { get; set; }
        public long updateDttm { get; set; }
        public string authtoken { get; set; }
        public string operationName { get; set; }
        public double inMoveTime { get; set; }
        public long insertDttm { get; set; }
        public string bomVersionId { get; set; }
        public string parentItemNm { get; set; }
        public string masterId { get; set; }
        public double outMoveTime { get; set; }
        public string moveTimeUom { get; set; }
        public string sessionAcquired { get; set; }
        public string plant { get; set; }
        public string sessionExpired { get; set; }
        public string siteId { get; set; }
        public double consumeRate { get; set; }
        public string insertId { get; set; }
    }

    public class SiteCalendarItem
    {
        public string corporation { get; set; }
        public string timeUom { get; set; }
        public string authtoken { get; set; }
        public string sessionCurrent { get; set; }
        public string masterId { get; set; }
        public string workDate { get; set; }
        public double onTime { get; set; }
        public string sessionAcquired { get; set; }
        public string plant { get; set; }
        public string sessionExpired { get; set; }
        public string siteId { get; set; }
        public string planId { get; set; }
        public string sessionRemainder { get; set; }
    }

    public class MaterialPlanItem
    {
        public string mesControllerName { get; set; }
        public string meaCd { get; set; }
        public string year { get; set; }
        public string pweek { get; set; }
        public string authtoken { get; set; }
        public string itemGroupId { get; set; }
        public string sessionCurrent { get; set; }
        public string masterId { get; set; }
        public string itemId { get; set; }
        public string itemName { get; set; }
        public string itemAttb14 { get; set; }
        public string month { get; set; }
        public double? receiptQty { get; set; }
        public string sessionAcquired { get; set; }
        public double? qty { get; set; }
        public string sessionExpired { get; set; }
        public double? onHandQty { get; set; }
        public int categoryNo { get; set; }
        public string categoryCd { get; set; }
        public string siteId { get; set; }
        public string planId { get; set; }
        public string sessionRemainder { get; set; }
        public double? preBuildQty { get; set; }
        public string day { get; set; }
    }

    public class ProductionPlanItem
    {
        public string mesItemId { get; set; }
        public string year { get; set; }
        public string pweek { get; set; }
        public string sessionCurrent { get; set; }
        public string itemName { get; set; }
        public string moduleModel { get; set; }
        public string operationId { get; set; }
        public string planId { get; set; }
        public string sessionRemainder { get; set; }
        public string customerGroup { get; set; }
        public string authtoken { get; set; }
        public string group3 { get; set; }
        public string operationName { get; set; }
        public string group2 { get; set; }
        public string group1 { get; set; }
        public string masterId { get; set; }
        public string itemId { get; set; }
        public string salesApproval { get; set; }
        public string size { get; set; }
        public string month { get; set; }
        public string sessionAcquired { get; set; }
        public double? qty { get; set; }
        public string sessionExpired { get; set; }
        public string siteId { get; set; }
        public string customer { get; set; }
    }

    public class DemandFulFillMentItem
    {
        public string customerGroupId { get; set; }
        public string toSiteId { get; set; }
        public string year { get; set; }
        public string pweek { get; set; }
        public string productSize { get; set; }
        public string authtoken { get; set; }
        public string salesModelId { get; set; }
        public string fromSiteId { get; set; }
        public string sessionCurrent { get; set; }
        public string masterId { get; set; }
        public string itemId { get; set; }
        public string itemName { get; set; }
        public string month { get; set; }
        public string productGroup { get; set; }
        public string sessionAcquired { get; set; }
        public double? qty { get; set; }
        public string sessionExpired { get; set; }
        public string planId { get; set; }
        public string sessionRemainder { get; set; }
        public string category { get; set; }
    }

    public class SCPInventory
    {
        public string salesApprovalId { get; set; }
        public string salesOrderId { get; set; }
        public string mesItemId { get; set; }
        public string sessionCurrent { get; set; }
        public object onhandDate { get; set; }
        public string salesModel { get; set; }
        public string invLocation { get; set; }
        public string planId { get; set; }
        public string sessionRemainder { get; set; }
        public double seq { get; set; }
        public string toSiteId { get; set; }
        public string corporation { get; set; }
        public string authtoken { get; set; }
        public string group3 { get; set; }
        public string lotId { get; set; }
        public string group2 { get; set; }
        public object customerName { get; set; }
        public string group1 { get; set; }
        public string bomVersion { get; set; }
        public string masterId { get; set; }
        public string itemId { get; set; }
        public string descr { get; set; }
        public string size { get; set; }
        public string sessionAcquired { get; set; }
        public string plant { get; set; }
        public double qty { get; set; }
        public string sessionExpired { get; set; }
        public string siteId { get; set; }
    }

    public class SCP_WIP
    {
        public long releaseDttm { get; set; }
        public string isUse { get; set; }
        public double completeQty { get; set; }
        public string resourceId { get; set; }
        public string endDate { get; set; }
        public string sessionCurrent { get; set; }
        public string tarUom { get; set; }
        public string updateId { get; set; }
        public string uom { get; set; }
        public string routeId { get; set; }
        public string tarRouteId { get; set; }
        public string tarOperationId { get; set; }
        public string operationId { get; set; }
        public string planId { get; set; }
        public string sessionRemainder { get; set; }
        public object subResourceId { get; set; }
        public string mesMaterialId { get; set; }
        public double operationQty { get; set; }
        public string corporation { get; set; }
        public object wipStatus { get; set; }
        public long updateDttm { get; set; }
        public string authtoken { get; set; }
        public string group3 { get; set; }
        public string lotId { get; set; }
        public double wipSeq { get; set; }
        public string tarItemId { get; set; }
        public long insertDttm { get; set; }
        public string group2 { get; set; }
        public string group1 { get; set; }
        public double tarQty { get; set; }
        public string masterId { get; set; }
        public string itemId { get; set; }
        public object descr { get; set; }
        public string size { get; set; }
        public string sessionAcquired { get; set; }
        public string plant { get; set; }
        public string sessionExpired { get; set; }
        public string siteId { get; set; }
        public string tarOperationName { get; set; }
        public string workOrderId { get; set; }
        public string tarSiteId { get; set; }
        public string startDate { get; set; }
        public string insertId { get; set; }
    }
}
