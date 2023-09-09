using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMNS.Data.Enums;
using HRMNS.Utilities.Common;
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
        private IRespository<HR_NHANVIEN, string> _nhanVienRespository;

        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DangKyChamCongDacBietService(IRespository<DANGKY_CHAMCONG_DACBIET, int> chamCongDbRepository, IRespository<HR_NHANVIEN, string> nhanVienRespository, IRespository<DANGKY_CHAMCONG_CHITIET, int> respository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _chamCongDbRepository = chamCongDbRepository;
            _nhanVienRespository = nhanVienRespository;
            _chamCongChiTietRepository = respository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        private void UpdateNghiViecNV(DangKyChamCongDacBietViewModel chamCongVm)
        {
            var chitiet = _chamCongChiTietRepository.FindAll(x => x.Id == chamCongVm.MaChamCong_ChiTiet).FirstOrDefault();
            if (chitiet != null && chitiet.KyHieuChamCong == "T") // nghi viec
            {
                var nv = _nhanVienRespository.FindById(chamCongVm.MaNV);
                if (nv != null)
                {
                    nv.NgayNghiViec = chamCongVm.NgayBatDau;

                    if (string.Compare(DateTime.Now.ToString("yyyy-MM-dd"), chamCongVm.NgayBatDau) >= 0)
                    {
                        nv.Status = Status.InActive.ToString();
                    }

                    _nhanVienRespository.Update(nv);
                }
            }
        }

        public DangKyChamCongDacBietViewModel Add(DangKyChamCongDacBietViewModel chamCongVm)
        {
            UpdateNghiViecNV(chamCongVm);

            chamCongVm.UserCreated = GetUserId();
            var entity = _mapper.Map<DANGKY_CHAMCONG_DACBIET>(chamCongVm);
            _chamCongDbRepository.Add(entity);

            return chamCongVm;
        }

        public void Delete(int id)
        {
            var entity = _chamCongDbRepository.FindById(id, x => x.DANGKY_CHAMCONG_CHITIET);
            if (entity.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong == "T") // nghi viec
            {
                var nv = _nhanVienRespository.FindById(entity.MaNV);
                if (nv != null)
                {
                    nv.NgayNghiViec = "";
                    nv.Status = Status.Active.ToString();
                    _nhanVienRespository.Update(nv);
                }
            }
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
            DANGKY_CHAMCONG_DACBIET en = _chamCongDbRepository.FindById(id);
            return _mapper.Map<DangKyChamCongDacBietViewModel>(en);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(DangKyChamCongDacBietViewModel chamCongVm)
        {
            UpdateNghiViecNV(chamCongVm);
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
                    return GetAll(includeProperties).Where(x=>x.NgayBatDau.CompareTo(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd")) > 0).ToList();
                }

                return _mapper.Map<List<DangKyChamCongDacBietViewModel>>(_chamCongDbRepository.FindAll(x => (x.NgayKetThuc.CompareTo(fromDate) >= 0 && x.NgayBatDau.CompareTo(toDate) <= 0), includeProperties).OrderByDescending(x => x.DateModified));
            }
            else
            {
                if (string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
                {
                    fromDate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM")+"-01";
                    toDate = DateTime.Now.ToString("yyyy-MM-dd");
                }

                return _mapper.Map<List<DangKyChamCongDacBietViewModel>>(_chamCongDbRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept && (x.NgayKetThuc.CompareTo(fromDate) >= 0 && x.NgayBatDau.CompareTo(toDate) <= 0), includeProperties).OrderByDescending(x => x.DateCreated));
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
                bool isSave = false;
                using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = packet.Workbook.Worksheets[0];

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

                        if (ValidateCommon.DateTimeValid(worksheet.Cells[i, 4].Text.NullString()))
                        {
                            row["NgayBatDau"] = worksheet.Cells[i, 4].Text.NullString();
                        }
                        else
                        {
                            throw new Exception("Định dạng ngày phải là : yyyy-MM-dd");
                        }

                        if (ValidateCommon.DateTimeValid(worksheet.Cells[i, 5].Text.NullString()))
                        {
                            row["NgayKetThuc"] = worksheet.Cells[i, 5].Text.NullString();
                        }
                        else
                        {
                            throw new Exception("Định dạng ngày phải là : yyyy-MM-dd");
                        }

                        if (worksheet.Cells[i, 6].Text.NullString() != "")
                        {
                            row["NoiDung"] = worksheet.Cells[i, 2].Text.NullString() + " " + worksheet.Cells[i, 6].Text.NullString();
                        }
                        else
                        {
                            if (CommonConstants.arrNghiPhep.Contains(kytuChamCong))
                            {
                                row["NoiDung"] = worksheet.Cells[i, 2].Text.NullString() + " " + worksheet.Cells[i, 3].Text.NullString();
                            }
                        }

                        //row["Approve"] = CommonConstants.Request;
                        //row["ApproveLV2"] = CommonConstants.Request;

                        //if (role == CommonConstants.AppRole.AdminRole || role == CommonConstants.roleApprove3)
                        //{
                        //    row["Approve"] = CommonConstants.Approved;
                        //    row["ApproveLV2"] = CommonConstants.Approved;
                        //    row["ApproveLV3"] = CommonConstants.Approved;
                        //}
                        //else
                        //{
                        //    row["ApproveLV3"] = CommonConstants.Request;
                        //}

                        row["Approve"] = CommonConstants.Approved;
                        row["ApproveLV2"] = CommonConstants.Approved;
                        row["ApproveLV3"] = CommonConstants.Request;
                        if (role == CommonConstants.AppRole.AdminRole || role == CommonConstants.roleApprove3)
                        {
                            row["ApproveLV3"] = CommonConstants.Approved;
                        }

                        if (kytuChamCong == "T") // nghi viec
                        {
                            var nv = _nhanVienRespository.FindById(row["MaNV"].NullString());
                            if (nv != null)
                            {
                                nv.NgayNghiViec = row["NgayBatDau"].NullString();

                                if (string.Compare(DateTime.Now.ToString("yyyy-MM-dd"), nv.NgayNghiViec) >= 0)
                                {
                                    nv.Status = Status.InActive.ToString();
                                }

                                _nhanVienRespository.Update(nv);
                                isSave = true;
                            }
                        }

                        table.Rows.Add(row);
                    }

                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add("A_USER", GetUserId());
                    resultDB = _chamCongDbRepository.ExecProceduce("PKG_BUSINESS.PUT_CHAMCONG_DB", dic, "A_DATA", table);
                }

                if (resultDB.ReturnInt == 0 && isSave)
                {
                    Save();
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
