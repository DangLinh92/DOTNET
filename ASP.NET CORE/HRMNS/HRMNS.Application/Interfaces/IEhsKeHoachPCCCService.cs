using HRMNS.Application.ViewModels.EHS;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IEhsKeHoachPCCCService
    {
        List<Ehs_KeHoach_PCCCViewModel> GetList(string year);
        Ehs_KeHoach_PCCCViewModel Update(Ehs_KeHoach_PCCCViewModel model);
        Ehs_KeHoach_PCCCViewModel Add(Ehs_KeHoach_PCCCViewModel model);
        void Delete(Guid Id);

        Ehs_KeHoach_PCCCViewModel GetById(Guid Id);

        EhsThoiGianThucHienPCCCViewModel UpdateThoiGianPCCC(EhsThoiGianThucHienPCCCViewModel model);
        EhsThoiGianThucHienPCCCViewModel AddThoiGianPCCC(EhsThoiGianThucHienPCCCViewModel model);
        void DeleteThoiGianPCCC(int Id);
        EhsThoiGianThucHienPCCCViewModel GetThoiGianPCCCById(int Id);

        string ImportExcel(string filePath);

        void Save();
    }
}
