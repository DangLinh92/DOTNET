using HRMNS.Application.Interfaces;
using HRMNS.Data.EF;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMNS.Data.Entities.Payroll;
using HRMNS.Utilities.Dtos;
using HRMS.Infrastructure.Interfaces;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class ConNhoMnsService : BaseService, IConNhoMnsService
    {
        private IRespository<HR_CON_NHO, int> _conNhoResponsitory;
        private IRespository<HR_NHANVIEN, string> _nhanvienResponsitory;
        private IUnitOfWork _unitOfWork;
        public ConNhoMnsService(IRespository<HR_CON_NHO, int> conNhoResponsitory, IRespository<HR_NHANVIEN, string> nhanvienResponsitory, IUnitOfWork unitOfWork)
        {
            _nhanvienResponsitory = nhanvienResponsitory;
            _conNhoResponsitory = conNhoResponsitory;
            _unitOfWork = unitOfWork;
        }
        public void DeleteConNho(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<HR_CON_NHO> GetConNhos()
        {
            List<HR_CON_NHO> lstConnho = _conNhoResponsitory.FindAll().ToList();
            return lstConnho;
        }

        public HR_CON_NHO PostConNho(HR_CON_NHO en)
        {
            _conNhoResponsitory.Add(en);
            Save();
            return en;
        }

        public HR_CON_NHO PutConNho(HR_CON_NHO en)
        {
            _conNhoResponsitory.Update(en);
            Save();
            return en;
        }

        public HR_CON_NHO GetConNhoById(int id)
        {
            return _conNhoResponsitory.FindById(id);
        }

        public ResultDB ImportConNhoExcel(string filePath, string param)
        {
            ResultDB resultDB = new ResultDB();

            try
            {
                using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ExcelWorksheet Sheet = packet.Workbook.Worksheets[0];
                    List<HR_CON_NHO> lstConNho = new List<HR_CON_NHO>();

                    string maNV = "";
                    string TenNV = "";
                    string NgaySinhCon = "";
                    string ThangTinhHuong = "";
                    HR_CON_NHO connho = null;
                    for (int i = Sheet.Dimension.Start.Row + 1; i <= Sheet.Dimension.End.Row; i++)
                    {
                        if (Sheet.Cells[i, 1].Text.NullString() == "")
                        {
                            break;
                        }

                        maNV = Sheet.Cells[i, 1].Text.NullString();
                        TenNV = Sheet.Cells[i, 2].Text.NullString();

                        if (DateTime.TryParse(Sheet.Cells[i, 3].Text.NullString(), out DateTime date))
                        {
                            NgaySinhCon = date.ToString("yyyy-MM-dd");
                        }

                        if (DateTime.TryParse(Sheet.Cells[i, 4].Text.NullString(), out DateTime _date))
                        {
                            ThangTinhHuong = _date.ToString("yyyy-MM-dd");
                        }

                        connho = new HR_CON_NHO()
                        {
                            MaNV = maNV,
                            TenNV = TenNV,
                            NgaySinh = NgaySinhCon,
                            ThangTinhHuong = ThangTinhHuong
                        };
                        lstConNho.Add(connho);
                    }

                    if (lstConNho.Count > 0)
                        _conNhoResponsitory.AddRange(lstConNho);
                }
            }
            catch (Exception ex)
            {
                resultDB.ReturnInt = -1;
                resultDB.ReturnString = ex.Message;
                return resultDB;
            }

            resultDB.ReturnInt = 0;
            return resultDB;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
