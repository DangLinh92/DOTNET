using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.EF;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMNS.Data.Enums;
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
    public class OvertimeService : BaseService, IOvertimeService
    {
        private IRespository<DANGKY_OT_NHANVIEN, int> _overtimeRepository;
        private IRespository<DM_NGAY_LAMVIEC, string> _dmNgayLamviecRepository;
        private IRespository<NGAY_LE_NAM, string> _ngaylenamRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OvertimeService(IRespository<DANGKY_OT_NHANVIEN, int> respository, IRespository<DM_NGAY_LAMVIEC, string> dmNgayLamviecRepository, IRespository<NGAY_LE_NAM, string> ngaylenamRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _overtimeRepository = respository;
            _dmNgayLamviecRepository = dmNgayLamviecRepository;
            _ngaylenamRepository = ngaylenamRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public DangKyOTNhanVienViewModel Add(DangKyOTNhanVienViewModel overtimeVm)
        {
            overtimeVm.UserCreated = GetUserId();
            overtimeVm.UserModified = GetUserId();

            var obj = _mapper.Map<DANGKY_OT_NHANVIEN>(overtimeVm);
            _overtimeRepository.Add(obj);
            return overtimeVm;
        }

        public void Delete(int id)
        {
            _overtimeRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<DangKyOTNhanVienViewModel> GetAll(string keyword, params Expression<Func<DANGKY_OT_NHANVIEN, object>>[] includeProperties)
        {
            if (keyword == "")
            {
                var lst = _overtimeRepository.FindAll(x => x.Passed != CommonConstants.Y, includeProperties).OrderByDescending(x => x.DateModified);
                return _mapper.Map<List<DangKyOTNhanVienViewModel>>(lst);
            }
            else
            {
                var lst = _overtimeRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan.Contains(keyword), includeProperties).OrderByDescending(x => x.DateModified);
                return _mapper.Map<List<DangKyOTNhanVienViewModel>>(lst);
            }
        }

        public DangKyOTNhanVienViewModel GetById(int id, params Expression<Func<DANGKY_OT_NHANVIEN, object>>[] includeProperties)
        {
            if (id > 0)
            {
                return _mapper.Map<DangKyOTNhanVienViewModel>(_overtimeRepository.FindById(id, includeProperties));
            }
            else
            {
                return null;
            }
        }

        public ResultDB ImportExcel(string filePath, string param)
        {
            ResultDB resultDB = new ResultDB();
            try
            {
                using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = packet.Workbook.Worksheets[1];

                    DataTable table = new DataTable();
                    table.Columns.Add("NgayOT");
                    table.Columns.Add("MaNV");
                    table.Columns.Add("DM_NgayLViec");
                    table.Columns.Add("Approve");

                    DataRow row = null;
                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                    {
                        row = table.NewRow();

                        if (string.IsNullOrEmpty(worksheet.Cells[i, 1].Text.NullString().ToUpper()))
                        {
                            continue;
                        }

                        row["MaNV"] = worksheet.Cells[i, 1].Text.NullString().ToUpper();
                        row["NgayOT"] = worksheet.Cells[i, 3].Text.NullString();
                        row["DM_NgayLViec"] = UpdateDMNgayLviec(worksheet.Cells[i, 3].Text.NullString());
                        row["Approve"] = CommonConstants.No_Approved;
                        table.Rows.Add(row);
                    }

                    resultDB = _overtimeRepository.ExecProceduce("PKG_BUSINESS.PUT_NHANVIEN_OVERTIME", new Dictionary<string, string>(), "A_DATA", table);
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

        private string UpdateDMNgayLviec(string NgayOT)
        {
            string DM_NgayLViec = "";
            var lstNgayLeNam = _mapper.Map<List<NgayLeNamViewModel>>(_ngaylenamRepository.FindAll(x => x.Id.Contains(DateTime.Now.Year.ToString())).OrderBy(x => x.Id));
            var itemcheck = lstNgayLeNam.FirstOrDefault(x => x.Id == NgayOT);
            var afterOneDay = DateTime.Parse(NgayOT).AddDays(1).ToString("yyyy-MM-dd");
            var itemcheck2 = lstNgayLeNam.FirstOrDefault(x => x.Id == afterOneDay);

            if (itemcheck != null)
            {
                DM_NgayLViec = "NL";

                if (itemcheck.IslastHoliday == CommonConstants.Y)
                {
                    DM_NgayLViec = "NLCC";
                }
            }
            else if (itemcheck2 != null)
            {
                DM_NgayLViec = "TNL";
            }
            else if (DateTime.Parse(NgayOT).DayOfWeek == DayOfWeek.Sunday)
            {
                DM_NgayLViec = "CN";
            }
            else
            {
                DM_NgayLViec = "NT";
            }
            return DM_NgayLViec;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public List<DangKyOTNhanVienViewModel> Search(string dept, string status, string timeFrom, string timeTo, params Expression<Func<DANGKY_OT_NHANVIEN, object>>[] includeProperties)
        {
            if (!string.IsNullOrEmpty(timeFrom) && !string.IsNullOrEmpty(timeTo))
            {
                if (string.IsNullOrEmpty(dept) && string.IsNullOrEmpty(status))
                {
                    var lst = _overtimeRepository.FindAll(x => string.Compare(x.NgayOT, timeFrom) >= 0 && string.Compare(x.NgayOT, timeTo) <= 0, includeProperties).OrderByDescending(x => x.DateModified);
                    return _mapper.Map<List<DangKyOTNhanVienViewModel>>(lst);
                }

                if (!string.IsNullOrEmpty(dept) && string.IsNullOrEmpty(status))
                {
                    var lst = _overtimeRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept && (string.Compare(x.NgayOT, timeFrom) >= 0 && string.Compare(x.NgayOT, timeTo) <= 0), includeProperties).OrderByDescending(x => x.DateModified);
                    return _mapper.Map<List<DangKyOTNhanVienViewModel>>(lst);
                }

                if (string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(status))
                {
                    if (status != CommonConstants.No_Approved && status != CommonConstants.Approved)
                    {
                        var lst = _overtimeRepository.FindAll(x => x.Approve == status && (string.Compare(x.NgayOT, timeFrom) >= 0 && string.Compare(x.NgayOT, timeTo) <= 0), includeProperties).OrderByDescending(x => x.DateModified);
                        return _mapper.Map<List<DangKyOTNhanVienViewModel>>(lst);
                    }
                    else
                    {
                        string[] itemCheck = status == CommonConstants.No_Approved ? new string[] { "", null } : new string[] { status };
                        var lst = _overtimeRepository.FindAll(x => (x.Approve == status || itemCheck.Contains(x.Approve)) && (string.Compare(x.NgayOT, timeFrom) >= 0 && string.Compare(x.NgayOT, timeTo) <= 0), includeProperties).OrderByDescending(x => x.DateModified);
                        return _mapper.Map<List<DangKyOTNhanVienViewModel>>(lst);
                    }
                }

                if (!string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(status))
                {
                    if (status != CommonConstants.No_Approved && status != CommonConstants.Approved)
                    {
                        var lst = _overtimeRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept && x.Approve == status && (string.Compare(x.NgayOT, timeFrom) >= 0 && string.Compare(x.NgayOT, timeTo) <= 0), includeProperties).OrderByDescending(x => x.DateModified);
                        return _mapper.Map<List<DangKyOTNhanVienViewModel>>(lst);
                    }
                    else
                    {
                        string[] itemCheck = status == CommonConstants.No_Approved ? new string[] { "", null } : new string[] { status };
                        var lst = _overtimeRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept && (x.Approve == status || itemCheck.Contains(x.Approve)) && (string.Compare(x.NgayOT, timeFrom) >= 0 && string.Compare(x.NgayOT, timeTo) <= 0), includeProperties).OrderByDescending(x => x.DateModified);
                        return _mapper.Map<List<DangKyOTNhanVienViewModel>>(lst);
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(dept) && string.IsNullOrEmpty(status))
                {
                    return GetAll("", x => x.HR_NHANVIEN, y => y.DM_NGAY_LAMVIEC);
                }

                if (!string.IsNullOrEmpty(dept) && string.IsNullOrEmpty(status))
                {
                    return GetAll(dept, x => x.HR_NHANVIEN, y => y.DM_NGAY_LAMVIEC);
                }

                if (string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(status))
                {
                    if (status != CommonConstants.No_Approved && status != CommonConstants.Approved)
                    {
                        var lst = _overtimeRepository.FindAll(x => x.Approve == status, includeProperties).OrderByDescending(x => x.DateModified);
                        return _mapper.Map<List<DangKyOTNhanVienViewModel>>(lst);
                    }
                    else
                    {
                        string[] itemCheck = status == CommonConstants.No_Approved ? new string[] { "", null } : new string[] { status };
                        var lst = _overtimeRepository.FindAll(x => x.Approve == status || itemCheck.Contains(x.Approve), includeProperties).OrderByDescending(x => x.DateModified);
                        return _mapper.Map<List<DangKyOTNhanVienViewModel>>(lst);
                    }
                }

                if (!string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(status))
                {
                    if (status != CommonConstants.No_Approved && status != CommonConstants.Approved)
                    {
                        var lst = _overtimeRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept && x.Approve == status, includeProperties).OrderByDescending(x => x.DateModified);
                        return _mapper.Map<List<DangKyOTNhanVienViewModel>>(lst);
                    }
                    else
                    {
                        string[] itemCheck = status == CommonConstants.No_Approved ? new string[] { "", null } : new string[] { status };
                        var lst = _overtimeRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept && (x.Approve == status || itemCheck.Contains(x.Approve)), includeProperties).OrderByDescending(x => x.DateModified);
                        return _mapper.Map<List<DangKyOTNhanVienViewModel>>(lst);
                    }
                }
            }

            return new List<DangKyOTNhanVienViewModel>();
        }

        public void Update(DangKyOTNhanVienViewModel overtimeVm)
        {
            overtimeVm.UserModified = GetUserId();

            var entity = _mapper.Map<DANGKY_OT_NHANVIEN>(overtimeVm);
            _overtimeRepository.Update(entity);
        }

        public void UpdateSingle(DangKyOTNhanVienViewModel nhanVienLVVm)
        {
            throw new NotImplementedException();
        }

        public DangKyOTNhanVienViewModel CheckExist(int id, string maNV, string date)
        {
            if (id > 0)
            {
                return GetById(id);
            }
            else
            {
                var obj = _overtimeRepository.FindSingle(x => x.MaNV == maNV && x.NgayOT == date);
                return _mapper.Map<DangKyOTNhanVienViewModel>(obj);
            }
        }

        public void Approve(string dept, string status, bool isApprove)
        {
            var lst = _overtimeRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept && x.Approve == status || x.Approve == null, y => y.HR_NHANVIEN).ToList();

            foreach (var item in lst)
            {
                item.Approve = isApprove ? CommonConstants.Approved : CommonConstants.No_Approved;
                item.HR_NHANVIEN = null;
            }
            _overtimeRepository.UpdateRange(lst);
        }

        public void ApproveSingle(int Id, bool isApprove)
        {
            var obj = _overtimeRepository.FindById(Id);
            obj.Approve = isApprove ? CommonConstants.Approved : CommonConstants.No_Approved;
            _overtimeRepository.Update(obj);
        }

    }
}
