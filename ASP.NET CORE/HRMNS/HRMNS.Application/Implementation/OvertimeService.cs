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
                var lst = _overtimeRepository.FindAll(includeProperties).OrderByDescending(x => x.DateModified);
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

        public ResultDB ImportExcel(string filePath, string role)
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
                    table.Columns.Add("ApproveLV2");
                    table.Columns.Add("ApproveLV3");
                    table.Columns.Add("HeSoOT");
                    table.Columns.Add("NoiDung");
                    table.Columns.Add("SoGioOT");

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

                        row["Approve"] = CommonConstants.Approved;
                        row["ApproveLV2"] = CommonConstants.Approved;
                        row["ApproveLV3"] = CommonConstants.Request;
                        if (role == CommonConstants.AppRole.AdminRole || role == CommonConstants.roleApprove3)
                        {
                            row["ApproveLV3"] = CommonConstants.Approved;
                        }

                        row["HeSoOT"] = worksheet.Cells[i, 4].Text.NullString();
                        row["NoiDung"] = worksheet.Cells[i, 6].Text.NullString();
                        row["SoGioOT"] = float.Parse(worksheet.Cells[i, 5].Text.NullString());

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
            DateTime dOT;
            try
            {
                dOT = DateTime.Parse(NgayOT);
            }
            catch (Exception)
            {

                dOT = DateTime.Now;
            }

            string DM_NgayLViec = "";
            var lstNgayLeNam = _mapper.Map<List<NgayLeNamViewModel>>(_ngaylenamRepository.FindAll(x => x.Id.Contains(dOT.Year.ToString())).OrderBy(x => x.Id));
            var itemcheck = lstNgayLeNam.FirstOrDefault(x => x.Id == NgayOT);
            var afterOneDay = DateTime.Parse(NgayOT).AddDays(1).ToString("yyyy-MM-dd");
            var itemcheck2 = lstNgayLeNam.FirstOrDefault(x => x.Id == afterOneDay);

            if (itemcheck != null)
            {
                DM_NgayLViec = CommonConstants.NgayLe;

                if (itemcheck.IslastHoliday == CommonConstants.Y)
                {
                    DM_NgayLViec = CommonConstants.NgayLeCuoiCung;
                }
            }
            else if (itemcheck2 != null)
            {
                DM_NgayLViec = CommonConstants.TruocNgayLe;
            }
            else if (DateTime.Parse(NgayOT).DayOfWeek == DayOfWeek.Sunday)
            {
                DM_NgayLViec = CommonConstants.ChuNhat;
            }
            else
            {
                DM_NgayLViec = CommonConstants.NgayThuong;
            }
            return DM_NgayLViec;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public List<DangKyOTNhanVienViewModel> Search(string role, string dept, string status, string timeFrom, string timeTo, params Expression<Func<DANGKY_OT_NHANVIEN, object>>[] includeProperties)
        {
            List<DangKyOTNhanVienViewModel> lstOT = new List<DangKyOTNhanVienViewModel>();
            if (!string.IsNullOrEmpty(timeFrom) && !string.IsNullOrEmpty(timeTo))
            {
                lstOT = _mapper.Map<List<DangKyOTNhanVienViewModel>>(_overtimeRepository.FindAll(x => string.Compare(x.NgayOT, timeFrom) >= 0 && string.Compare(x.NgayOT, timeTo) <= 0, includeProperties).OrderByDescending(x => x.DateModified));
            }
            else
            {
                lstOT = _mapper.Map<List<DangKyOTNhanVienViewModel>>(_overtimeRepository.FindAll(includeProperties).OrderByDescending(x => x.DateModified));
            }

            if (!string.IsNullOrEmpty(dept))
            {
                lstOT = lstOT.Where(x => x.HR_NHANVIEN.MaBoPhan == dept).ToList();
            }

            if (string.IsNullOrEmpty(status))
            {
                if (role == CommonConstants.AssLeader_Role)
                {
                    lstOT = lstOT.Where(x => x.ApproveLV3 != CommonConstants.Approved).ToList();
                }
                else if (role == CommonConstants.roleApprove1) // leader approve
                {
                    lstOT = lstOT.Where(x => x.ApproveLV3 != CommonConstants.Approved && (x.Approve == CommonConstants.Approved || x.Approve == CommonConstants.No_Approved || x.Approve == CommonConstants.Request)).ToList();
                }
                else if (role == CommonConstants.roleApprove2) // korea mnger
                {
                    lstOT = lstOT.Where(x => x.Approve == CommonConstants.Approved && x.ApproveLV3 != CommonConstants.Approved).ToList();
                }
                else if (role == CommonConstants.roleApprove3 || role == CommonConstants.AppRole.AdminRole)
                {
                    lstOT = lstOT.Where(x => x.ApproveLV2 == CommonConstants.Approved).ToList();
                }
            }
            else
            {
                if (role == CommonConstants.AssLeader_Role)
                {
                    lstOT = lstOT.Where(x => x.Approve == status && x.ApproveLV3 != CommonConstants.Approved).ToList();
                }
                else if (role == CommonConstants.roleApprove1) // leader approve
                {
                    lstOT = lstOT.Where(x => x.Approve == status && x.ApproveLV3 != CommonConstants.Approved).ToList();
                }
                else if (role == CommonConstants.roleApprove2) // korea mnger
                {
                    lstOT = lstOT.Where(x => x.Approve == CommonConstants.Approved && x.ApproveLV2 == status && x.ApproveLV3 != CommonConstants.Approved).ToList();
                }
                else if (role == CommonConstants.roleApprove3 || role == CommonConstants.AppRole.AdminRole)
                {
                    lstOT = lstOT.Where(x => x.ApproveLV2 == CommonConstants.Approved && x.ApproveLV3 == status).ToList();
                }
            }

            return lstOT;
        }

        public void Update(DangKyOTNhanVienViewModel overtimeVm)
        {
            overtimeVm.UserModified = GetUserId();

            var entity = _mapper.Map<DANGKY_OT_NHANVIEN>(overtimeVm);
            _overtimeRepository.Update(entity);
        }

        public DangKyOTNhanVienViewModel CheckExist(int id, string maNV, string date,string hso)
        {
            if (id > 0)
            {
                return GetById(id);
            }
            else
            {
                var obj = _overtimeRepository.FindSingle(x => x.MaNV == maNV && x.NgayOT == date && x.HeSoOT == hso);
                return _mapper.Map<DangKyOTNhanVienViewModel>(obj);
            }
        }

        public void UpdateRange(List<DangKyOTNhanVienViewModel> OTVms)
        {
            var lstEntity = _mapper.Map<List<DANGKY_OT_NHANVIEN>>(OTVms);
            foreach (var item in lstEntity)
            {
                item.UserModified = GetUserId();
            }
            _overtimeRepository.UpdateRange(lstEntity);
        }
    }
}
