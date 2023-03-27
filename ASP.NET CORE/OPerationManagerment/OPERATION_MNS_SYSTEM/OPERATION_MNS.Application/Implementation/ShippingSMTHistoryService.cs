using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels.Wlp2;
using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Infrastructure.Interfaces;
using OPERATION_MNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OPERATION_MNS.Application.Implementation
{
    public class ShippingSMTHistoryService : BaseService, IShippingSMTHistoryService
    {
        private IRespository<GOC_PLAN_WLP2, int> _GocPlanRepository;
        public ShippingSMTHistoryService(IRespository<GOC_PLAN_WLP2, int> GocPlanRepository)
        {
            _GocPlanRepository = GocPlanRepository;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<ShippingSMTHistoryViewModel> GetHistoryByTime(string fromDate, string toDate)
        {
            List<ShippingSMTHistoryViewModel> result = new List<ShippingSMTHistoryViewModel>();

            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("A_FROM_DATE", fromDate);
            param.Add("A_TO_DATE", toDate);
            ResultDB rs = _GocPlanRepository.ExecProceduce2("GET_HISTORY_SHIPPING_SMT_WLP2", param);

            if(rs.ReturnInt == 0)
            {
                ShippingSMTHistoryViewModel item = new ShippingSMTHistoryViewModel();

                if (rs.ReturnDataSet.Tables[0].Rows.Count > 0)
                {
                    ShippingSMTHistoryByLotIdViewModel lot;
                   
                    foreach (DataRow row in rs.ReturnDataSet.Tables[0].Rows)
                    {
                        lot = new ShippingSMTHistoryByLotIdViewModel();
                        lot.LotID = row["Lot ID"].NullString();
                        lot.MoveOutTime = row["Move Out Time"].NullString();
                        lot.SapMaterial = row["SAP Material"].NullString();
                        lot.OutPutQty = float.Parse(row["Output Qty"].IfNullIsZero());
                        item.shippingSMTHistoryByLotIdViewModels.Add(lot);
                    }
                }

                if(rs.ReturnDataSet.Tables[1].Rows.Count > 0)
                {
                    ShippingSMTHistoryBySapCodeViewModel sap;
                    foreach (DataRow row in rs.ReturnDataSet.Tables[1].Rows)
                    {
                        sap = new ShippingSMTHistoryBySapCodeViewModel();
                        sap.SapMaterial = row["SapCode"].NullString();
                        sap.OutPutQty = float.Parse(row["Shipping_Real"].IfNullIsZero());
                        item.shippingSMTHistoryBySapCodeViewModels.Add(sap);
                    }
                }

                result.Add(item);
            }

            return result;
        }
    }
}
