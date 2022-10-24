using HRMNS.Application.ViewModels.HR;
using HRMNS.Application.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface ITrainingTypeService : IDisposable
    {
        TrainingTypeViewModel Add(TrainingTypeViewModel typeModel);

        void Update(TrainingTypeViewModel typeModel);

        void Delete(int id);

        List<TrainingTypeViewModel> GetAll();

        TrainingTypeViewModel GetById(int id);
        void Save();
    }
}
