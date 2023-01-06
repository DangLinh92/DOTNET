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
    public class UserMailService : BaseService, IUserMailService
    {
        private IRespository<CTQ_EMAIL_RECEIV, string> _UserMailRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserMailService(IRespository<CTQ_EMAIL_RECEIV, string> UserMailRepository,
                              IUnitOfWork unitOfWork, IMapper mapper,
                              IHttpContextAccessor httpContextAccessor)
        {
            _UserMailRepository = UserMailRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<CTQEmailReceivViewModel> GetListMail()
        {
            string dept = GetDepartment();
            if(dept == "")
            {
                return _mapper.Map<List<CTQEmailReceivViewModel>>(_UserMailRepository.FindAll());
            }
            else
            {
                return _mapper.Map<List<CTQEmailReceivViewModel>>(_UserMailRepository.FindAll(x => x.Department == dept));
            }
        }

        // update
        public void PutMail(CTQEmailReceivViewModel email)
        {
            var en = _mapper.Map<CTQ_EMAIL_RECEIV>(email);
            _UserMailRepository.Update(en);
            _unitOfWork.Commit();
        }

        // add
        public void PostMail(CTQEmailReceivViewModel email)
        {
            var en = _mapper.Map<CTQ_EMAIL_RECEIV>(email);
            _UserMailRepository.Add(en);
            _unitOfWork.Commit();
        }

        public void DeleteMail(string email)
        {
            _UserMailRepository.Remove(email);
            _unitOfWork.Commit();
        }

        public CTQEmailReceivViewModel GetMail(string email)
        {
           return _mapper.Map<CTQEmailReceivViewModel>(_UserMailRepository.FindById(email));
        }
    }
}
