using OPERATION_MNS.Application.ViewModels.SCP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OPERATION_MNS.Application.Interfaces
{
    public interface ISCPService
    {
        Task<SCPLoginModel> SCP_LoginData();
        Task<MasterIdModel> GetMasterIdCode();
        Task<List<MasterIdModel>> GetSiteIdCode();
        Task<List<UnitCodeItem>> GetUnitCode();
        Task<List<PlanIdModel>> GetPlanIdCode();
        Task<List<PlowItem>> GetFlowList(string masterID,string planID,string site);
        Task<List<SalesApprovalItem>> GetSalesApprovals(string masterID,string planID,string site);
        Task<List<ProductMixItem>> GetProductMixOperationCAPA(string masterID,string planID,string site);
        Task<List<ProductMixItem>> GetProductMixKitSizeCAPA(string masterID,string planID,string site);
        Task<List<ProductMixItem>> GetProductMixMaterialCAPA(string masterID,string planID,string site);
        Task<List<SCPPlanBomItem>> GetSCPPlanBoms(string masterID,string planID,string site);
        Task<List<SiteCalendarItem>> GetSiteCalendarItems(string masterID,string planID,string site,string fromDate,string toDate);
        Task<List<MaterialPlanItem>> GetMaterialPlanItem(string masterID,string planID,string site,string fromDate,string toDate);
        Task<List<ProductionPlanItem>> GetProductionPlanItems(string masterID,string planID,string site,string fromDate,string toDate,string unit);
        Task<List<DemandFulFillMentItem>> GetDemandFulFillMentItems(string masterID,string planID,string site,string fromDate,string toDate);
        Task<List<SCPInventory>> GetSCPInventorys(string masterID,string planID,string site);
        Task<List<SCP_WIP>> GetSCP_WIPs(string masterID,string planID,string site);
    }
}
