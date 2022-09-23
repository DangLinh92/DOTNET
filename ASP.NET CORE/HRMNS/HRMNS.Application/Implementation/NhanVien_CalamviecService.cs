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
using System.Threading;
using System.Threading.Tasks;
using HRMNS.Utilities.Common;

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
            nhanVienLVVm.UserCreated = GetUserId();
            nhanVienLVVm.UserModified = GetUserId();

            if (string.Compare(DateTime.Now.ToString("yyyy-MM-dd"), nhanVienLVVm.BatDau_TheoCa) >= 0 && string.Compare(DateTime.Now.ToString("yyyy-MM-dd"), nhanVienLVVm.KetThuc_TheoCa) <= 0)
            {
                nhanVienLVVm.Status = Status.Active.NullString();
            }
            else if (string.Compare(DateTime.Now.ToString("yyyy-MM-dd"), nhanVienLVVm.BatDau_TheoCa) < 0)
            {
                nhanVienLVVm.Status = Status.New.ToString();
            }
            else
            {
                nhanVienLVVm.Status = Status.InActive.ToString();
            }

            var obj = _mapper.Map<NHANVIEN_CALAMVIEC>(nhanVienLVVm);
            _nhanvienClviecRepository.Add(obj);
            return nhanVienLVVm;
        }

        public void Delete(int id)
        {
            _nhanvienClviecRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<NhanVien_CalamViecViewModel> GetAll()
        {
            var lst = _nhanvienClviecRepository.FindAll(x => x.Status == Status.Active.ToString() || x.Status == Status.New.ToString()).OrderByDescending(x => x.DateModified);
            return _mapper.Map<List<NhanVien_CalamViecViewModel>>(lst);
        }

        public List<NhanVien_CalamViecViewModel> GetAllWithoutStatus()
        {
            var lst = _nhanvienClviecRepository.FindAll().OrderByDescending(x => x.DateModified);
            return _mapper.Map<List<NhanVien_CalamViecViewModel>>(lst);
        }

        public List<NhanVien_CalamViecViewModel> GetAll(string keyword, params Expression<Func<NHANVIEN_CALAMVIEC, object>>[] includeProperties)
        {
            if (keyword == "")
            {
                var lst = _nhanvienClviecRepository.FindAll(x => x.Status == Status.Active.ToString() || x.Status == Status.New.ToString(), includeProperties).OrderByDescending(x => x.DateModified);
                return _mapper.Map<List<NhanVien_CalamViecViewModel>>(lst);
            }
            else
            {
                var lst = _nhanvienClviecRepository.FindAll(x => (x.Status.Contains(keyword) || x.HR_NHANVIEN.MaBoPhan.Contains(keyword)) && (x.Status == Status.Active.ToString() || x.Status == Status.New.ToString()), includeProperties).OrderByDescending(x => x.DateModified);
                return _mapper.Map<List<NhanVien_CalamViecViewModel>>(lst);
            }
        }

        public NhanVien_CalamViecViewModel GetById(int id, params Expression<Func<NHANVIEN_CALAMVIEC, object>>[] includeProperties)
        {
            if (id > 0)
            {
                return _mapper.Map<NhanVien_CalamViecViewModel>(_nhanvienClviecRepository.FindById(id, includeProperties));
            }
            else
            {
                return null;
            }
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
                    table.Columns.Add("CaLV_DB");
                    table.Columns.Add("BatDau_TheoCa");
                    table.Columns.Add("KetThuc_TheoCa");
                    table.Columns.Add("Status");

                    DataRow row = null;
                    string dmCalviec = "";
                    string calv = "";
                    string dateCheck = "";
                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                    {
                        row = table.NewRow();

                        if (string.IsNullOrEmpty(worksheet.Cells[i, 1].Text.NullString().ToUpper()))
                        {
                            continue;
                        }

                        row["MaNV"] = worksheet.Cells[i, 1].Text.NullString().ToUpper();

                        dmCalviec = worksheet.Cells[i, 3].Text.NullString();
                        if (lstDmCalamViec.Any(x => dmCalviec.Contains(x.TenCaLamViec)))
                        {
                            calv = lstDmCalamViec.FirstOrDefault(x => dmCalviec.Contains(x.TenCaLamViec))?.Id.NullString();
                            if (calv == "CD_WHC" || calv == "CN_WHC")
                            {
                                row["Danhmuc_CaLviec"] = calv;
                                row["CaLV_DB"] = "";
                            }
                            else if (calv == "CD_CN") // ca dem con nho
                            {
                                row["Danhmuc_CaLviec"] = "CD_WHC";
                                row["CaLV_DB"] = "CD_CN";
                            }
                            else if (calv == "CN_CN") // ca ngay con nho
                            {
                                row["Danhmuc_CaLviec"] = "CN_WHC";
                                row["CaLV_DB"] = "CN_CN";
                            }
                            else if (calv == "VP_CN") // ca ngay con nho
                            {
                                row["Danhmuc_CaLviec"] = "CN_WHC";
                                row["CaLV_DB"] = "VP_CN";
                            }
                            else if (calv == "TS")
                            {
                                row["Danhmuc_CaLviec"] = "CN_WHC";
                                row["CaLV_DB"] = "TS";
                            }
                        }
                        else
                        {
                            throw new Exception("Not found danh muc ca lam viec");
                        }

                        if (!ValidateCommon.DateTimeValid(worksheet.Cells[i, 4].Text.NullString()))
                        {
                            throw new Exception("Ngày phải có định dạng: yyyy-MM-dd");
                        }

                        if (!ValidateCommon.DateTimeValid(worksheet.Cells[i, 5].Text.NullString()))
                        {
                            throw new Exception("Ngày phải có định dạng: yyyy-MM-dd");
                        }

                        DateTime dStart = DateTime.Parse(worksheet.Cells[i, 4].Text.NullString());
                        DateTime dEnd = DateTime.Parse(worksheet.Cells[i, 5].Text.NullString());

                        row["BatDau_TheoCa"] = dStart.ToString("yyyy-MM-dd");
                        row["KetThuc_TheoCa"] = dEnd.ToString("yyyy-MM-dd");

                        foreach (DateTime day in EachDay.EachDays(dStart, dEnd))
                        {
                            dateCheck = day.ToString("yyyy-MM-dd");
                            if(_nhanvienClviecRepository.FindAll(x => x.MaNV == row["MaNV"].NullString() && string.Compare(x.BatDau_TheoCa, dateCheck) <= 0 && string.Compare(x.KetThuc_TheoCa, dateCheck) >= 0).Count() > 0)
                            {
                                throw new Exception("Ca làm việc bị trùng ngày: "+ dateCheck + " Mã NV: "+ row["MaNV"].NullString());
                            }
                        }

                        if (string.Compare(DateTime.Now.ToString("yyyy-MM-dd"), dStart.ToString("yyyy-MM-dd")) >= 0 && string.Compare(DateTime.Now.ToString("yyyy-MM-dd"), dEnd.ToString("yyyy-MM-dd")) <= 0)
                        {
                            row["Status"] = Status.Active.ToString();
                        }
                        else if (string.Compare(DateTime.Now.ToString("yyyy-MM-dd"), dStart.ToString("yyyy-MM-dd")) < 0)
                        {
                            row["Status"] = Status.New.ToString();
                        }
                        else
                        {
                            row["Status"] = Status.InActive.ToString();
                        }
                        table.Rows.Add(row);
                    }

                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add("A_USER_UPDATE", GetUserId());
                    resultDB = _nhanvienClviecRepository.ExecProceduce("PKG_BUSINESS.PUT_NHANVIEN_CALAMVIEC", dic, "A_DATA", table);
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
            _unitOfWork.Commit();
        }

        public List<NhanVien_CalamViecViewModel> Search(string dept, string status, string timeFrom, string timeTo, params Expression<Func<NHANVIEN_CALAMVIEC, object>>[] includeProperties)
        {
            if (!string.IsNullOrEmpty(timeFrom) && !string.IsNullOrEmpty(timeTo))
            {
                List<NhanVien_CalamViecViewModel> lstResult = new List<NhanVien_CalamViecViewModel>();

                if (string.IsNullOrEmpty(dept) && string.IsNullOrEmpty(status))
                {
                    var lst = _nhanvienClviecRepository.FindAll(includeProperties).OrderByDescending(x => x.DateModified);
                    lstResult = _mapper.Map<List<NhanVien_CalamViecViewModel>>(lst);
                }

                if (!string.IsNullOrEmpty(dept) && string.IsNullOrEmpty(status))
                {
                    var lst = _nhanvienClviecRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept, includeProperties).OrderByDescending(x => x.DateModified);
                    lstResult = _mapper.Map<List<NhanVien_CalamViecViewModel>>(lst);
                }

                if (string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(status))
                {
                    if (status != CommonConstants.No_Approved && status != CommonConstants.Approved)
                    {
                        var lst = _nhanvienClviecRepository.FindAll(x => x.Status == status, includeProperties).OrderByDescending(x => x.DateModified);
                        lstResult = _mapper.Map<List<NhanVien_CalamViecViewModel>>(lst);
                    }
                    else
                    {
                        string[] itemCheck = status == CommonConstants.No_Approved ? new string[] { "", null } : new string[] { status };
                        var lst = _nhanvienClviecRepository.FindAll(x => (x.Approved == status || itemCheck.Contains(x.Approved)), includeProperties).OrderByDescending(x => x.DateModified);
                        lstResult = _mapper.Map<List<NhanVien_CalamViecViewModel>>(lst);
                    }
                }

                if (!string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(status))
                {
                    if (status != CommonConstants.No_Approved && status != CommonConstants.Approved)
                    {
                        var lst = _nhanvienClviecRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept && x.Status == status, includeProperties).OrderByDescending(x => x.DateModified);
                        lstResult = _mapper.Map<List<NhanVien_CalamViecViewModel>>(lst);
                    }
                    else
                    {
                        string[] itemCheck = status == CommonConstants.No_Approved ? new string[] { "", null } : new string[] { status };
                        var lst = _nhanvienClviecRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept && (x.Approved == status || itemCheck.Contains(x.Approved)), includeProperties).OrderByDescending(x => x.DateModified);
                        lstResult = _mapper.Map<List<NhanVien_CalamViecViewModel>>(lst);
                    }
                }

                DateTime dFrom = DateTime.Parse(timeFrom);
                DateTime dTo = DateTime.Parse(timeTo);
                string dateCheck = "";
                List<NhanVien_CalamViecViewModel> nvclviec;
                List<NhanVien_CalamViecViewModel> lstResult2 = new List<NhanVien_CalamViecViewModel>();
                foreach (DateTime day in EachDay.EachDays(dFrom, dTo))
                {
                    dateCheck = day.ToString("yyyy-MM-dd");
                    nvclviec = lstResult.FindAll(x => string.Compare(x.BatDau_TheoCa, dateCheck) <= 0 && string.Compare(x.KetThuc_TheoCa, dateCheck) >= 0);
                    if (nvclviec != null && nvclviec.Count > 0)
                    {
                        foreach (var item in nvclviec)
                        {
                            if (!lstResult2.Contains(item))
                            {
                                lstResult2.Add(item);
                            }
                        }
                    }
                }
                return lstResult2;

            }
            else
            {
                if (string.IsNullOrEmpty(dept) && string.IsNullOrEmpty(status))
                {
                    return GetAll("", x => x.HR_NHANVIEN, y => y.DM_CA_LVIEC);
                }

                if (!string.IsNullOrEmpty(dept) && string.IsNullOrEmpty(status))
                {
                    return GetAll(dept, x => x.HR_NHANVIEN, y => y.DM_CA_LVIEC);
                }

                if (string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(status))
                {
                    if (status != CommonConstants.No_Approved && status != CommonConstants.Approved)
                    {
                        var lst = _nhanvienClviecRepository.FindAll(x => x.Status == status, includeProperties).OrderByDescending(x => x.DateModified);
                        return _mapper.Map<List<NhanVien_CalamViecViewModel>>(lst);
                    }
                    else
                    {
                        string[] itemCheck = status == CommonConstants.No_Approved ? new string[] { "", null } : new string[] { status };
                        var lst = _nhanvienClviecRepository.FindAll(x => x.Approved == status || itemCheck.Contains(x.Approved), includeProperties).OrderByDescending(x => x.DateModified);
                        return _mapper.Map<List<NhanVien_CalamViecViewModel>>(lst);
                    }
                }

                if (!string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(status))
                {
                    if (status != CommonConstants.No_Approved && status != CommonConstants.Approved)
                    {
                        var lst = _nhanvienClviecRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept && x.Status == status, includeProperties).OrderByDescending(x => x.DateModified);
                        return _mapper.Map<List<NhanVien_CalamViecViewModel>>(lst);
                    }
                    else
                    {
                        string[] itemCheck = status == CommonConstants.No_Approved ? new string[] { "", null } : new string[] { status };
                        var lst = _nhanvienClviecRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept && (x.Approved == status || itemCheck.Contains(x.Approved)), includeProperties).OrderByDescending(x => x.DateModified);
                        return _mapper.Map<List<NhanVien_CalamViecViewModel>>(lst);
                    }
                }
            }

            return new List<NhanVien_CalamViecViewModel>();
        }

        public void Update(NhanVien_CalamViecViewModel nhanVienLVVm)
        {
            nhanVienLVVm.UserModified = GetUserId();
            if (string.Compare(DateTime.Now.ToString("yyyy-MM-dd"), nhanVienLVVm.BatDau_TheoCa) >= 0 && string.Compare(DateTime.Now.ToString("yyyy-MM-dd"), nhanVienLVVm.KetThuc_TheoCa) <= 0)
            {
                nhanVienLVVm.Status = Status.Active.NullString();
            }
            else if (string.Compare(DateTime.Now.ToString("yyyy-MM-dd"), nhanVienLVVm.BatDau_TheoCa) < 0)
            {
                nhanVienLVVm.Status = Status.New.ToString();
            }
            else
            {
                nhanVienLVVm.Status = Status.InActive.ToString();
            }
            var entity = _mapper.Map<NHANVIEN_CALAMVIEC>(nhanVienLVVm);
            _nhanvienClviecRepository.Update(entity);
        }

        public void UpdateSingle(NhanVien_CalamViecViewModel nhanVienLVVm)
        {
            throw new NotImplementedException();
        }

        public NhanVien_CalamViecViewModel CheckExist(int id, string maNV, string from, string end)
        {
            if (id > 0)
            {
                return GetById(id);
            }
            else
            {
                var obj = _nhanvienClviecRepository.FindSingle(x => x.MaNV.Contains(maNV) && x.BatDau_TheoCa == from && x.KetThuc_TheoCa == end);

                if (obj != null)
                    return _mapper.Map<NhanVien_CalamViecViewModel>(obj);

                return null;
            }
        }

        public void Approve(string dept, string status, bool isApprove)
        {
            var lst = _nhanvienClviecRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept && x.Approved == status || x.Approved == null, y => y.HR_NHANVIEN).ToList();

            foreach (var item in lst)
            {
                item.Approved = isApprove ? CommonConstants.Approved : CommonConstants.No_Approved;
                item.HR_NHANVIEN = null;
            }
            _nhanvienClviecRepository.UpdateRange(lst);
        }

        public void ApproveSingle(int Id, bool isApprove)
        {
            var obj = _nhanvienClviecRepository.FindById(Id);
            obj.Approved = isApprove ? CommonConstants.Approved : CommonConstants.No_Approved;
            _nhanvienClviecRepository.Update(obj);
        }

        public void UpdateRange(List<NhanVien_CalamViecViewModel> OTVms)
        {
            var lstEntity = _mapper.Map<List<NHANVIEN_CALAMVIEC>>(OTVms);
            foreach (var item in lstEntity)
            {
                item.UserModified = GetUserId();
            }
            _nhanvienClviecRepository.UpdateRange(lstEntity);
        }

        public NhanVien_CalamViecViewModel FindCaLamViecByDay(string maNV, string time)
        {
            var obj = _nhanvienClviecRepository.FindAll(x => x.MaNV.Contains(maNV) && x.BatDau_TheoCa.CompareTo(time) <= 0 && x.KetThuc_TheoCa.CompareTo(time) >= 0, x => x.DM_CA_LVIEC).OrderByDescending(x => x.BatDau_TheoCa).FirstOrDefault();

            if (obj != null)
                return _mapper.Map<NhanVien_CalamViecViewModel>(obj);

            return null;
        }
    }
}
