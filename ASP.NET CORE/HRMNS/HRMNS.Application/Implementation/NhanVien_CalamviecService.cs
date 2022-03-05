using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMNS.Data.Enums;
using HRMNS.Utilities.Dtos;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class NhanVien_CalamviecService : BaseService, INhanVien_CalamviecService
    {
        private IRespository<NHANVIEN_CALAMVIEC, int> _nhanvienClviecRepository;
        private IRespository<DM_CA_LVIEC, string> _dmCalamviecResponsitory;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NhanVien_CalamviecService(IRespository<NHANVIEN_CALAMVIEC, int> respository, IRespository<DM_CA_LVIEC, string> dmCalamviecRespository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _nhanvienClviecRepository = respository;
            _dmCalamviecResponsitory = dmCalamviecRespository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public NhanVien_CalamViecViewModel Add(NhanVien_CalamViecViewModel nhanVienLVVm)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<NhanVien_CalamViecViewModel> GetAll()
        {
            var lst = _nhanvienClviecRepository.FindAll(x => x.Status == Status.Active.ToString() || x.Status == Status.New.ToString());
            return _mapper.Map<List<NhanVien_CalamViecViewModel>>(lst);
        }

        public List<NhanVien_CalamViecViewModel> GetAll(string keyword, params Expression<Func<NHANVIEN_CALAMVIEC, object>>[] includeProperties)
        {
            var lst = _nhanvienClviecRepository.FindAll(x => x.Status == Status.Active.ToString() || x.Status == Status.New.ToString(),includeProperties);
            return _mapper.Map<List<NhanVien_CalamViecViewModel>>(lst);
        }

        public NhanVien_CalamViecViewModel GetById(string id, params Expression<Func<NHANVIEN_CALAMVIEC, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public List<DMCalamviecViewModel> GetDMCalamViec()
        {
            return _mapper.Map<List<DMCalamviecViewModel>>(_dmCalamviecResponsitory.FindAll());
        }

        public ResultDB ImportExcel(string filePath, string param)
        {
            ResultDB resultDB = new ResultDB();
            try
            {
                var lstDmCalamViec = _mapper.Map<List<DMCalamviecViewModel>>(_dmCalamviecResponsitory.FindAll());

                using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = packet.Workbook.Worksheets[1];

                    DataTable table = new DataTable();
                    table.Columns.Add("MaNV");
                    table.Columns.Add("Danhmuc_CaLviec");
                    table.Columns.Add("BatDau_TheoCa");
                    table.Columns.Add("KetThuc_TheoCa");
                    table.Columns.Add("Status");

                    DataRow row = null;
                    string dmCalviec = "";
                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                    {
                        row = table.NewRow();

                        if (string.IsNullOrEmpty(worksheet.Cells[i, 1].Text.NullString().ToUpper()))
                        {
                            continue;
                        }

                        row["MaNV"] = worksheet.Cells[i, 1].Text.NullString().ToUpper();

                        dmCalviec = worksheet.Cells[i, 2].Text.NullString();
                        if (lstDmCalamViec.Any(x => dmCalviec.Contains(x.TenCaLamViec)))
                        {
                            row["Danhmuc_CaLviec"] = lstDmCalamViec.FirstOrDefault(x => dmCalviec.Contains(x.TenCaLamViec))?.Id.NullString();
                        }
                        else
                        {
                            throw new Exception("Not found danh muc ca lam viec");
                        }

                        row["BatDau_TheoCa"] = worksheet.Cells[i, 3].Text.NullString();
                        row["KetThuc_TheoCa"] = worksheet.Cells[i, 4].Text.NullString();

                        if (string.Compare(DateTime.Now.ToString("yyyy-MM-dd"), worksheet.Cells[i, 3].Text.NullString()) >= 0 && string.Compare(DateTime.Now.ToString("yyyy-MM-dd"), worksheet.Cells[i, 4].Text.NullString()) <= 0)
                        {
                            row["Status"] = Status.Active;
                        }
                        else if (string.Compare(DateTime.Now.ToString("yyyy-MM-dd"), worksheet.Cells[i, 3].Text.NullString()) < 0)
                        {
                            row["Status"] = Status.New;
                        }
                        else
                        {
                            row["Status"] = Status.InActive;
                        }
                        table.Rows.Add(row);
                    }

                    resultDB = _nhanvienClviecRepository.ExecProceduce("PKG_BUSINESS.PUT_NHANVIEN_CALAMVIEC", new Dictionary<string, string>(), "A_DATA", table);
                }
                return resultDB;
            }
            catch (Exception ex)
            {
                resultDB.ReturnInt = -1;
                resultDB.ReturnString = ex.Message;
                return resultDB;
            }
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public List<NhanVien_CalamViecViewModel> Search(string id, string name, string dept)
        {
            throw new NotImplementedException();
        }

        public void Update(NhanVien_CalamViecViewModel nhanVienLVVm)
        {
            throw new NotImplementedException();
        }

        public void UpdateSingle(NhanVien_CalamViecViewModel nhanVienLVVm)
        {
            throw new NotImplementedException();
        }
    }
}
