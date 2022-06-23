using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMNS.Utilities.Constants;
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
    public class DangKyChamCongDacBietService : BaseService, IDangKyChamCongDacBietService
    {
        private IRespository<DANGKY_CHAMCONG_DACBIET, int> _chamCongDbRepository;
        private IRespository<DANGKY_CHAMCONG_CHITIET, int> _chamCongChiTietRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DangKyChamCongDacBietService(IRespository<DANGKY_CHAMCONG_DACBIET, int> chamCongDbRepository, IRespository<DANGKY_CHAMCONG_CHITIET, int> respository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _chamCongDbRepository = chamCongDbRepository;
            _chamCongChiTietRepository = respository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public DangKyChamCongDacBietViewModel Add(DangKyChamCongDacBietViewModel chamCongVm)
        {
            chamCongVm.UserCreated = GetUserId();
            var entity = _mapper.Map<DANGKY_CHAMCONG_DACBIET>(chamCongVm);
            _chamCongDbRepository.Add(entity);
            return chamCongVm;
        }

        public void Delete(int id)
        {
            _chamCongDbRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<DangKyChamCongDacBietViewModel> GetAll(string keyword)
        {
            return _mapper.Map<List<DangKyChamCongDacBietViewModel>>(_chamCongDbRepository.FindAll().OrderByDescending(x => x.DateModified));
        }

        public List<DangKyChamCongDacBietViewModel> GetAll(params Expression<Func<DANGKY_CHAMCONG_DACBIET, object>>[] includeProperties)
        {
            return _mapper.Map<List<DangKyChamCongDacBietViewModel>>(_chamCongDbRepository.FindAll(x => x.Id > 0, includeProperties).OrderByDescending(x => x.DateModified));
        }

        public DangKyChamCongDacBietViewModel GetById(int id)
        {
            return _mapper.Map<DangKyChamCongDacBietViewModel>(_chamCongDbRepository.FindById(id));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(DangKyChamCongDacBietViewModel chamCongVm)
        {
            chamCongVm.UserModified = GetUserId();
            var entity = _mapper.Map<DANGKY_CHAMCONG_DACBIET>(chamCongVm);
            _chamCongDbRepository.Update(entity);
        }

        public List<DangKyChamCongDacBietViewModel> Search(string dept, string fromDate, string toDate, params Expression<Func<DANGKY_CHAMCONG_DACBIET, object>>[] includeProperties)
        {
            if (string.IsNullOrEmpty(dept))
            {
                if (string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
                {
                    return GetAll(includeProperties);
                }

                return _mapper.Map<List<DangKyChamCongDacBietViewModel>>(_chamCongDbRepository.FindAll(x => x.DateCreated.CompareTo(fromDate) >= 0 && x.DateCreated.CompareTo(toDate) <= 0, includeProperties).OrderByDescending(x => x.DateModified));
            }
            else
            {
                if (string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
                {
                    return _mapper.Map<List<DangKyChamCongDacBietViewModel>>(_chamCongDbRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept, includeProperties).OrderByDescending(x => x.DateModified));
                }

                return _mapper.Map<List<DangKyChamCongDacBietViewModel>>(_chamCongDbRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept && x.DateCreated.CompareTo(fromDate) >= 0 && x.DateCreated.CompareTo(toDate) <= 0, includeProperties).OrderByDescending(x => x.DateCreated));
            }
        }

        public DangKyChamCongDacBietViewModel GetSingle(Expression<Func<DANGKY_CHAMCONG_DACBIET, bool>> predicate)
        {
            return _mapper.Map<DangKyChamCongDacBietViewModel>(_chamCongDbRepository.FindSingle(predicate));
        }

        public void UpdateRange(List<DangKyChamCongDacBietViewModel> chamCongVms)
        {
            var lstEntity = _mapper.Map<List<DANGKY_CHAMCONG_DACBIET>>(chamCongVms);
            foreach (var item in lstEntity)
            {
                item.UserModified = GetUserId();
            }
            _chamCongDbRepository.UpdateRange(lstEntity);
        }

        public ResultDB ImportExcel(string filePath, string role)
        {
            ResultDB resultDB = new ResultDB();
            try
            {
                using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = packet.Workbook.Worksheets[1];

                    DataTable table = new DataTable();
                    table.Columns.Add("MaNV");
                    table.Columns.Add("MaChamCong_ChiTiet");
                    table.Columns.Add("NgayBatDau");
                    table.Columns.Add("NgayKetThuc");
                    table.Columns.Add("NoiDung");
                    table.Columns.Add("Approve");
                    table.Columns.Add("ApproveLV2");
                    table.Columns.Add("ApproveLV3");

                    DataRow row = null;
                    string kytuChamCong = "";
                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                    {
                        row = table.NewRow();

                        if (string.IsNullOrEmpty(worksheet.Cells[i, 1].Text.NullString().ToUpper()))
                        {
                            continue;
                        }

                        if (!worksheet.Cells[i, 3].Text.NullString().Contains(":"))
                        {
                            throw new Exception("Ký tự châm công không phù hợp: " + worksheet.Cells[i, 3].Text.NullString());
                        }

                        kytuChamCong = worksheet.Cells[i, 3].Text.NullString().Split(":")[0].NullString();

                        row["MaNV"] = worksheet.Cells[i, 1].Text.NullString().ToUpper();
                        row["MaChamCong_ChiTiet"] = _chamCongChiTietRepository.FindSingle(x => x.KyHieuChamCong == kytuChamCong).Id;
                        row["NgayBatDau"] = worksheet.Cells[i, 4].Text.NullString();
                        row["NgayKetThuc"] = worksheet.Cells[i, 5].Text.NullString();
                        row["NoiDung"] = worksheet.Cells[i, 6].Text.NullString();
                        row["Approve"] = CommonConstants.Request;
                        row["ApproveLV2"] = CommonConstants.Request;

                        if (role == CommonConstants.AppRole.AdminRole || role == CommonConstants.roleApprove3)
                        {
                            row["Approve"] = CommonConstants.Approved;
                            row["ApproveLV2"] = CommonConstants.Approved;
                            row["ApproveLV3"] = CommonConstants.Approved;
                        }
                        else
                        {
                            row["ApproveLV3"] = CommonConstants.Request;
                        }

                        table.Rows.Add(row);
                    }

                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add("A_USER", GetUserId());
                    resultDB = _chamCongDbRepository.ExecProceduce("PKG_BUSINESS.PUT_CHAMCONG_DB", dic, "A_DATA", table);
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
    }
}
