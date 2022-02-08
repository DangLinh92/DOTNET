﻿using HRMNS.Application.ViewModels.HR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IPhepNamService
    {
        PhepNamViewModel Add(PhepNamViewModel phepNamVm);
        void Update(PhepNamViewModel phepNamVm);
        void UpdateSingle(PhepNamViewModel phepNamVm);
        void Delete(int id);
        List<PhepNamViewModel> GetAll(string keyword);
        PhepNamViewModel GetById(int id);
        void Save();
    }
}