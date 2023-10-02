using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OPERATION_MNS.Application.Interfaces
{
    public interface IGOCModuleService : IDisposable
    {
        Task<ResultDB> ImportGocSales(string filePath, string param);
        List<GOC_PRODUCTION_PLAN_LFEM_UPDATE> GetSalesByPlanId(string planId,string masterId,string siteId);
        List<GOC_PRODUCTION_PLAN_LFEM_UPDATE> GetSalesPlanByMonth(string month);

        void Save();
    }
}
