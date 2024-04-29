using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.EF;
using HRMNS.Data.Entities;
using HRMNS.Data.Entities.Payroll;
using HRMNS.Utilities.Dtos;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace HRMNS.Application.Implementation
{
    public class PhuCapLuongService : BaseService, IPhuCapLuongService
    {
        private IRespository<PHUCAP_DOC_HAI, int> _phucapDocHaiRepository;
        private IRespository<HR_SALARY_GRADE, string> _gradeRepository;
        private IRespository<BOPHAN, string> _bophanRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PhuCapLuongService(IRespository<PHUCAP_DOC_HAI, int> phucapDocHaiRepository, IRespository<BOPHAN, string> bophanRepository,
            IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IRespository<HR_SALARY_GRADE, string> gradeRepository)
        {
            _phucapDocHaiRepository = phucapDocHaiRepository;
            _bophanRepository = bophanRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _gradeRepository = gradeRepository;
        }

        public PHUCAP_DOC_HAI AddDH(PHUCAP_DOC_HAI en)
        {
            en.UserCreated = GetUserId();
            _phucapDocHaiRepository.Add(en);
            _unitOfWork.Commit();
            return en;
        }

        public void DeleteDH(int id)
        {
            _phucapDocHaiRepository.Remove(id);
            _unitOfWork.Commit();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<PHUCAP_DOC_HAI> GetAll()
        {
            return _phucapDocHaiRepository.FindAll().ToList();
        }

        public PHUCAP_DOC_HAI GetAllById(int Id)
        {
            return _phucapDocHaiRepository.FindById(Id);
        }

        public PHUCAP_DOC_HAI UpdateDH(PHUCAP_DOC_HAI en)
        {
            en.UserCreated = GetUserId();
            _phucapDocHaiRepository.Update(en);
            _unitOfWork.Commit();
            return en;
        }

        public List<BOPHAN> GetBoPhanAll()
        {
            return _bophanRepository.FindAll().ToList();
        }

        public List<HR_SALARY_GRADE> GetAllGrade(int year)
        {
            return _gradeRepository.FindAll(x => x.Year == year).ToList();
        }

        public HR_SALARY_GRADE AddGrade(HR_SALARY_GRADE en)
        {
            en.UserCreated = GetUserId();
            en.Id = en.CapBac + "_" + en.Year.ToString();
            _gradeRepository.Add(en);
            _unitOfWork.Commit();
            return en;
        }

        public HR_SALARY_GRADE UpdateGrade(HR_SALARY_GRADE en)
        {
            en.UserCreated = GetUserId();
            _gradeRepository.Update(en);
            _unitOfWork.Commit();
            return en;
        }

        public void DeleteGrade(string Id)
        {
            _gradeRepository.Remove(Id);
            _unitOfWork.Commit();
        }

        public HR_SALARY_GRADE GetGradeById(string Id)
        {
            return _gradeRepository.FindById(Id);
        }

        public ResultDB ImportExcel(string filePath)
        {
            using var transaction = ((EFUnitOfWork)_unitOfWork).DBContext().Database.BeginTransaction();
            ResultDB resultDB = new ResultDB();
            try
            {
                using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = packet.Workbook.Worksheets[0];
                    HR_SALARY_GRADE salary;
                    List<HR_SALARY_GRADE> lstUpdate = new List<HR_SALARY_GRADE>();
                    List<HR_SALARY_GRADE> lstAdd = new List<HR_SALARY_GRADE>();
                    string capbac;
                    int year;
                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                    {
                        capbac = worksheet.Cells[i, 1].Text.NullString();
                        year = int.Parse(worksheet.Cells[i, 8].Text.NullString());
                        if (capbac.NullString() == "")
                        {
                            break;
                        }

                        salary = _gradeRepository.FindSingle(x => x.CapBac == capbac && x.Year == year);

                        if (salary == null)
                        {
                            salary = new HR_SALARY_GRADE();

                            salary.Id = capbac + "_" + year;
                            salary.CapBac = capbac;
                            salary.Year = year;
                            salary.BasicSalaryStandard = double.Parse(worksheet.Cells[i, 2].Text.IfNullIsZero());
                            salary.IncentiveLanguage = double.Parse(worksheet.Cells[i, 3].Text.IfNullIsZero());
                            salary.BasicSalary = double.Parse(worksheet.Cells[i, 4].Text.IfNullIsZero());
                            salary.LivingAllowance = double.Parse(worksheet.Cells[i, 5].Text.IfNullIsZero());
                            salary.IncentiveStandard = double.Parse(worksheet.Cells[i, 6].Text.IfNullIsZero());
                            salary.AttendanceAllowance = double.Parse(worksheet.Cells[i, 7].Text.IfNullIsZero());

                            lstAdd.Add(salary);
                        }
                        else
                        {
                            salary.CapBac = capbac;
                            salary.Year = year;
                            salary.BasicSalaryStandard = double.Parse(worksheet.Cells[i, 2].Text.IfNullIsZero());
                            salary.IncentiveLanguage = double.Parse(worksheet.Cells[i, 3].Text.IfNullIsZero());
                            salary.BasicSalary = double.Parse(worksheet.Cells[i, 4].Text.IfNullIsZero());
                            salary.LivingAllowance = double.Parse(worksheet.Cells[i, 5].Text.IfNullIsZero());
                            salary.IncentiveStandard = double.Parse(worksheet.Cells[i, 6].Text.IfNullIsZero());
                            salary.AttendanceAllowance = double.Parse(worksheet.Cells[i, 7].Text.IfNullIsZero());

                            lstUpdate.Add(salary);
                        }
                    }

                    if (lstUpdate.Count > 0)
                    {
                        var trackedEntitie = ((EFUnitOfWork)_unitOfWork).DBContext().Set<HR_SALARY_GRADE>().Local.ToList();
                        foreach (var entity in trackedEntitie)
                        {
                            foreach (var item in lstUpdate.ToList())
                            {
                                if (entity.Id == item.Id)
                                {
                                    ((EFUnitOfWork)_unitOfWork).DBContext().Entry(entity).State = EntityState.Detached;
                                    break;
                                }
                            }
                            // Cập nhật thuộc tính của entity nếu cần thiết
                        }
                    }

                    _gradeRepository.AddRange(lstAdd);
                    _gradeRepository.UpdateRange(lstUpdate);
                    _unitOfWork.Commit();

                    transaction.Commit();
                    resultDB.ReturnInt = 0;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                resultDB.ReturnInt = -1;
                resultDB.ReturnString = ex.Message;
            }
            return resultDB;
        }
    }
}
