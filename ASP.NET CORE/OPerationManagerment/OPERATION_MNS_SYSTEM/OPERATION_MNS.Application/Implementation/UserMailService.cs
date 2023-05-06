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
        private IRespository<CTQ_EMAIL_RECEIV_WLP2, string> _UserMailWlp2Repository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserMailService(IRespository<CTQ_EMAIL_RECEIV, string> UserMailRepository,
            IRespository<CTQ_EMAIL_RECEIV_WLP2, string> UserMailWlp2Repository,
                              IUnitOfWork unitOfWork, IMapper mapper,
                              IHttpContextAccessor httpContextAccessor)
        {
            _UserMailWlp2Repository = UserMailWlp2Repository;
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
            return _mapper.Map<List<CTQEmailReceivViewModel>>(_UserMailRepository.FindAll());
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

        #region WLP2
        public List<CTQEmailReceivViewModel> GetListMailWlp2()
        {
            return _mapper.Map<List<CTQEmailReceivViewModel>>(_UserMailWlp2Repository.FindAll());
        }

        public void PutMailWlp2(CTQEmailReceivViewModel email)
        {
            var en = _mapper.Map<CTQ_EMAIL_RECEIV_WLP2>(email);
            _UserMailWlp2Repository.Update(en);
            _unitOfWork.Commit();
        }

        public void PostMailWlp2(CTQEmailReceivViewModel email)
        {
            var en = _mapper.Map<CTQ_EMAIL_RECEIV_WLP2>(email);
            _UserMailWlp2Repository.Add(en);
            _unitOfWork.Commit();
        }

        public void DeleteMailWlp2(string email)
        {
            _UserMailWlp2Repository.Remove(email);
            _unitOfWork.Commit();
        }

        public CTQEmailReceivViewModel GetMailWlp2(string email)
        {
            return _mapper.Map<CTQEmailReceivViewModel>(_UserMailWlp2Repository.FindById(email));
        }

        #endregion
    }
}
