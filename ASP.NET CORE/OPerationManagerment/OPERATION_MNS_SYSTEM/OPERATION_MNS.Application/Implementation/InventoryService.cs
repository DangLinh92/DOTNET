using AutoMapper;
using Microsoft.AspNetCore.Http;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Infrastructure.Interfaces;
using OPERATION_MNS.Utilities.Common;
using OPERATION_MNS.Utilities.Constants;
using OPERATION_MNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.Implementation
{
    public class InventoryService : BaseService,IInventoryService
    {
        private IRespository<INVENTORY_ACTUAL, int> _InventoryActualRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public InventoryService(IRespository<INVENTORY_ACTUAL, int> InventoryActualRepository,
                              IUnitOfWork unitOfWork, IMapper mapper,
                              IHttpContextAccessor httpContextAccessor)
        {
            _InventoryActualRepository = InventoryActualRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Hàng normal, loại A/B, đơn vị chip
        /// Get current inventory
        /// </summary>
        /// <param name="unit">CHIP WAFER</param>
        /// <returns></returns>
        public List<InventoryActualViewModel> GetCurrentInventory(string unit)
        {
            List<InventoryActualViewModel> result = new List<InventoryActualViewModel>();
            if (unit == CommonConstants.CHIP)
            {
                ResultDB resultDB = _InventoryActualRepository.ExecProceduce2("PKG_BUSINESS@GET_INVENTORY_CURRENT_CHIP", new Dictionary<string, string>());

                if(resultDB.ReturnInt == 0)
                {
                    result = DataTableToJson.ConvertDataTable<InventoryActualViewModel>(resultDB.ReturnDataSet.Tables[0]);
                }
            }
            else
            {
                ResultDB resultDB = _InventoryActualRepository.ExecProceduce2("PKG_BUSINESS@GET_INVENTORY_CURRENT_WAFER", new Dictionary<string, string>());

                if (resultDB.ReturnInt == 0)
                {
                    result = DataTableToJson.ConvertDataTable<InventoryActualViewModel>(resultDB.ReturnDataSet.Tables[0]);
                }
            }

            return result;
        }
    }
}
