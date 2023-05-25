using HRMNS.Data.Entities;
using HRMNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface ICapBacNhanVienService
    {
        List<NHANVIEN_INFOR_EX> GetAll(int year);
        NHANVIEN_INFOR_EX Update(NHANVIEN_INFOR_EX salary);
        NHANVIEN_INFOR_EX Add(NHANVIEN_INFOR_EX salary);
        void Delete(int id);
        NHANVIEN_INFOR_EX GetCapBacById(int id);
        ResultDB ImportCapBacExcel(string path);
        NHANVIEN_INFOR_EX GetCapBacByMaNV(string manv,int year);
    }
}
