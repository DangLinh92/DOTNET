using HRMNS.Application.ViewModels.HR;
using HRMNS.Application.ViewModels.System;
using HRMNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface ITrainingListService : IDisposable
    {
        Hr_TrainingViewModel AddTraining(Hr_TrainingViewModel trainigModel);

        void UpdateTraining(Hr_TrainingViewModel typeModel);

        void Delete(Guid id);

        List<Hr_TrainingViewModel> GetAll();

        Hr_TrainingViewModel GetById(Guid id);

        List<Training_NhanVienViewModel> GetNhanVienTraining(Guid id);

        ResultDB ImportNhanVienDaoTao(string filePath,Guid trainingId);

        void Save();
    }
}
