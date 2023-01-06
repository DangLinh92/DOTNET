using AutoMapper;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.Implementation
{
    public class MaterialToSapCodeService : BaseService, IMaterialToSapCodeService
    {
        private IRespository<MATERIAL_TO_SAP, int> _MaterialToSapResponsitory;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MaterialToSapCodeService(IRespository<MATERIAL_TO_SAP, int> MaterialToSapResponsitory, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _MaterialToSapResponsitory = MaterialToSapResponsitory;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public MaterialToSapViewModel Add(MaterialToSapViewModel model)
        {
            var en = _mapper.Map<MATERIAL_TO_SAP>(model);
            _MaterialToSapResponsitory.Add(en);
            return model;
        }

        public void Delete(int Id)
        {
            _MaterialToSapResponsitory.Remove(Id);
            Save();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<MaterialToSapViewModel> GetAllData()
        {
            return _mapper.Map<List<MaterialToSapViewModel>>(_MaterialToSapResponsitory.FindAll());
        }

        public MaterialToSapViewModel GetById(int id)
        {
            return _mapper.Map<MaterialToSapViewModel>(_MaterialToSapResponsitory.FindById(id));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public MaterialToSapViewModel Update(MaterialToSapViewModel model)
        {
            var en = _mapper.Map<MATERIAL_TO_SAP>(model);
            _MaterialToSapResponsitory.Update(en);
            return model;
        }
    }
}
