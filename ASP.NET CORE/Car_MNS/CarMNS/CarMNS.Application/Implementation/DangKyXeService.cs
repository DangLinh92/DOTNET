using CarMNS.Application.Interfaces;
using CarMNS.Data.EF;
using CarMNS.Data.EF.Extensions;
using CarMNS.Data.Entities;
using CarMNS.Infrastructure.Interfaces;
using CarMNS.Utilities.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarMNS.Application.Implementation
{
    public class DangKyXeService : BaseService, IDangKyXeService
    {
        private IUnitOfWork _unitOfWork;
        private IRespository<DANG_KY_XE, int> _DangKyXeRepository;
        private IRespository<BOPHAN, string> _BoPhanRepository;
        private IRespository<DIEUXE_DANGKY, int> _DieuXeRepository;
        private IRespository<LAI_XE, int> _LaiXeRepository;
        private IRespository<CAR, int> _XeRepository;

        public DangKyXeService(IRespository<LAI_XE, int> LaiXeRepository, IRespository<CAR, int> XeRepository, IUnitOfWork unitOfWork, IRespository<DANG_KY_XE, int> dangKyXeRepository,
            IRespository<BOPHAN, string> boPhanRepository, IRespository<DIEUXE_DANGKY, int> dieuXeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _DangKyXeRepository = dangKyXeRepository;
            _BoPhanRepository = boPhanRepository;
            _DieuXeRepository = dieuXeRepository;
            _LaiXeRepository = LaiXeRepository;
            _XeRepository = XeRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public DANG_KY_XE AddDangKyXe(DANG_KY_XE dangky, string role)
        {
            try
            {
                if (role == CommonConstants.ROLE_GROUP_LD)
                {
                    dangky.XacNhanLV1 = true;
                    dangky.Nguoi_XacNhanLV1 = GetUserId() + ":Approve";
                }
                else if (role == CommonConstants.ROLE_HR)
                {
                    dangky.XacNhanLV1 = true;
                    dangky.Nguoi_XacNhanLV1 = GetUserId() + ":Approve";

                    dangky.XacNhanLV2 = true;
                    dangky.Nguoi_XacNhanLV2 = GetUserId() + ":Approve";

                }
                else if (role == CommonConstants.ROLE_HR_TOP)
                {
                    dangky.XacNhanLV1 = true;
                    dangky.Nguoi_XacNhanLV1 = GetUserId() + ":Approve";

                    dangky.XacNhanLV2 = true;
                    dangky.Nguoi_XacNhanLV2 = GetUserId() + ":Approve";

                    dangky.XacNhanLV3 = true;
                    dangky.Nguoi_XacNhanLV3 = GetUserId() + ":Approve";
                }

                dangky.NguoiDangKy = ((EFUnitOfWork)_unitOfWork).DBContext().AppUsers.ToList().FirstOrDefault(x => x.UserName == GetUserId()).FullName;
                _DangKyXeRepository.Add(dangky);
                Save();
                return dangky;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DIEUXE_DANGKY AddXe(DIEUXE_DANGKY en)
        {
            try
            {
                _DieuXeRepository.Add(en);
                Save();
                return en;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void DeleteDangKyXe(int id)
        {
            _DangKyXeRepository.Remove(id);
            Save();
        }

        public void DeleteXe(int id)
        {
            _DieuXeRepository.Remove(id);
            Save();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<DANG_KY_XE> GetAllDangKyXe(string role, string bophan)
        {
            var lst = _DangKyXeRepository.FindAll(x => x.DIEUXE_DANGKY).ToList();
            List<LAI_XE> lstXe = _LaiXeRepository.FindAll(x => x.DIEUXE_DANGKY).ToList();
            List<CAR> Xe = _XeRepository.FindAll(x => x.DIEUXE_DANGKY).ToList();

            foreach (var item in lst)
            {
                item.Lxe_BienSo = "";

                foreach (var dx in item.DIEUXE_DANGKY)
                {
                    item.Lxe_BienSo += lstXe.FirstOrDefault(x => x.Id == dx.MaLaiXe).HoTen + ": " + Xe.FirstOrDefault(x => x.Id == dx.MaXe).BienSoXe + "\n";
                }
            }

            if (role == CommonConstants.ROLE_DANGKY || role == CommonConstants.ROLE_GROUP_LD)
            {
                lst = lst.FindAll(x => x.BoPhan == bophan).Where(x => x.XacNhanLV2 == false || (x.XacNhanLV2 == true && x.DateModified.CompareTo(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd HH:mm:ss")) >= 0)).OrderByDescending(x => x.DateModified).ToList();
            }
            else if (role == CommonConstants.ROLE_HR)
            {
                lst = lst.Where(x => x.XacNhanLV1 == true && (x.XacNhanLV2 == false || (x.XacNhanLV2 == true && x.DateModified.CompareTo(DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss")) >= 0))).OrderByDescending(x => x.DateModified).ToList();
            }
            else if (role == CommonConstants.ROLE_HR_TOP || role == CommonConstants.AppRole.AdminRole)
            {
                lst = lst.Where(x => x.XacNhanLV2 == true && (x.XacNhanLV3 == false || (x.XacNhanLV3 == true && x.DateModified.CompareTo(DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd HH:mm:ss")) >= 0))).OrderByDescending(x => x.DateModified).ToList();
            }

            return lst;
        }

        public List<BOPHAN> GetBoPhan()
        {
            return _BoPhanRepository.FindAll().ToList();
        }

        public DANG_KY_XE GetDangKyXeById(int id)
        {
            return _DangKyXeRepository.FindById(id);
        }

        public DIEUXE_DANGKY GetDieuXeById(int id)
        {
            return _DieuXeRepository.FindById(id);
        }

        public List<DIEUXE_DANGKY> GetXe(int maDangKy)
        {
            return _DieuXeRepository.FindAll(x => x.MaDangKyXe == maDangKy, x => x.Car).ToList();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public bool UnApprove(int maDangKy, string role)
        {
            try
            {
                var dangky = _DangKyXeRepository.FindById(maDangKy);
                if (dangky != null)
                {
                    if (role == CommonConstants.ROLE_GROUP_LD)
                    {
                        dangky.XacNhanLV1 = false;
                        dangky.Nguoi_XacNhanLV1 = GetUserId() + ":UnApprove";
                    }
                    else if (role == CommonConstants.ROLE_HR)
                    {
                        dangky.XacNhanLV2 = false;
                        dangky.Nguoi_XacNhanLV2 = GetUserId() + ":UnApprove";
                    }
                    else if (role == CommonConstants.ROLE_HR_TOP || role == CommonConstants.AppRole.AdminRole)
                    {
                        dangky.XacNhanLV3 = false;
                        dangky.Nguoi_XacNhanLV3 = GetUserId() + ":UnApprove";
                    }

                    _DangKyXeRepository.Update(dangky);
                    Save();
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }
            return false;
        }

        public bool Approve(int maDangKy, string role)
        {
            try
            {
                var dangky = _DangKyXeRepository.FindById(maDangKy);
                if (dangky != null)
                {
                    if (role == CommonConstants.ROLE_GROUP_LD)
                    {
                        dangky.XacNhanLV1 = true;
                        dangky.Nguoi_XacNhanLV1 = GetUserId() + ":Approve";
                    }
                    else if (role == CommonConstants.ROLE_HR)
                    {
                        if (!dangky.XacNhanLV1)
                        {
                            dangky.XacNhanLV1 = true;
                            dangky.Nguoi_XacNhanLV1 = GetUserId() + ":Approve";
                        }

                        dangky.XacNhanLV2 = true;
                        dangky.Nguoi_XacNhanLV2 = GetUserId() + ":Approve";
                    }
                    else if (role == CommonConstants.ROLE_HR_TOP || role == CommonConstants.AppRole.AdminRole)
                    {
                        if (!dangky.XacNhanLV1)
                        {
                            dangky.XacNhanLV1 = true;
                            dangky.Nguoi_XacNhanLV1 = GetUserId() + ":Approve";
                        }

                        if (!dangky.XacNhanLV2)
                        {
                            dangky.XacNhanLV2 = true;
                            dangky.Nguoi_XacNhanLV2 = GetUserId() + ":Approve";
                        }

                        dangky.XacNhanLV3 = true;
                        dangky.Nguoi_XacNhanLV3 = GetUserId() + ":Approve";
                    }

                    _DangKyXeRepository.Update(dangky);
                    Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        public DANG_KY_XE UpdateDangKyXe(DANG_KY_XE dangky)
        {
            _DangKyXeRepository.Update(dangky);
            Save();
            return dangky;
        }

        public DIEUXE_DANGKY UpdateXe(DIEUXE_DANGKY en)
        {
            _DieuXeRepository.Update(en);
            Save();
            return en;
        }

        public Dictionary<string, List<string>> GetUserSendMail(int maDangKy, bool isNew, bool isApprove, bool isUnApprove)
        {
            List<APP_USER> APP_USER = ((EFUnitOfWork)_unitOfWork).DBContext().AppUsers.ToList();
            List<APP_ROLE> APP_ROLE = ((EFUnitOfWork)_unitOfWork).DBContext().AppRoles.ToList();
            List<APP_USER_ROLE> APP_USER_ROLE = ((EFUnitOfWork)_unitOfWork).DBContext().AppUserRoles.ToList();

            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
            List<string> lstSendNext = new List<string>();
            List<string> lstSendPre = new List<string>();
            APP_USER user = APP_USER.FirstOrDefault(x => x.UserName == GetUserId());
            Guid userId = user.Id;
            string bophan = user.Department;

            Guid roleId = APP_USER_ROLE.FindAll(x => x.UserId.Equals(userId)).FirstOrDefault().RoleId;
            string roleName = APP_ROLE.FindLast(x => x.Id.Equals(roleId)).Name;

            // Tạo mới
            if (isNew)
            {
                if (roleName == CommonConstants.ROLE_DANGKY)
                {
                    Guid roleDuyetBP = APP_ROLE.FirstOrDefault(x => x.Name == CommonConstants.ROLE_GROUP_LD).Id;
                    List<Guid> lstUserId = APP_USER_ROLE.FindAll(x => x.RoleId.Equals(roleDuyetBP)).Select(x => x.UserId).ToList();
                    lstSendNext = APP_USER.FindAll(x => x.Department == bophan && lstUserId.Contains(x.Id)).Select(x => x.Email).ToList();
                }
                else if (roleName == CommonConstants.ROLE_GROUP_LD)
                {
                    Guid roleDuyetHR = APP_ROLE.FirstOrDefault(x => x.Name == CommonConstants.ROLE_HR).Id;
                    List<Guid> lstUserId = APP_USER_ROLE.FindAll(x => x.RoleId.Equals(roleDuyetHR)).Select(x => x.UserId).ToList();

                    lstSendNext = APP_USER.FindAll(x => lstUserId.Contains(x.Id)).Select(x => x.Email).ToList();
                }
                else if (roleName == CommonConstants.ROLE_HR)
                {
                    Guid roleDuyetLeadHR = APP_ROLE.FirstOrDefault(x => x.Name == CommonConstants.ROLE_HR_TOP).Id;
                    List<Guid> lstUserId = APP_USER_ROLE.FindAll(x => x.RoleId.Equals(roleDuyetLeadHR)).Select(x => x.UserId).ToList();
                    lstSendNext = APP_USER.FindAll(x => lstUserId.Contains(x.Id)).Select(x => x.Email).ToList();
                }
            }
            else if (isApprove || isUnApprove) // approve/ unapprove
            {
                if (roleName == CommonConstants.ROLE_GROUP_LD)
                {
                    if (isApprove)
                    {
                        Guid roleDuyetHR = APP_ROLE.FirstOrDefault(x => x.Name == CommonConstants.ROLE_HR).Id;
                        List<Guid> lstUserId = APP_USER_ROLE.FindAll(x => x.RoleId.Equals(roleDuyetHR)).Select(x => x.UserId).ToList();
                        lstSendNext = APP_USER.FindAll(x => lstUserId.Contains(x.Id)).Select(x => x.Email).ToList();
                    }

                    DANG_KY_XE xe = _DangKyXeRepository.FindById(maDangKy);
                    if (xe != null)
                    {
                        APP_USER us = APP_USER.FirstOrDefault(x => x.UserName == xe.UserCreated);

                        if (us != null)
                        {
                            lstSendPre.Add(us.Email.NullString());
                        }
                    }
                }
                else if (roleName == CommonConstants.ROLE_HR)
                {
                    if (isApprove)
                    {
                        Guid roleDuyetLeadHR = APP_ROLE.FirstOrDefault(x => x.Name == CommonConstants.ROLE_HR_TOP).Id;
                        List<Guid> lstUserId = APP_USER_ROLE.FindAll(x => x.RoleId.Equals(roleDuyetLeadHR)).Select(x => x.UserId).ToList();
                        lstSendNext = APP_USER.FindAll(x => lstUserId.Contains(x.Id)).Select(x => x.Email).ToList();
                    }

                    DANG_KY_XE xe = _DangKyXeRepository.FindById(maDangKy);
                    if (xe != null)
                    {
                        APP_USER us = APP_USER.FirstOrDefault(x => x.UserName == xe.UserCreated);
                        string userAp1 = xe.Nguoi_XacNhanLV1.NullString().Split(':')[0].NullString();
                        APP_USER usApLV1 = APP_USER.FirstOrDefault(x => x.UserName == userAp1);

                        if (us != null)
                        {
                            lstSendPre.Add(us.Email.NullString());
                        }

                        if (usApLV1 != null)
                        {
                            lstSendPre.Add(usApLV1.Email.NullString());
                        }
                    }
                }
                else if (roleName == CommonConstants.ROLE_HR_TOP)
                {
                    DANG_KY_XE xe = _DangKyXeRepository.FindById(maDangKy);
                    if (xe != null)
                    {
                        string userAp1 = xe.Nguoi_XacNhanLV1.NullString().Split(':')[0].NullString();
                        string userAp2 = xe.Nguoi_XacNhanLV2.NullString().Split(':')[0].NullString();

                        APP_USER us = APP_USER.FirstOrDefault(x => x.UserName == xe.UserCreated);
                        APP_USER usApLV1 = APP_USER.FirstOrDefault(x => x.UserName == userAp1);
                        APP_USER usApLV2 = APP_USER.FirstOrDefault(x => x.UserName == userAp2);

                        if (us != null)
                        {
                            lstSendPre.Add(us.Email.NullString());
                        }

                        if (usApLV1 != null)
                        {
                            lstSendPre.Add(usApLV1.Email.NullString());
                        }

                        if (usApLV2 != null)
                        {
                            lstSendPre.Add(usApLV2.Email.NullString());
                        }
                    }
                }
            }

            result.Add("SEND_NEXT", lstSendNext.Where(x => x.NullString() != "").ToList());
            result.Add("SEND_PRE", lstSendPre.Where(x => x.NullString() != "").ToList());

            return result;
        }

        public List<DANG_KY_XE> GetDangKyXeHistory()
        {
            var lst = _DangKyXeRepository.FindAll(x => x.DIEUXE_DANGKY).ToList();

            foreach (var item in lst)
            {
                item.Lxe_BienSo = "";

                foreach (var dx in item.DIEUXE_DANGKY)
                {
                    item.Lxe_BienSo += _LaiXeRepository.FindById(dx.MaLaiXe).HoTen + ": " + _XeRepository.FindById(dx.MaXe).BienSoXe + "\n";
                }
            }

            return lst;
        }
    }
}
