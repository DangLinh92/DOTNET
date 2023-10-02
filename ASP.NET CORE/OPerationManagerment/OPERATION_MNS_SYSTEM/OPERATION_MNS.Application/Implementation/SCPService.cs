using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OPERATION_MNS.Application.HttpClients;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels.SCP;
using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using System.Numerics;
using System.Security.Policy;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OPERATION_MNS.Application.Implementation
{
    public class SCPService : BaseService, ISCPService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private IConfiguration _configuration;
        public SCPService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        /// <summary>
        /// Get operation flow
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<PlowItem>> GetFlowList(string masterID, string planID, string site)
        {
            try
            {
                if (CommonConstants.AUTHTOKEN.NullString() == "")
                {
                    return null;
                }

                var query = new Dictionary<string, string>()
                {
                    ["filterMasterId"] = masterID,
                    ["filterPlanId"] = planID,
                    ["filterSiteId"] = site,
                    ["filterPopSiteType"] = "ALL",
                    ["filterPopOperationType"] = "ALL",
                    ["startPage"] = "1",
                    ["endPage"] = "1000000000",
                    ["divisionCd"] = "WISOL",
                    ["siteCd"] = "KR",
                    ["product"] = "SCM",
                    ["locale"] = "KR",
                    ["authtoken"] = CommonConstants.AUTHTOKEN.NullString()
                };

                string url = "http://10.20.10.238:3622/ngs/request/com.thirautech.ngs.scm.scp.md.metainfo.flowlist.get_FlowList.dummy";
                var uri = QueryHelpers.AddQueryString(url, query);

                var request = new HttpRequestMessage(HttpMethod.Get, uri);

                string jscontent = "";
                request.Content = new JsonContent(jscontent, Encoding.UTF8);

                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    List<PlowItem> PlowItems = JsonSerializer.Deserialize<List<PlowItem>>(responseString);

                    if (PlowItems.Count > 0)
                        CommonConstants.AUTHTOKEN = PlowItems[0].authtoken;

                    return PlowItems;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// get master Id 
        /// </summary>
        /// <param name="authtoken"></param>
        /// <returns>SCP</returns>
        public async Task<MasterIdModel> GetMasterIdCode()
        {
            try
            {
                if (CommonConstants.AUTHTOKEN.NullString() == "")
                {
                    return null;
                }

                string url = "http://10.20.10.238:3622/ngs/request/com.thirautech.ngs.scm.scp.common.common.get_MasterIdCode2_code.dummy?divisionCd=WISOL&siteCd=KR&userId=&authId=&product=SCM&locale=KR&errCode=&errMsg=&authtoken=" + CommonConstants.AUTHTOKEN.NullString();

                var request = new HttpRequestMessage(HttpMethod.Get, url);

                string jscontent = "";
                request.Content = new JsonContent(jscontent, Encoding.UTF8);

                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    List<MasterIdModel> masterIdModel = JsonSerializer.Deserialize<List<MasterIdModel>>(responseString);

                    if (masterIdModel.Count > 0)
                        CommonConstants.AUTHTOKEN = masterIdModel[0].authtoken;

                    return masterIdModel[0];
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Get plan ID
        /// </summary>
        /// <returns></returns>
        public async Task<List<PlanIdModel>> GetPlanIdCode()
        {
            try
            {
                if (CommonConstants.AUTHTOKEN.NullString() == "")
                {
                    return null;
                }

                string url = "http://10.20.10.238:3622/ngs/request/com.thirautech.ngs.scm.scp.common.common.get_PlanIdCode2_code.dummy?isDefault=Y&divisionCd=WISOL&siteCd=KR&userId=&authId=&product=SCM&locale=KR&errCode=&errMsg=&authtoken=" + CommonConstants.AUTHTOKEN.NullString();

                var request = new HttpRequestMessage(HttpMethod.Get, url);

                string jscontent = "";
                request.Content = new JsonContent(jscontent, Encoding.UTF8);

                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    List<PlanIdModel> planIds = JsonSerializer.Deserialize<List<PlanIdModel>>(responseString);

                    if (planIds.Count > 0)
                        CommonConstants.AUTHTOKEN = planIds[0].authtoken;

                    return planIds;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// PRODUCT MIX(OPERATION_CAPA)
        /// </summary>
        /// <param name="masterID"></param>
        /// <param name="planID"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<ProductMixItem>> GetProductMixOperationCAPA(string masterID, string planID, string site)
        {
            try
            {
                if (CommonConstants.AUTHTOKEN.NullString() == "")
                {
                    return null;
                }

                var query = new Dictionary<string, string>()
                {
                    ["filterMasterId"] = masterID,
                    ["filterPlanId"] = planID,
                    ["filterSiteId"] = site,
                    ["startPage"] = "1",
                    ["endPage"] = "1000000000",
                    ["divisionCd"] = "WISOL",
                    ["siteCd"] = "KR",
                    ["product"] = "SCM",
                    ["locale"] = "KR",
                    ["authtoken"] = CommonConstants.AUTHTOKEN.NullString()
                };

                string url = "http://10.20.10.238:3622/ngs/request/com.thirautech.ngs.scm.scp.md.masterinfo.productmixOp.get_productmixOp.dummy";
                var uri = QueryHelpers.AddQueryString(url, query);

                var request = new HttpRequestMessage(HttpMethod.Get, uri);

                string jscontent = "";
                request.Content = new JsonContent(jscontent, Encoding.UTF8);

                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    List<ProductMixItem> ProductMixItems = JsonSerializer.Deserialize<List<ProductMixItem>>(responseString);

                    if (ProductMixItems.Count > 0 && ProductMixItems[0].authtoken.NullString() != "")
                        CommonConstants.AUTHTOKEN = ProductMixItems[0].authtoken;

                    return ProductMixItems;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// PRODUCT MIX(KIT_SIZE_CAPA)
        /// </summary>
        /// <param name="masterID"></param>
        /// <param name="planID"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<ProductMixItem>> GetProductMixKitSizeCAPA(string masterID, string planID, string site)
        {
            try
            {
                if (CommonConstants.AUTHTOKEN.NullString() == "")
                {
                    return null;
                }

                var query = new Dictionary<string, string>()
                {
                    ["filterMasterId"] = masterID,
                    ["filterPlanId"] = planID,
                    ["filterSiteId"] = site,
                    ["siteId_input"] = site,
                    ["operationId_input"] = "OC420",
                    ["typeValue_input"] = "6941",
                    ["startPage"] = "1",
                    ["endPage"] = "1000000000",
                    ["divisionCd"] = "WISOL",
                    ["siteCd"] = "KR",
                    ["product"] = "SCM",
                    ["locale"] = "KR",
                    ["authtoken"] = CommonConstants.AUTHTOKEN.NullString()
                };

                string url = "http://10.20.10.238:3622/ngs/request/com.thirautech.ngs.scm.scp.md.masterinfo.productmixKit.get_productmixKit.dummy";
                var uri = QueryHelpers.AddQueryString(url, query);

                var request = new HttpRequestMessage(HttpMethod.Get, uri);

                string jscontent = "";
                request.Content = new JsonContent(jscontent, Encoding.UTF8);

                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    List<ProductMixItem> ProductMixItems = JsonSerializer.Deserialize<List<ProductMixItem>>(responseString);

                    if (ProductMixItems.Count > 0 && ProductMixItems[0].authtoken.NullString() != "")
                        CommonConstants.AUTHTOKEN = ProductMixItems[0].authtoken;

                    return ProductMixItems;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// PRODUCT MIX(MATERIAL_CAPA)
        /// </summary>
        /// <param name="masterID"></param>
        /// <param name="planID"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<ProductMixItem>> GetProductMixMaterialCAPA(string masterID, string planID, string site)
        {
            try
            {
                if (CommonConstants.AUTHTOKEN.NullString() == "")
                {
                    return null;
                }

                var query = new Dictionary<string, string>()
                {
                    ["filterMasterId"] = masterID,
                    ["filterPlanId"] = planID,
                    ["filterSiteId"] = site,
                    ["startPage"] = "1",
                    ["endPage"] = "1000000000",
                    ["divisionCd"] = "WISOL",
                    ["siteCd"] = "KR",
                    ["product"] = "SCM",
                    ["locale"] = "KR",
                    ["authtoken"] = CommonConstants.AUTHTOKEN.NullString()
                };

                string url = "http://10.20.10.238:3622/ngs/request/com.thirautech.ngs.scm.scp.md.masterinfo.productmixMat.get_productmixMat.dummy";
                var uri = QueryHelpers.AddQueryString(url, query);

                var request = new HttpRequestMessage(HttpMethod.Get, uri);

                string jscontent = "";
                request.Content = new JsonContent(jscontent, Encoding.UTF8);

                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    List<ProductMixItem> ProductMixItems = JsonSerializer.Deserialize<List<ProductMixItem>>(responseString);

                    if (ProductMixItems.Count > 0 && ProductMixItems[0].authtoken.NullString() != "")
                        CommonConstants.AUTHTOKEN = ProductMixItems[0].authtoken;

                    return ProductMixItems;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// SCP PLAN BOM
        /// </summary>
        /// <param name="masterID"></param>
        /// <param name="planID"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<SCPPlanBomItem>> GetSCPPlanBoms(string masterID, string planID, string site)
        {
            try
            {
                if (CommonConstants.AUTHTOKEN.NullString() == "")
                {
                    return null;
                }

                var query = new Dictionary<string, string>()
                {
                    ["filterMasterId"] = masterID,
                    ["filterPlanId"] = planID,
                    ["filterSiteId"] = site,
                    ["filterPopItemType"] = "ALL",
                    ["filterPopSiteType"] = "ALL",
                    ["filterPopOperationType"] = "ALL",
                    ["startPage"] = "1",
                    ["endPage"] = "1000000000",
                    ["divisionCd"] = "WISOL",
                    ["siteCd"] = "KR",
                    ["product"] = "SCM",
                    ["locale"] = "KR",
                    ["authtoken"] = CommonConstants.AUTHTOKEN.NullString()
                };

                string url = "http://10.20.10.238:3622/ngs/request/com.thirautech.ngs.scm.scp.md.masterinfo.bomlist.get_bomlist.dummy";
                var uri = QueryHelpers.AddQueryString(url, query);

                var request = new HttpRequestMessage(HttpMethod.Get, uri);

                string jscontent = "";
                request.Content = new JsonContent(jscontent, Encoding.UTF8);

                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    List<SCPPlanBomItem> SCPPlanBomItems = JsonSerializer.Deserialize<List<SCPPlanBomItem>>(responseString);

                    if (SCPPlanBomItems.Count > 0 && SCPPlanBomItems[0].authtoken.NullString() != "")
                        CommonConstants.AUTHTOKEN = SCPPlanBomItems[0].authtoken;

                    return SCPPlanBomItems;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// SITE CALENDAR
        /// </summary>
        /// <param name="masterID"></param>
        /// <param name="planID"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<SiteCalendarItem>> GetSiteCalendarItems(string masterID, string planID, string site, string fromDate, string toDate)
        {
            try
            {
                if (CommonConstants.AUTHTOKEN.NullString() == "")
                {
                    return null;
                }

                var query = new Dictionary<string, string>()
                {
                    ["filterMasterId"] = masterID,
                    ["filterPlanId"] = planID,
                    ["filterSiteId"] = site,
                    ["filterFromDate"] = fromDate,
                    ["filterToDate"] = toDate,
                    ["startPage"] = "1",
                    ["endPage"] = "1000000000",
                    ["divisionCd"] = "WISOL",
                    ["siteCd"] = "KR",
                    ["product"] = "SCM",
                    ["locale"] = "KR",
                    ["authtoken"] = CommonConstants.AUTHTOKEN.NullString()
                };

                string url = "http://10.20.10.238:3622/ngs/request/com.thirautech.ngs.scm.scp.md.resource.sitecalendar.get_sitecalendar.dummy";
                var uri = QueryHelpers.AddQueryString(url, query);

                var request = new HttpRequestMessage(HttpMethod.Get, uri);

                string jscontent = "";
                request.Content = new JsonContent(jscontent, Encoding.UTF8);

                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    List<SiteCalendarItem> SiteCalendarItems = JsonSerializer.Deserialize<List<SiteCalendarItem>>(responseString);

                    if (SiteCalendarItems.Count > 0 && SiteCalendarItems[0].authtoken.NullString() != "")
                        CommonConstants.AUTHTOKEN = SiteCalendarItems[0].authtoken;

                    return SiteCalendarItems;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// MATERIAL PLAN
        /// </summary>
        /// <param name="masterID"></param>
        /// <param name="planID"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<MaterialPlanItem>> GetMaterialPlanItem(string masterID, string planID, string site, string fromDate, string toDate)
        {
            try
            {
                if (CommonConstants.AUTHTOKEN.NullString() == "")
                {
                    return null;
                }

                var query = new Dictionary<string, string>()
                {
                    ["filterMasterId"] = masterID,
                    ["filterPlanId"] = planID,
                    ["filterSiteId"] = site,
                    ["filterFromDate"] = fromDate,
                    ["filterToDate"] = toDate,
                    ["filterDisplay"] = "DAY",
                    ["filterShortYN"] = "N",
                    ["filterPopItemType"] = "ALL",
                    ["startPage"] = "1",
                    ["endPage"] = "1000000000",
                    ["divisionCd"] = "WISOL",
                    ["siteCd"] = "KR",
                    ["product"] = "SCM",
                    ["locale"] = "KR",
                    ["authtoken"] = CommonConstants.AUTHTOKEN.NullString()
                };

                string url = "http://10.20.10.238:3622/ngs/request/com.thirautech.ngs.scm.scp.pr.result.materialplan.get_MaterialPlan.dummy";
                var uri = QueryHelpers.AddQueryString(url, query);

                var request = new HttpRequestMessage(HttpMethod.Get, uri);

                string jscontent = "";
                request.Content = new JsonContent(jscontent, Encoding.UTF8);

                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    List<MaterialPlanItem> MaterialPlanItems = JsonSerializer.Deserialize<List<MaterialPlanItem>>(responseString);

                    if (MaterialPlanItems.Count > 0 && MaterialPlanItems[0].authtoken.NullString() != "")
                        CommonConstants.AUTHTOKEN = MaterialPlanItems[0].authtoken;

                    return MaterialPlanItems;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// PRODUCTION PLAN
        /// </summary>
        /// <param name="masterID"></param>
        /// <param name="planID"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<ProductionPlanItem>> GetProductionPlanItems(string masterID, string planID, string site, string fromDate, string toDate,string unit)
        {
            try
            {
                if (CommonConstants.AUTHTOKEN.NullString() == "")
                {
                    return null;
                }

                var query = new Dictionary<string, string>()
                {
                    ["filterMasterId"] = masterID,
                    ["filterPlanId"] = planID,
                    ["filterSiteId"] = site,
                    ["filterFromDate"] = fromDate,
                    ["filterToDate"] = toDate,
                    ["filterUnit"] = unit,
                    ["filterDisplay"] = "WEEK",
                    ["startPage"] = "1",
                    ["endPage"] = "1000000000",
                    ["divisionCd"] = "WISOL",
                    ["siteCd"] = "KR",
                    ["product"] = "SCM",
                    ["locale"] = "KR",
                    ["authtoken"] = CommonConstants.AUTHTOKEN.NullString()
                };

                string url = "http://10.20.10.238:3622/ngs/request/com.thirautech.ngs.scm.scp.pr.result.dailyplan.get_ProductionPlan.dummy";
                var uri = QueryHelpers.AddQueryString(url, query);

                var request = new HttpRequestMessage(HttpMethod.Get, uri);

                string jscontent = "";
                request.Content = new JsonContent(jscontent, Encoding.UTF8);

                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    List<ProductionPlanItem> ProductionPlanItems = JsonSerializer.Deserialize<List<ProductionPlanItem>>(responseString);

                    if (ProductionPlanItems.Count > 0 && ProductionPlanItems[0].authtoken.NullString() != "")
                        CommonConstants.AUTHTOKEN = ProductionPlanItems[0].authtoken;

                    return ProductionPlanItems;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// DEMAND FULFILLMENT
        /// </summary>
        /// <param name="masterID"></param>
        /// <param name="planID"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<DemandFulFillMentItem>> GetDemandFulFillMentItems(string masterID, string planID, string site, string fromDate, string toDate)
        {
            try
            {
                if (CommonConstants.AUTHTOKEN.NullString() == "")
                {
                    return null;
                }

                var query = new Dictionary<string, string>()
                {
                    ["filterMasterId"] = masterID,
                    ["filterPlanId"] = planID,
                    ["filterSiteId"] = site,
                    ["filterGbn"] = "All",
                    ["startPage"] = "1",
                    ["endPage"] = "1000000000",
                    ["divisionCd"] = "WISOL",
                    ["siteCd"] = "KR",
                    ["product"] = "SCM",
                    ["locale"] = "KR",
                    ["filterToDate"] = toDate,
                    ["filterFromDate"] = fromDate,
                    ["filterDisplay"] = "WEEK",
                    ["authtoken"] = CommonConstants.AUTHTOKEN.NullString()
                };

                string url = "http://10.20.10.238:3622/ngs/request/com.thirautech.ngs.scm.scp.pr.analysis.demandfulfillment.get_DemandFulfillment.dummy";
                var uri = QueryHelpers.AddQueryString(url, query);

                var request = new HttpRequestMessage(HttpMethod.Get, uri);

                string jscontent = "";
                request.Content = new JsonContent(jscontent, Encoding.UTF8);

                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    List<DemandFulFillMentItem> DemandFulFillMentItems = JsonSerializer.Deserialize<List<DemandFulFillMentItem>>(responseString);

                    if (DemandFulFillMentItems.Count > 0 && DemandFulFillMentItems[0].authtoken.NullString() != "")
                        CommonConstants.AUTHTOKEN = DemandFulFillMentItems[0].authtoken;

                    return DemandFulFillMentItems;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        ///  Get tồn thành phẩm
        /// </summary>
        /// <param name="masterID"></param>
        /// <param name="planID"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public async Task<List<SCPInventory>> GetSCPInventorys(string masterID, string planID, string site)
        {
            try
            {
                if (CommonConstants.AUTHTOKEN.NullString() == "")
                {
                    return null;
                }

                var query = new Dictionary<string, string>()
                {
                    ["startPage"] = "1",
                    ["siteCd"] = "KR",
                    ["product"] = "SCM",
                    ["locale"] = "KR",
                    ["filterMasterId"] = masterID,
                    ["filterPlanId"] = planID,
                    ["filterSiteId"] = site,
                    ["endPage"] = "1000000000",
                    ["divisionCd"] = "WISOL",
                    ["authtoken"] = CommonConstants.AUTHTOKEN.NullString()
                };

                string url = "http://10.20.10.238:3622/ngs/request/com.thirautech.ngs.scm.scp.od.operationresult.inventory.get_inventorylist.dummy";
                var uri = QueryHelpers.AddQueryString(url, query);

                var request = new HttpRequestMessage(HttpMethod.Get, uri);

                string jscontent = "";
                request.Content = new JsonContent(jscontent, Encoding.UTF8);

                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    List<SCPInventory> SCPInventorys = JsonSerializer.Deserialize<List<SCPInventory>>(responseString);

                    if (SCPInventorys.Count > 0 && SCPInventorys[0].authtoken.NullString() != "")
                        CommonConstants.AUTHTOKEN = SCPInventorys[0].authtoken;

                    return SCPInventorys;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        ///  Get tồn bán thành phẩm
        /// </summary>
        /// <param name="masterID"></param>
        /// <param name="planID"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public async Task<List<SCP_WIP>> GetSCP_WIPs(string masterID, string planID, string site)
        {
            try
            {
                if (CommonConstants.AUTHTOKEN.NullString() == "")
                {
                    return null;
                }

                var query = new Dictionary<string, string>()
                {
                    ["startPage"] = "1",
                    ["siteCd"] = "KR",
                    ["product"] = "SCM",
                    ["locale"] = "KR",
                    ["filterMasterId"] = masterID,
                    ["filterPlanId"] = planID,
                    ["filterSiteId"] = site,
                    ["endPage"] = "1000000000",
                    ["divisionCd"] = "WISOL",
                    ["authtoken"] = CommonConstants.AUTHTOKEN.NullString()
                };

                string url = "http://10.20.10.238:3622/ngs/request/com.thirautech.ngs.scm.scp.od.operationresult.wip.get_wiplist.dummy";
                var uri = QueryHelpers.AddQueryString(url, query);

                var request = new HttpRequestMessage(HttpMethod.Get, uri);

                string jscontent = "";
                request.Content = new JsonContent(jscontent, Encoding.UTF8);

                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    List<SCP_WIP> SCP_WIPs = JsonSerializer.Deserialize<List<SCP_WIP>>(responseString);

                    if (SCP_WIPs.Count > 0 && SCP_WIPs[0].authtoken.NullString() != "")
                        CommonConstants.AUTHTOKEN = SCP_WIPs[0].authtoken;

                    return SCP_WIPs;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// SALES APPROVAL MANUFACTURE
        /// </summary>
        /// <param name="masterID"></param>
        /// <param name="planID"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<SalesApprovalItem>> GetSalesApprovals(string masterID, string planID, string site)
        {
            try
            {
                if (CommonConstants.AUTHTOKEN.NullString() == "")
                {
                    return null;
                }

                var query = new Dictionary<string, string>()
                {
                    ["filterMasterId"] = masterID,
                    ["filterPlanId"] = planID,
                    ["filterSiteId"] = site,
                    ["startPage"] = "1",
                    ["endPage"] = "1000000000",
                    ["divisionCd"] = "WISOL",
                    ["siteCd"] = "KR",
                    ["product"] = "SCM",
                    ["locale"] = "KR",
                    ["authtoken"] = CommonConstants.AUTHTOKEN.NullString()
                };

                string url = "http://10.20.10.238:3622/ngs/request/com.thirautech.ngs.scm.scp.md.salesinfo.salesapprovalmanufacture.get_salesapprovalmanufacture.dummy";
                var uri = QueryHelpers.AddQueryString(url, query);

                var request = new HttpRequestMessage(HttpMethod.Get, uri);

                string jscontent = "";
                request.Content = new JsonContent(jscontent, Encoding.UTF8);

                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    List<SalesApprovalItem> SalesApprovalItems = JsonSerializer.Deserialize<List<SalesApprovalItem>>(responseString);

                    if (SalesApprovalItems.Count > 0)
                        CommonConstants.AUTHTOKEN = SalesApprovalItems[0].authtoken;

                    return SalesApprovalItems;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Get site Id
        /// </summary>
        /// <param name="authTockent"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<MasterIdModel>> GetSiteIdCode()
        {
            try
            {
                if (CommonConstants.AUTHTOKEN.NullString() == "")
                {
                    return null;
                }

                string url = "http://10.20.10.238:3622/ngs/request/com.thirautech.ngs.scm.scp.common.common.get_SiteIdCode.dummy?&divisionCd=WISOL&siteCd=KR&userId=&authId=&product=SCM&locale=KR&errCode=&errMsg=&authtoken=" + CommonConstants.AUTHTOKEN.NullString();

                var request = new HttpRequestMessage(HttpMethod.Get, url);

                string jscontent = "";
                request.Content = new JsonContent(jscontent, Encoding.UTF8);

                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    List<MasterIdModel> masterIdModel = JsonSerializer.Deserialize<List<MasterIdModel>>(responseString);

                    if (masterIdModel.Count > 0)
                        CommonConstants.AUTHTOKEN = masterIdModel[0].authtoken;

                    return masterIdModel;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Get unit code
        /// </summary>
        /// <returns></returns>
        public async Task<List<UnitCodeItem>> GetUnitCode()
        {
            try
            {
                if (CommonConstants.AUTHTOKEN.NullString() == "")
                {
                    return null;
                }

                var query = new Dictionary<string, string>()
                {
                    ["siteCd"] = "KR",
                    ["product"] = "SCM",
                    ["locale"] = "KR",
                    ["divisionCd"] = "WISOL",
                    ["authtoken"] = CommonConstants.AUTHTOKEN.NullString()
                };

                string url = "http://10.20.10.238:3622/ngs/request/com.thirautech.ngs.scm.scp.common.common.get_UnitCode.dummy";
                var uri = QueryHelpers.AddQueryString(url, query);

                var request = new HttpRequestMessage(HttpMethod.Get, uri);

                string jscontent = "";
                request.Content = new JsonContent(jscontent, Encoding.UTF8);

                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    List<UnitCodeItem> unitCodes = JsonSerializer.Deserialize<List<UnitCodeItem>>(responseString);

                    if (unitCodes.Count > 0)
                        CommonConstants.AUTHTOKEN = unitCodes[0].authtoken;

                    return unitCodes;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Thông tin authtoken sau khi login
        /// </summary>
        /// <returns></returns>
        public async Task<SCPLoginModel> SCP_LoginData()
        {
            LoginParam param = new LoginParam();

            param.userId1 = _configuration["SCPLogin:userId1"].ToString();
            param.password1 = _configuration["SCPLogin:password1"].ToString();
            param.locale1 = _configuration["SCPLogin:locale1"].ToString();
            param.SAVEID = _configuration["SCPLogin:SAVEID"].ToString();
            param.divisionCd1 = _configuration["SCPLogin:divisionCd1"].ToString();
            param.product = _configuration["SCPLogin:product"].ToString();

            var loginJson = JsonSerializer.Serialize<LoginParam>(param);

            var request = new HttpRequestMessage(HttpMethod.Post, "http://10.20.10.238:3622/so/auth/com.thirautech.ngs.login.get_login.dummy");

            request.Content = new JsonContent(loginJson, Encoding.UTF8);

            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                List<SCPLoginModel> loginData = JsonSerializer.Deserialize<List<SCPLoginModel>>(responseString);

                if (loginData.Count > 0)
                {
                    if (loginData[0].authtoken.NullString() != "")
                        CommonConstants.AUTHTOKEN = loginData[0].authtoken;
                }
                return loginData.FirstOrDefault();
            }

            return null;
        }
    }
}
