using Microsoft.AspNetCore.Http;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels.Lfem;
using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Infrastructure.Interfaces;
using OPERATION_MNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace OPERATION_MNS.Application.Implementation
{
    public class LotTestHistoryLFemService : BaseService, ILotTestHistoryLFemService
    {
        private IUnitOfWork _unitOfWork;
        private IRespository<LOT_TEST_HISTOTY_LFEM, int> _lotTestRepository;

        public LotTestHistoryLFemService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IRespository<LOT_TEST_HISTOTY_LFEM, int> lotTestRepository)
        {
            _unitOfWork = unitOfWork;
            _lotTestRepository = lotTestRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void DeleteData(int id)
        {
            _lotTestRepository.Remove(id);
            Save();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public LOT_TEST_HISTOTY_LFEM FindById(int id)
        {
            return _lotTestRepository.FindById(id);
        }

        public List<LOT_TEST_HISTOTY_LFEM> GetAllData()
        {

            return _lotTestRepository.FindAll().ToList();
        }

        public LOT_TEST_HISTOTY_LFEM PostData(LOT_TEST_HISTOTY_LFEM en)
        {
            _lotTestRepository.Add(en);
            Save();
            return en;
        }

        public LOT_TEST_HISTOTY_LFEM PutData(LOT_TEST_HISTOTY_LFEM en)
        {
            _lotTestRepository.Update(en);
            Save();
            return en;
        }

        /// <summary>
        /// 6-2 : VIEW WIP LOT LIST : Tồn hiện tại LFEM
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<WipLotListLFEMViewModel> GetWIPLotListLfem()
        {
            List<WipLotListLFEMViewModel> lstReuslt = new List<WipLotListLFEMViewModel>();
            ResultDB resultDB = _lotTestRepository.ExecProceduce2("GET_VIEW_WIP_LOT_LIST_LFEM", new Dictionary<string, string>());

            if (resultDB.ReturnInt == 0)
            {
                DataTable data = resultDB.ReturnDataSet.Tables[0];

                if (data.Rows.Count > 0)
                {
                    WipLotListLFEMViewModel wipLotList;
                    foreach (DataRow item in data.Rows)
                    {
                        if (lstReuslt.Any(x => x.Model == item["Material"].NullString()))
                        {
                            wipLotList = lstReuslt.FirstOrDefault(x => x.Model == item["Material"].NullString());
                        }
                        else
                        {
                            wipLotList = new WipLotListLFEMViewModel()
                            {
                                Model = item["Material"].NullString(),
                                Size = item["Size"].NullString()
                            };
                        }

                        if (item["LOT_STATUS"].NullString() == "RUN")
                        {
                            wipLotList.DAM_RUN += double.Parse(item["DAM"].IfNullIsZero());
                            wipLotList.MOLD_RUN += double.Parse(item["Mold"].IfNullIsZero());
                            wipLotList.GRINDING_RUN += double.Parse(item["Grind"].IfNullIsZero());
                            wipLotList.MARKING_RUN += double.Parse(item["Marking"].IfNullIsZero());
                            wipLotList.DICING_RUN += double.Parse(item["Dicing"].IfNullIsZero());
                            wipLotList.TEST_RUN += double.Parse(item["Test"].IfNullIsZero());
                            wipLotList.VI_RUN += double.Parse(item["VI"].IfNullIsZero());
                            wipLotList.OQC_RUN += double.Parse(item["OQC"].IfNullIsZero());
                        }

                        if (item["LOT_STATUS"].NullString() == "WAIT")
                        {
                            wipLotList.DAM_WAIT += double.Parse(item["DAM"].IfNullIsZero());
                            wipLotList.MOLD_WAIT += double.Parse(item["Mold"].IfNullIsZero());
                            wipLotList.GRINDING_WAIT += double.Parse(item["Grind"].IfNullIsZero());
                            wipLotList.MARKING_WAIT += double.Parse(item["Marking"].IfNullIsZero());
                            wipLotList.DICING_WAIT += double.Parse(item["Dicing"].IfNullIsZero());
                            wipLotList.TEST_WAIT += double.Parse(item["Test"].IfNullIsZero());
                            wipLotList.VI_WAIT += double.Parse(item["VI"].IfNullIsZero());
                            wipLotList.OQC_WAIT += double.Parse(item["OQC"].IfNullIsZero());
                        }

                        if (item["LOT_STATUS"].NullString() == "HOLD")
                        {
                            wipLotList.DAM_HOLD += double.Parse(item["DAM"].IfNullIsZero());
                            wipLotList.MOLD_HOLD += double.Parse(item["Mold"].IfNullIsZero());
                            wipLotList.GRINDING_HOLD += double.Parse(item["Grind"].IfNullIsZero());
                            wipLotList.MARKING_HOLD += double.Parse(item["Marking"].IfNullIsZero());
                            wipLotList.DICING_HOLD += double.Parse(item["Dicing"].IfNullIsZero());
                            wipLotList.TEST_HOLD += double.Parse(item["Test"].IfNullIsZero());
                            wipLotList.VI_HOLD += double.Parse(item["VI"].IfNullIsZero());
                            wipLotList.OQC_HOLD += double.Parse(item["OQC"].IfNullIsZero());
                            wipLotList.DRY_HOLD += double.Parse(item["Dry"].IfNullIsZero());
                        }
                        wipLotList.LastUpdate = DateTime.Now.ToString("HH:mm:ss");
                        if (!lstReuslt.Any(x => x.Model == item["Material"].NullString()))
                        {
                            lstReuslt.Add(wipLotList);
                        }
                    }
                }
            }

            return lstReuslt;
        }
    }
}
