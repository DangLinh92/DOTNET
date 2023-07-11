using HRMNS.Application.Interfaces;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMNS.Data.Enums;
using HRMNS.Utilities.Dtos;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class CapBacNhanVienService : BaseService, ICapBacNhanVienService
    {
        private IRespository<NHANVIEN_INFOR_EX, int> _capbacRepository;
        private IUnitOfWork _unitOfWork;

        public CapBacNhanVienService(IRespository<NHANVIEN_INFOR_EX, int> capbacRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _capbacRepository = capbacRepository;
            _unitOfWork = unitOfWork;
        }

        public NHANVIEN_INFOR_EX Add(NHANVIEN_INFOR_EX salary)
        {
            salary.UserCreated = GetUserId();
            _capbacRepository.Add(salary);
            _unitOfWork.Commit();
            salary = GetCapBacByMaNV(salary.MaNV,salary.Year);
            return salary;
        }

        public void Delete(int id)
        {
            _capbacRepository.Remove(id);
            _unitOfWork.Commit();
        }

        public List<NHANVIEN_INFOR_EX> GetAll(int year)
        {
            return _capbacRepository.FindAll(x=>x.Year == year,x => x.HR_NHANVIEN, z => z.HR_NHANVIEN.HR_BO_PHAN_DETAIL).ToList();
        }

        public NHANVIEN_INFOR_EX GetCapBacById(int id)
        {
            return _capbacRepository.FindById(id);
        }

        public NHANVIEN_INFOR_EX GetCapBacByMaNV(string manv,int year)
        {
            return _capbacRepository.FindSingle(x => x.MaNV == manv && x.Year ==year, x => x.HR_NHANVIEN, z => z.HR_NHANVIEN.HR_BO_PHAN_DETAIL);
        }

        public ResultDB ImportCapBacExcel(string filePath)
        {
            ResultDB resultDB = new ResultDB();
            try
            {
                using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = packet.Workbook.Worksheets[0];
                    NHANVIEN_INFOR_EX nhanvienEx;
                    List<NHANVIEN_INFOR_EX> lstAdd = new List<NHANVIEN_INFOR_EX>();
                    string manv;

                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                    {
                        manv = worksheet.Cells[i, 1].Text.NullString();

                        if (manv.NullString() == "")
                        {
                            break;
                        }

                        nhanvienEx = new NHANVIEN_INFOR_EX()
                        {
                            MaNV = manv
                        };

                        nhanvienEx.MaBoPhanEx = worksheet.Cells[i, 3].Text.NullString();
                        nhanvienEx.Grade = worksheet.Cells[i, 4].Text.NullString();
                        nhanvienEx.Year = int.Parse(worksheet.Cells[i, 5].Text.NullString());

                        lstAdd.Add(nhanvienEx);
                    }

                    if (lstAdd.Count > 0)
                        _capbacRepository.AddRange(lstAdd);

                    resultDB.ReturnInt = 0;
                    _unitOfWork.Commit();
                }
            }
            catch (Exception ex)
            {
                resultDB.ReturnInt = -1;
                resultDB.ReturnString = ex.Message;
            }
            return resultDB;
        }

        public NHANVIEN_INFOR_EX Update(NHANVIEN_INFOR_EX salary)
        {
            salary.UserModified = GetUserId();
            _capbacRepository.Update(salary);
            _unitOfWork.Commit();
            return salary;
        }
    }
}
