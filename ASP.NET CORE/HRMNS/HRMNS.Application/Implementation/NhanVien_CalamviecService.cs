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

namespace HRMNS.Application.Implementation
{
    public class NhanVien_CalamviecService : BaseService, INhanVien_CalamviecService
    {
        private IRespository<NHANVIEN_CALAMVIEC, int> _nhanvienClviecRepository;
        private IRespository<DM_CA_LVIEC, string> _dmCalamviecResponsitory;
        private IRespository<SETTING_TIME_CA_LVIEC,int> _settingTimeCalamviec;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NhanVien_CalamviecService(IRespository<NHANVIEN_CALAMVIEC, int> respository, IRespository<DM_CA_LVIEC, string> dmCalamviecRespository, IRespository<SETTING_TIME_CA_LVIEC,int> settingTimeCalamviec, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _nhanvienClviecRepository = respository;
            _dmCalamviecResponsitory = dmCalamviecRespository;
            _settingTimeCalamviec = settingTimeCalamviec;
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

                        dmCalviec = worksheet.Cells[i, 3].Text.NullString();
                        if (lstDmCalamViec.Any(x => dmCalviec.Contains(x.TenCaLamViec)))
                        {
                            row["Danhmuc_CaLviec"] = lstDmCalamViec.FirstOrDefault(x => dmCalviec.Contains(x.TenCaLamViec))?.Id.NullString();
                        }
                        else
                        {
                            throw new Exception("Not found danh muc ca lam viec");
                        }

                        var calaviecActive = _settingTimeCalamviec.FindAll(x => x.Status == Status.Active.ToString() && x.CaLamViec == row["Danhmuc_CaLviec"].NullString()).OrderByDescending(x => x.DateModified).FirstOrDefault();

                        if(calaviecActive != null)
                        {
                            row["BatDau_TheoCa"] = calaviecActive.NgayBatDau;
                            row["KetThuc_TheoCa"] = calaviecActive.NgayKetThuc;

                            if (string.Compare(DateTime.Now.ToString("yyyy-MM-dd"), calaviecActive.NgayBatDau) >= 0 && string.Compare(DateTime.Now.ToString("yyyy-MM-dd"), calaviecActive.NgayKetThuc) <= 0)
                            {
                                row["Status"] = Status.Active.ToString();
                            }
                            else if (string.Compare(DateTime.Now.ToString("yyyy-MM-dd"), calaviecActive.NgayBatDau) < 0)
                            {
                                row["Status"] = Status.New.ToString();
                            }
                            else
                            {
                                row["Status"] = Status.InActive.ToString();
                            }
                        }
                        else
                        {
                            throw new Exception("Not found ca lam viec");
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
            _unitOfWork.Commit();
        }

        public List<NhanVien_CalamViecViewModel> Search(string dept, string status, string timeFrom, string timeTo, params Expression<Func<NHANVIEN_CALAMVIEC, object>>[] includeProperties)
        {
            if (!string.IsNullOrEmpty(timeFrom) && !string.IsNullOrEmpty(timeTo))
            {
                if (string.IsNullOrEmpty(dept) && string.IsNullOrEmpty(status))
                {
                    var lst = _nhanvienClviecRepository.FindAll(x => string.Compare(x.BatDau_TheoCa, timeFrom) >= 0 && string.Compare(x.KetThuc_TheoCa, timeTo) <= 0, includeProperties).OrderByDescending(x => x.DateModified);
                    return _mapper.Map<List<NhanVien_CalamViecViewModel>>(lst);
                }

                if (!string.IsNullOrEmpty(dept) && string.IsNullOrEmpty(status))
                {
                    var lst = _nhanvienClviecRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept && (string.Compare(x.BatDau_TheoCa, timeFrom) >= 0 && string.Compare(x.KetThuc_TheoCa, timeTo) <= 0), includeProperties).OrderByDescending(x => x.DateModified);
                    return _mapper.Map<List<NhanVien_CalamViecViewModel>>(lst);
                }

                if (string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(status))
                {
                    if (status != CommonConstants.No_Approved && status != CommonConstants.Approved)
                    {
                        var lst = _nhanvienClviecRepository.FindAll(x => x.Status == status && (string.Compare(x.BatDau_TheoCa, timeFrom) >= 0 && string.Compare(x.KetThuc_TheoCa, timeTo) <= 0), includeProperties).OrderByDescending(x => x.DateModified);
                        return _mapper.Map<List<NhanVien_CalamViecViewModel>>(lst);
                    }
                    else
                    {
                        string[] itemCheck = status == CommonConstants.No_Approved ? new string[] { "", null } : new string[] { status };
                        var lst = _nhanvienClviecRepository.FindAll(x => (x.Approved == status || itemCheck.Contains(x.Approved)) && (string.Compare(x.BatDau_TheoCa, timeFrom) >= 0 && string.Compare(x.KetThuc_TheoCa, timeTo) <= 0), includeProperties).OrderByDescending(x => x.DateModified);
                        return _mapper.Map<List<NhanVien_CalamViecViewModel>>(lst);
                    }
                }

                if (!string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(status))
                {
                    if (status != CommonConstants.No_Approved && status != CommonConstants.Approved)
                    {
                        var lst = _nhanvienClviecRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept && x.Status == status && (string.Compare(x.BatDau_TheoCa, timeFrom) >= 0 && string.Compare(x.KetThuc_TheoCa, timeTo) <= 0), includeProperties).OrderByDescending(x => x.DateModified);
                        return _mapper.Map<List<NhanVien_CalamViecViewModel>>(lst);
                    }
                    else
                    {
                        string[] itemCheck = status == CommonConstants.No_Approved ? new string[] { "", null } : new string[] { status };
                        var lst = _nhanvienClviecRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept && (x.Approved == status || itemCheck.Contains(x.Approved)) && (string.Compare(x.BatDau_TheoCa, timeFrom) >= 0 && string.Compare(x.KetThuc_TheoCa, timeTo) <= 0), includeProperties).OrderByDescending(x => x.DateModified);
                        return _mapper.Map<List<NhanVien_CalamViecViewModel>>(lst);
                    }
                }
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

        public NhanVien_CalamViecViewModel CheckExist(int id, string maNV, string dmCa, string from, string end)
        {
            if (id > 0)
            {
                return GetById(id);
            }
            else
            {
                var obj = _nhanvienClviecRepository.FindSingle(x => x.MaNV == maNV && x.Danhmuc_CaLviec == dmCa && x.BatDau_TheoCa == from && x.KetThuc_TheoCa == end);
                return _mapper.Map<NhanVien_CalamViecViewModel>(obj);
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
    }
}
