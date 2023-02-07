using HRMNS.Application.ViewModels.EHS;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IEhsKeHoachDaoTaoATVSLDService
    {
        List<EhsKeHoachDaoTaoATLDViewModel> GetList(string year);
        EhsKeHoachDaoTaoATLDViewModel Update(EhsKeHoachDaoTaoATLDViewModel model);
        EhsKeHoachDaoTaoATLDViewModel Add(EhsKeHoachDaoTaoATLDViewModel model);
        void Delete(Guid Id);

        EhsKeHoachDaoTaoATLDViewModel GetById(Guid Id);

        EhsThoiGianThucHienDaoTaoATVSViewModel UpdateThoiGianATVSLD(EhsThoiGianThucHienDaoTaoATVSViewModel model);
        EhsThoiGianThucHienDaoTaoATVSViewModel AddThoiGianATVSLD(EhsThoiGianThucHienDaoTaoATVSViewModel model);
        void DeleteThoiGianATVSLD(int Id);
        EhsThoiGianThucHienDaoTaoATVSViewModel GetThoiGianATVSLDById(int Id);

        void ImportExcel(string filePath);

        void Save();
    }
}
