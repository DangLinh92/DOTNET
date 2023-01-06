using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPERATION_MNS.Application.Implementation
{
    public class YieldOfModelService : BaseService, IYieldOfModelService
    {
        private IRespository<YIELD_OF_MODEL, int> _YieldRepository;
        private IRespository<GOC_STANDAR_QTY, int> _GocStandQtyResponsitory;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public YieldOfModelService(IHttpContextAccessor httpContextAccessor, IRespository<YIELD_OF_MODEL, int> YieldRepository, IRespository<GOC_STANDAR_QTY, int> GocStandQtyResponsitory, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _YieldRepository = YieldRepository;
            _GocStandQtyResponsitory = GocStandQtyResponsitory;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void Delete(int Id)
        {
            _YieldRepository.Remove(Id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<YieldOfModelViewModel> GetAllYeildOfModel()
        {
            return _mapper.Map<List<YieldOfModelViewModel>>(_YieldRepository.FindAll());
        }

        public YieldOfModelViewModel UpdateYeildOfModel(YieldOfModelViewModel model)
        {
            var en = _YieldRepository.FindById(model.Id);

            en.Model = model.Model;
            en.Month = model.Month;
            en.YieldActual = model.YieldActual;
            en.YieldPlan = model.YieldPlan;
            en.YieldGap = model.YieldActual - model.YieldPlan;

            _YieldRepository.Update(en);
            return model;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public YieldOfModelViewModel AddYeildOfModel(YieldOfModelViewModel model)
        {
            var en = _mapper.Map<YIELD_OF_MODEL>(model);
            en.YieldGap = en.YieldActual - en.YieldPlan;
            en.UserCreated = GetUserId();
            _YieldRepository.Add(en);
            return model;
        }

        public List<string> GetAllModel()
        {
            var lst = _GocStandQtyResponsitory.FindAll().ToList().Select(x => x.Model).ToList();
            return lst;
        }

        public YieldOfModelViewModel GetYeildOfModelById(int id)
        {
           return _mapper.Map<YieldOfModelViewModel>(_YieldRepository.FindById(id));
        }
    }
}
