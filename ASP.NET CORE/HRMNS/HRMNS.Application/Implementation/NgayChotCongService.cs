using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using HRMNS.Data.Entities;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class NgayChotCongService : BaseService, INgayChotCongService
    {
        private IUnitOfWork _unitOfWork;
        IRespository<HR_NGAY_CHOT_CONG, int> _repository;
        private readonly IMapper _mapper;

        public NgayChotCongService(IUnitOfWork unitOfWork, IRespository<HR_NGAY_CHOT_CONG, int> repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public HR_NgayChotCongViewModel FindItem(string chotCongChoThang)
        {
            return _mapper.Map<HR_NgayChotCongViewModel>(_repository.FindSingle(x => x.ChotCongChoThang == chotCongChoThang));
        }

        public HR_NgayChotCongViewModel Update(HR_NgayChotCongViewModel model)
        {
            var en = _repository.FindSingle(x => x.ChotCongChoThang == model.ChotCongChoThang);
            if (en == null)
            {
                en = _mapper.Map<HR_NGAY_CHOT_CONG>(model);
                en.UserCreated = GetUserId();
                _repository.Add(en);
            }
            else
            {
                en.NgayChotCong = model.NgayChotCong;
                en.UserModified = GetUserId();
                _repository.Update(en);
            }
            _unitOfWork.Commit();
            return model;
        }

        public HR_NgayChotCongViewModel FinLastItem()
        {
            return _mapper.Map<HR_NgayChotCongViewModel>(_repository.FindAll().ToList().OrderByDescending(x => x.ChotCongChoThang).FirstOrDefault());
        }
    }
}
