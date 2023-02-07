using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.System;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMNS.Utilities.Common;
using HRMNS.Utilities.Dtos;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class TrainningListService : BaseService, ITrainingListService
    {
        private IRespository<TRAINING_TYPE, int> _trainingTypeRepository;
        private IRespository<HR_TRAINING, Guid> _trainingListRepository;
        private IRespository<TRAINING_NHANVIEN, int> _trainingNhanVienRepository;
        private IRespository<EVENT_SHEDULE_PARENT, Guid> _eventScheduleParentRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainningListService(IRespository<EVENT_SHEDULE_PARENT, Guid> eventScheduleParentRepository,
            IRespository<TRAINING_TYPE, int> trainingTypeRepository,
            IRespository<HR_TRAINING, Guid> trainingListRepository,
            IRespository<TRAINING_NHANVIEN, int> trainingNhanVienRepository,
            IUnitOfWork unitOfWork, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _eventScheduleParentRepository = eventScheduleParentRepository;
            _trainingTypeRepository = trainingTypeRepository;
            _trainingListRepository = trainingListRepository;
            _trainingNhanVienRepository = trainingNhanVienRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Hr_TrainingViewModel AddTraining(Hr_TrainingViewModel trainigModel)
        {
            Guid scheduleId = Guid.NewGuid();
            string trainName = _trainingTypeRepository.FindById(trainigModel.TrainnigType).TrainName;
            _eventScheduleParentRepository.Add(new EVENT_SHEDULE_PARENT()
            {
                Id = scheduleId,
                Subject = trainName,
                StartEvent = trainigModel.FromDate,
                EndEvent = trainigModel.ToDate,
                StartTime = DateTime.Parse(trainigModel.FromDate),
                EndTime = DateTime.Parse(trainigModel.ToDate).AddDays(1),
                Description = trainigModel.Description,
                UserCreated = GetUserId()
            });

            trainigModel.MaEventParent = scheduleId;
            var en = _mapper.Map<HR_TRAINING>(trainigModel);
            en.UserCreated = GetUserId();
            _trainingListRepository.Add(en);
            return trainigModel;
        }

        public void UpdateTraining(Hr_TrainingViewModel trainigModel)
        {
            var oldTraining = _trainingListRepository.FindById(trainigModel.Id);

            oldTraining.CopyPropertiesFrom(trainigModel, new List<string>() { "Id", "DateCreated", "DateModified", "UserCreated", "UserModified", "MaEventParent" });
            oldTraining.UserModified = GetUserId();
            _trainingListRepository.Update(oldTraining);

            var scheduleParent = _eventScheduleParentRepository.FindById(oldTraining.MaEventParent);
            string trainName = _trainingTypeRepository.FindById(trainigModel.TrainnigType).TrainName;
            scheduleParent.Subject = trainName;
            scheduleParent.StartEvent = trainigModel.FromDate;
            scheduleParent.EndEvent = trainigModel.ToDate;
            scheduleParent.StartTime = DateTime.Parse(trainigModel.FromDate);
            scheduleParent.EndTime = DateTime.Parse(trainigModel.ToDate).AddDays(1) ;
            scheduleParent.Description = trainigModel.Description;
            scheduleParent.UserModified = GetUserId();
            _eventScheduleParentRepository.Update(scheduleParent);
        }

        public void Delete(Guid id)
        {
            var en = _trainingListRepository.FindById(id);
            var scheduleParent = _eventScheduleParentRepository.FindById(en.MaEventParent);
            _trainingListRepository.Remove(id);
            _eventScheduleParentRepository.Remove(scheduleParent);
        }

        public List<Hr_TrainingViewModel> GetAll()
        {
            var lst = _mapper.Map<List<Hr_TrainingViewModel>>(_trainingListRepository.FindAll(x => x.TRAINING_TYPE, y => y.TRAINING_NHANVIEN));
            return lst.OrderBy(x=>x.TrainnigType).ThenByDescending(x=>x.ToDate).ToList();
        }

        public Hr_TrainingViewModel GetById(Guid id)
        {
            return _mapper.Map<Hr_TrainingViewModel>(_trainingListRepository.FindById(id, x => x.TRAINING_TYPE));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public ResultDB ImportNhanVienDaoTao(string filePath, Guid trainingId)
        {
            ResultDB resultDB = new ResultDB();
            try
            {
                using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = packet.Workbook.Worksheets[1];

                    TRAINING_NHANVIEN nv = null;
                    List<TRAINING_NHANVIEN> lstNV = new List<TRAINING_NHANVIEN>();

                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                    {
                        nv = new TRAINING_NHANVIEN(worksheet.Cells[i, 1].Text.NullString(), trainingId);
                        nv.UserCreated = GetUserId();
                        lstNV.Add(nv);
                    }

                    var lstNVOld = _trainingNhanVienRepository.FindAll(x => x.TrainnigId.Equals(trainingId)).ToList();

                    _trainingNhanVienRepository.RemoveMultiple(lstNVOld);
                    _trainingNhanVienRepository.AddRange(lstNV);

                    _unitOfWork.Commit();
                }

                resultDB.ReturnInt = 0;

                return resultDB;
            }
            catch (Exception ex)
            {
                resultDB.ReturnInt = -1;
                resultDB.ReturnString = ex.Message;
                return resultDB;
            }
        }

        public List<Training_NhanVienViewModel> GetNhanVienTraining(Guid id)
        {
            var lst = _mapper.Map<List<Training_NhanVienViewModel>>(_trainingNhanVienRepository.FindAll(x => x.TrainnigId.Equals(id), y => y.HR_NHANVIEN, d => d.HR_TRAINING));
            return lst;
        }
    }
}
