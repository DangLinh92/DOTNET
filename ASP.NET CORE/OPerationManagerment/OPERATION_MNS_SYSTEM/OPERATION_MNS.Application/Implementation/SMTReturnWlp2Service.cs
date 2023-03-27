using AutoMapper;
using Microsoft.AspNetCore.Http;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels.Wlp2;
using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPERATION_MNS.Application.Implementation
{
    public class SMTReturnWlp2Service : BaseService, ISMTReturnWlp2Service
    {
        private IRespository<SMT_RETURN_WLP2, int> _smtReturnResponsitory;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SMTReturnWlp2Service(IRespository<SMT_RETURN_WLP2, int> smtReturnResponsitory, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _smtReturnResponsitory = smtReturnResponsitory;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<SmtReturnWlp2ViewModel> GetSmtReturn()
        {
            return _mapper.Map<List<SmtReturnWlp2ViewModel>>(_smtReturnResponsitory.FindAll());
        }

        public SmtReturnWlp2ViewModel Insert(SmtReturnWlp2ViewModel model)
        {
            model.UserCreated = GetUserId();
           var en = _mapper.Map<SMT_RETURN_WLP2>(model);
            _smtReturnResponsitory.Add(en);
            return _mapper.Map<SmtReturnWlp2ViewModel>(en);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public SmtReturnWlp2ViewModel Update(SmtReturnWlp2ViewModel model)
        {
            var en = _mapper.Map<SMT_RETURN_WLP2>(model);
            _smtReturnResponsitory.Update(en);
            return model;
        }

        public SmtReturnWlp2ViewModel FindSingle(string sapcode)
        {
            var en = _smtReturnResponsitory.FindAll(x => x.SapCode == sapcode).ToList().FirstOrDefault();
            if (en == null) return null;

            return _mapper.Map<SmtReturnWlp2ViewModel>(en);
        }
    }
}
