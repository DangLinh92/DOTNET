using HRMNS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
   public interface IKyLuatKhenThuongService : IDisposable
    {
        List<HR_KY_LUAT_KHENTHUONG> GetAll();
        HR_KY_LUAT_KHENTHUONG Add(HR_KY_LUAT_KHENTHUONG nhanVienLVVm);
        HR_KY_LUAT_KHENTHUONG FindById(int id);

        void Update(HR_KY_LUAT_KHENTHUONG nhanVienLVVm);

        void Delete(int id);
    }
}
