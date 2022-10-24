using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.System;
using HRMNS.Data.Entities;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class TrainningTypeService : BaseService, ITrainingTypeService
    {
        private IRespository<TRAINING_TYPE, int> _trainingTypeRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainningTypeService(IRespository<TRAINING_TYPE, int> trainingTypeRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _trainingTypeRepository = trainingTypeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public TrainingTypeViewModel Add(TrainingTypeViewModel typeModel)
        {
            var en = _mapper.Map<TRAINING_TYPE>(typeModel);
            en.UserCreated = GetUserId();
            _trainingTypeRepository.Add(en);
            return typeModel;
        }

        public void Delete(int id)
        {
            var en = _trainingTypeRepository.FindById(id);
            _trainingTypeRepository.Remove(en);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<TrainingTypeViewModel> GetAll()
        {
           return _mapper.Map<List<TrainingTypeViewModel>>(_trainingTypeRepository.FindAll());
        }

        public TrainingTypeViewModel GetById(int id)
        {
            var en = _trainingTypeRepository.FindById(id);
            return _mapper.Map<TrainingTypeViewModel>(en);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(TrainingTypeViewModel typeModel)
        {
            var en = _trainingTypeRepository.FindById(typeModel.Id);
            en.TrainName = typeModel.TrainName;
            en.Description = typeModel.Description;
            en.Status = typeModel.Status;
            _trainingTypeRepository.Update(en);
        }
    }
}
