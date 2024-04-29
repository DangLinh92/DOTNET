using HRMNS.Data.Entities;
using HRMNS.Data.Entities.Payroll;
using HRMNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IPhuCapLuongService: IDisposable
    {
        public List<PHUCAP_DOC_HAI> GetAll();
        public PHUCAP_DOC_HAI AddDH(PHUCAP_DOC_HAI en);
        public PHUCAP_DOC_HAI UpdateDH(PHUCAP_DOC_HAI en);
        public void DeleteDH(int Id);
        public PHUCAP_DOC_HAI GetAllById(int Id);
        public List<BOPHAN> GetBoPhanAll();

        public List<HR_SALARY_GRADE> GetAllGrade(int year);
        public HR_SALARY_GRADE AddGrade(HR_SALARY_GRADE en);
        public HR_SALARY_GRADE UpdateGrade(HR_SALARY_GRADE en);
        public HR_SALARY_GRADE GetGradeById(string Id);
        public void DeleteGrade(string Id);
        public ResultDB ImportExcel(string filePath);
    }
}
