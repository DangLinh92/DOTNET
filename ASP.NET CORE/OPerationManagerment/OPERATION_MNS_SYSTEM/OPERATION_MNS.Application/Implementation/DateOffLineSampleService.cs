using AutoMapper;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Infrastructure.Interfaces;
using OPERATION_MNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPERATION_MNS.Application.Implementation
{
    public class DateOffLineSampleService : BaseService, IDateOffLineSampleService
    {
        private IRespository<DATE_OFF_LINE_SAMPLE, int> _DateOffLineResponsitory;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DateOffLineSampleService(IRespository<DATE_OFF_LINE_SAMPLE, int> dateOffLineResponsitory, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _DateOffLineResponsitory = dateOffLineResponsitory;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public DATE_OFF_LINE_SAMPLE Add(DATE_OFF_LINE_SAMPLE model)
        {
            DATE_OFF_LINE_SAMPLE newEn = _DateOffLineResponsitory.FindSingle(x => x.ItemValue == model.ItemValue);
            if (newEn != null)
            {
                newEn.CopyPropertiesFrom(model, new List<string>() { "Id", "UserCreated", "DateCreated" });
                _DateOffLineResponsitory.Update(newEn);
            }
            else
            {
                _DateOffLineResponsitory.Add(model);
            }

            return model;
        }

        public void Delete(int Id)
        {
            _DateOffLineResponsitory.Remove(Id);
            Save();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<DATE_OFF_LINE_SAMPLE> GetAllData()
        {
            List<DATE_OFF_LINE_SAMPLE> rs = _DateOffLineResponsitory.FindAll().OrderByDescending(x=>x.ItemValue).ToList();
            return rs;
        }

        public DATE_OFF_LINE_SAMPLE GetById(int id)
        {
            return _DateOffLineResponsitory.FindById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public DATE_OFF_LINE_SAMPLE Update(DATE_OFF_LINE_SAMPLE model)
        {
            _DateOffLineResponsitory.Update(model);
            return model;
        }
    }
}
