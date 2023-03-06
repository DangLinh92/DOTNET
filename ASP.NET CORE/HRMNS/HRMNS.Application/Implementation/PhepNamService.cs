using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class PhepNamService : BaseService, IPhepNamService
    {
        private IRespository<HR_PHEP_NAM, int> _phepNamRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PhepNamService(IRespository<HR_PHEP_NAM, int> phepNamRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _phepNamRepository = phepNamRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public PhepNamViewModel Add(PhepNamViewModel phepNamVm)
        {
            var phepNam = _mapper.Map<PhepNamViewModel, HR_PHEP_NAM>(phepNamVm);
            phepNam.DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            phepNam.DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            phepNam.UserCreated = GetUserId();
            _phepNamRepository.Add(phepNam);
            Save();
            return _mapper.Map<PhepNamViewModel>(phepNam);
        }

        public void Delete(int id)
        {
            _phepNamRepository.Remove(id);
        }

        public List<PhepNamViewModel> GetAll(string keyword)
        {
            return _mapper.Map<List<PhepNamViewModel>>(_phepNamRepository.FindAll());
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public PhepNamViewModel Update(PhepNamViewModel phepNamVm)
        {
            var phepNam = _mapper.Map<PhepNamViewModel, HR_PHEP_NAM>(phepNamVm);
            phepNam.UserModified = GetUserId();
            _phepNamRepository.Update(phepNam);
            Save();
            return _mapper.Map<PhepNamViewModel>(phepNam);
        }

        public PhepNamViewModel UpdateSingle(PhepNamViewModel phepNamVm)
        {
            throw new NotImplementedException();
        }

        public PhepNamViewModel GetById(int id)
        {
            return _mapper.Map<HR_PHEP_NAM, PhepNamViewModel>(_phepNamRepository.FindById(id));
        }

        public List<PhepNamViewModel> GetList(string year)
        {
            int _year = int.Parse(year);
            var lst = _phepNamRepository.FindAll(x => x.Year == _year, x => x.HR_NHANVIEN).ToList();
            return _mapper.Map<List<PhepNamViewModel>>(lst);
        }

        public void ImportExcel(string filePath)
        {
            using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = packet.Workbook.Worksheets[1];
                HR_PHEP_NAM phepNam; ;

                for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                {
                    phepNam = new HR_PHEP_NAM();
                    phepNam.MaNhanVien = worksheet.Cells[i, 1].Text.NullString();
                    phepNam.SoPhepNam = float.Parse(worksheet.Cells[i, 3].Text.NullString());
                    phepNam.SoPhepConLai = float.Parse(worksheet.Cells[i, 4].Text.NullString());
                    phepNam.Year = int.Parse(worksheet.Cells[i, 5].Text.NullString());
                    phepNam.SoTienChiTra = decimal.Parse(worksheet.Cells[i, 6].Text.NullString());
                    if(DateTime.TryParse(worksheet.Cells[i, 7].Text.NullString(),out _))
                    {
                        phepNam.ThoiGianChiTra = DateTime.Parse(worksheet.Cells[i, 7].Text.NullString());
                    }
                    else
                    {
                        phepNam.ThoiGianChiTra = null;
                    }
                    _phepNamRepository.Add(phepNam);
                }
            }
        }
    }
}
