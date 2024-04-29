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

        public List<PhepNamViewModel> GetList(string month)
        {
            int _year = int.Parse(month.Substring(0, 4));
            var lst = _phepNamRepository.FindAll(x => x.Year == _year, x => x.HR_NHANVIEN, z => z.HR_NHANVIEN.HR_BO_PHAN_DETAIL).ToList();
            List<HR_PHEP_NAM> phepnam = new List<HR_PHEP_NAM>();
            foreach (var item in lst.ToList())
            {
                if (item.HR_NHANVIEN.NgayNghiViec.NullString() == "" || item.HR_NHANVIEN.NgayNghiViec.NullString().Substring(0, 7).CompareTo(month.Substring(0, 7)) >= 0 || (item.ThoiGianChiTra.NullString() != "" && item.ThoiGianChiTra.NullString().Substring(0, 7) == month.Substring(0, 7)))
                {
                    phepnam.Add(item);
                }
            }

            return _mapper.Map<List<PhepNamViewModel>>(phepnam);
        }

        public void ImportExcel(string filePath)
        {
            using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = packet.Workbook.Worksheets[0];
                HR_PHEP_NAM phepNam; ;
                string maNV = "";
                int year;
                for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                {
                    maNV = worksheet.Cells[i, 1].Text.NullString();

                    if (maNV == "")
                        break;

                    year = int.Parse(worksheet.Cells[i, 3].Text.NullString());

                    phepNam = _phepNamRepository.FindSingle(x => x.MaNhanVien == maNV && year == x.Year);

                    if (phepNam == null)
                    {
                        phepNam = new HR_PHEP_NAM();
                        phepNam.MaNhanVien = worksheet.Cells[i, 1].Text.NullString();
                        phepNam.Year = year;

                        if (DateTime.TryParse(worksheet.Cells[i, 4].Text.NullString(), out _))
                        {
                            phepNam.ThangBatDauDocHai = DateTime.Parse(worksheet.Cells[i, 4].Text.NullString());
                        }

                        if (DateTime.TryParse(worksheet.Cells[i, 5].Text.NullString(), out _))
                        {
                            phepNam.ThangKetThucDocHai = DateTime.Parse(worksheet.Cells[i, 5].Text.NullString());
                        }
                        _phepNamRepository.Add(phepNam);
                    }
                    else
                    {
                        if (DateTime.TryParse(worksheet.Cells[i, 4].Text.NullString(), out _))
                        {
                            phepNam.ThangBatDauDocHai = DateTime.Parse(worksheet.Cells[i, 4].Text.NullString());
                        }

                        if (DateTime.TryParse(worksheet.Cells[i, 5].Text.NullString(), out _))
                        {
                            phepNam.ThangKetThucDocHai = DateTime.Parse(worksheet.Cells[i, 5].Text.NullString());
                        }

                        _phepNamRepository.Update(phepNam);
                    }
                }
            }
        }

        public PhepNamViewModel GetByCodeAndYear(string maNV, int year)
        {
            return _mapper.Map<PhepNamViewModel>(_phepNamRepository.FindSingle(x => x.MaNhanVien == maNV && x.Year == year));
        }

        public void UpdateRange(List<PhepNamViewModel> phepNamVms)
        {
            _phepNamRepository.UpdateRange(_mapper.Map<List<HR_PHEP_NAM>>(phepNamVms));
        }

        public void AddRange(List<PhepNamViewModel> phepNamVms)
        {
            _phepNamRepository.AddRange(_mapper.Map<List<HR_PHEP_NAM>>(phepNamVms));
        }
    }
}
