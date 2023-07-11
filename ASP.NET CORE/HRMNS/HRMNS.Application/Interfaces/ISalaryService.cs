using HRMNS.Data.Entities;
using HRMNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface ISalaryService : IDisposable
    {
        List<HR_SALARY> GetAllSalary();
        HR_SALARY UpdateSalary(HR_SALARY salary);
        void UpdateRangeSalary(List<HR_SALARY> salary);
        HR_SALARY AddSalary(HR_SALARY salary);
        void DeleteSalary(int id);
        HR_SALARY GetByMaNV(string manv);
        HR_SALARY GetById(int id);

        void Save();

        ResultDB ImportExcel(string path);
        ResultDB ImportTaiKhoanNHExcel(string path);

        // Thông tin chi phí phát sinh
        List<HR_SALARY_PHATSINH> GetAllSalaryPhatSinh(int year);
        HR_SALARY_PHATSINH UpdateSalaryPhatSinh(HR_SALARY_PHATSINH salary);
        HR_SALARY_PHATSINH AddSalaryPhatSinh(HR_SALARY_PHATSINH salary);
        void DeleteSalaryPhatSinh(int id);
        HR_SALARY_PHATSINH GetPhatSinhById(int id);
        List<HR_SALARY_DANHMUC_PHATSINH> GetDanhMucPhatSinh();
        ResultDB ImportPhatSinhExcel(string path);
        HR_SALARY_PHATSINH GetPhatSinhByMaNV(string manv);
    }
}
