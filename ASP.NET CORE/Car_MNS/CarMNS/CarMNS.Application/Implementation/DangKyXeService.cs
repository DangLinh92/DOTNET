using CarMNS.Application.Interfaces;
using CarMNS.Application.ViewModels;
using CarMNS.Data.EF;
using CarMNS.Data.EF.Extensions;
using CarMNS.Data.Entities;
using CarMNS.Infrastructure.Interfaces;
using CarMNS.Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
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
        private IRespository<DANG_KY_XE_TAXI, int> _TaxiRepository;
        private IRespository<BOPHAN, string> _BoPhanRepository;
        private IRespository<DIEUXE_DANGKY, int> _DieuXeRepository;
        private IRespository<LAI_XE, int> _LaiXeRepository;
        private IRespository<BOPHAN_DUYET, int> _BoPhanDuyetRepository;
        private IRespository<CAR, int> _XeRepository;

        public DangKyXeService(IRespository<LAI_XE, int> LaiXeRepository, IRespository<CAR, int> XeRepository, IUnitOfWork unitOfWork, IRespository<DANG_KY_XE, int> dangKyXeRepository,
            IRespository<BOPHAN, string> boPhanRepository, IRespository<DIEUXE_DANGKY, int> dieuXeRepository, IHttpContextAccessor httpContextAccessor, IRespository<DANG_KY_XE_TAXI, int> taxiRepository,
            IRespository<BOPHAN_DUYET, int> boPhanDuyetRepository)
        {
            _unitOfWork = unitOfWork;
            _DangKyXeRepository = dangKyXeRepository;
            _BoPhanRepository = boPhanRepository;
            _DieuXeRepository = dieuXeRepository;
            _LaiXeRepository = LaiXeRepository;
            _XeRepository = XeRepository;
            _httpContextAccessor = httpContextAccessor;
            _TaxiRepository = taxiRepository;
            _BoPhanDuyetRepository = boPhanDuyetRepository;
        }

        public DANG_KY_XE AddDangKyXe(DANG_KY_XE dangky, string role)
        {
            try
            {
                if (role == CommonConstants.ROLE_DANGKY)
                {
                    //List<APP_USER> APP_USER = ((EFUnitOfWork)_unitOfWork).DBContext().AppUsers.ToList();
                    //List<APP_ROLE> APP_ROLE = ((EFUnitOfWork)_unitOfWork).DBContext().AppRoles.ToList();
                    //List<APP_USER_ROLE> APP_USER_ROLE = ((EFUnitOfWork)_unitOfWork).DBContext().AppUserRoles.ToList();
                    //List<string> Userbophanduyets = _BoPhanDuyetRepository.FindAll(x => x.BoPhan.Contains(dangky.BoPhan)).Select(x => x.UserId).ToList();
                    //Guid roleDuyetBP = APP_ROLE.FirstOrDefault(x => x.Name == CommonConstants.ROLE_GROUP_LD).Id;
                    //List<Guid> lstUserId = APP_USER_ROLE.FindAll(x => x.RoleId.Equals(roleDuyetBP)).Select(x => x.UserId).ToList();

                    //var lstSendNext = APP_USER.FindAll(x => (x.Department == dangky.BoPhan && lstUserId.Contains(x.Id)) || Userbophanduyets.Contains(x.UserName)).Select(x => x.Email).ToList();

                    //if (lstSendNext.Count == 0)
                    //{
                    //    dangky.XacNhanLV1 = true;
                    //    dangky.Nguoi_XacNhanLV1 = GetUserId() + ":Approve" + ":" + GetUserName();
                    //}
                }
                else
               if (role == CommonConstants.ROLE_GROUP_LD)
                {
                    dangky.XacNhanLV1 = true;
                    dangky.Nguoi_XacNhanLV1 = GetUserId() + ":Approve" + ":" + GetUserName();
                }
                else if (role == CommonConstants.ROLE_HR)
                {
                    dangky.XacNhanLV1 = true;
                    dangky.Nguoi_XacNhanLV1 = GetUserId() + ":Approve" + ":" + GetUserName();

                    dangky.XacNhanLV2 = true;
                    dangky.Nguoi_XacNhanLV2 = GetUserId() + ":Approve" + ":" + GetUserName();

                }
                else if (role == CommonConstants.ROLE_HR_TOP)
                {
                    dangky.XacNhanLV1 = true;
                    dangky.Nguoi_XacNhanLV1 = GetUserId() + ":Approve" + ":" + GetUserName();

                    dangky.XacNhanLV2 = true;
                    dangky.Nguoi_XacNhanLV2 = GetUserId() + ":Approve" + ":" + GetUserName();

                    dangky.XacNhanLV3 = true;
                    dangky.Nguoi_XacNhanLV3 = GetUserId() + ":Approve" + ":" + GetUserName();
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
            var lst = _DangKyXeRepository.FindAll(x => x.XacNhanLV3 == false, x => x.DIEUXE_DANGKY).ToList();
            List<LAI_XE> lstXe = _LaiXeRepository.FindAll(x => x.DIEUXE_DANGKY).ToList();
            List<CAR> Xe = _XeRepository.FindAll(x => x.DIEUXE_DANGKY).ToList();
            List<string> bophanduyets = new List<string>();
            string userId = GetUserId();
            BOPHAN_DUYET bpd = _BoPhanDuyetRepository.FindSingle(x => x.UserId == userId);

            if (bpd != null)
            {
                bophanduyets = bpd.BoPhan.Split(",").ToList();
            }

            foreach (var item in lst)
            {
                item.Lxe_BienSo = "";

                foreach (var dx in item.DIEUXE_DANGKY)
                {
                    item.Lxe_BienSo += lstXe.FirstOrDefault(x => x.Id == dx.MaLaiXe).HoTen + ": " + Xe.FirstOrDefault(x => x.Id == dx.MaXe).BienSoXe + "\n";
                    item.SoXe = Xe.FirstOrDefault(x => x.Id == dx.MaXe).LoaiXe;
                }
            }

            if (role == CommonConstants.ROLE_DANGKY)
            {
                lst = lst.FindAll(x => x.BoPhan == bophan || x.UserCreated == userId || x.UserModified == userId).Where(x => x.XacNhanLV2 == false || (x.XacNhanLV2 == true && x.DateModified.CompareTo(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd HH:mm:ss")) >= 0)).OrderByDescending(x => x.NgaySuDung).ToList();
            }
            else if (role == CommonConstants.ROLE_GROUP_LD)
            {
                lst = lst.FindAll(x => x.BoPhan == bophan || bophanduyets.Contains(x.BoPhan)).Where(x => x.XacNhanLV2 == false || (x.XacNhanLV2 == true && x.DateModified.CompareTo(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd HH:mm:ss")) >= 0)).OrderByDescending(x => x.NgaySuDung).ToList();
            }
            else if (role == CommonConstants.ROLE_HR)
            {
                lst = lst.Where(x => x.XacNhanLV1 == true).OrderByDescending(x => x.NgaySuDung).ToList();
            }
            else if (role == CommonConstants.ROLE_HR_TOP || role == CommonConstants.AppRole.AdminRole)
            {
                lst = lst.Where(x => x.XacNhanLV2 == true).OrderByDescending(x => x.NgaySuDung).ToList();
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
                        dangky.Nguoi_XacNhanLV1 = GetUserId() + ":UnApprove" + ":" + GetUserName();
                    }
                    else if (role == CommonConstants.ROLE_HR)
                    {
                        dangky.XacNhanLV2 = false;
                        dangky.XacNhanLV1 = false;

                        if (dangky.Nguoi_XacNhanLV1.NullString().Contains(":Approve")) // H20201001:Approve:JO DUKRAE
                        {
                            dangky.Nguoi_XacNhanLV1 = dangky.Nguoi_XacNhanLV1.NullString().Replace("Approve", "UnApprove");
                        }
                        else
                        {
                            dangky.Nguoi_XacNhanLV1 = GetUserId() + ":UnApprove" + ":" + GetUserName();
                        }

                        dangky.Nguoi_XacNhanLV2 = GetUserId() + ":UnApprove" + ":" + GetUserName();
                    }
                    else if (role == CommonConstants.ROLE_HR_TOP || role == CommonConstants.AppRole.AdminRole)
                    {
                        dangky.XacNhanLV3 = false;
                        dangky.XacNhanLV2 = false;
                        dangky.XacNhanLV1 = false;

                        if (dangky.Nguoi_XacNhanLV1.NullString().Contains(":Approve")) // H20201001:Approve:JO DUKRAE
                        {
                            dangky.Nguoi_XacNhanLV1 = dangky.Nguoi_XacNhanLV1.NullString().Replace("Approve", "UnApprove");
                        }
                        else
                        {
                            dangky.Nguoi_XacNhanLV1 = GetUserId() + ":UnApprove" + ":" + GetUserName();
                        }

                        if (dangky.Nguoi_XacNhanLV2.NullString().Contains(":Approve")) // H20201001:Approve:JO DUKRAE
                        {
                            dangky.Nguoi_XacNhanLV2 = dangky.Nguoi_XacNhanLV2.NullString().Replace("Approve", "UnApprove");
                        }
                        else
                        {
                            dangky.Nguoi_XacNhanLV2 = GetUserId() + ":UnApprove" + ":" + GetUserName();
                        }

                        dangky.Nguoi_XacNhanLV3 = GetUserId() + ":UnApprove" + ":" + GetUserName();
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
                        dangky.Nguoi_XacNhanLV1 = GetUserId() + ":Approve" + ":" + GetUserName();
                    }
                    else if (role == CommonConstants.ROLE_HR)
                    {
                        if (!dangky.XacNhanLV1)
                        {
                            dangky.XacNhanLV1 = true;
                            dangky.Nguoi_XacNhanLV1 = GetUserId() + ":Approve" + ":" + GetUserName();
                        }

                        dangky.XacNhanLV2 = true;
                        dangky.Nguoi_XacNhanLV2 = GetUserId() + ":Approve" + ":" + GetUserName();
                    }
                    else if (role == CommonConstants.ROLE_HR_TOP || role == CommonConstants.AppRole.AdminRole)
                    {
                        if (!dangky.XacNhanLV1)
                        {
                            dangky.XacNhanLV1 = true;
                            dangky.Nguoi_XacNhanLV1 = GetUserId() + ":Approve" + ":" + GetUserName();
                        }

                        if (!dangky.XacNhanLV2)
                        {
                            dangky.XacNhanLV2 = true;
                            dangky.Nguoi_XacNhanLV2 = GetUserId() + ":Approve" + ":" + GetUserName();
                        }

                        dangky.XacNhanLV3 = true;
                        dangky.Nguoi_XacNhanLV3 = GetUserId() + ":Approve" + ":" + GetUserName();
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

            List<string> Userbophanduyets = _BoPhanDuyetRepository.FindAll(x => x.BoPhan.Contains(bophan)).Select(x => x.UserId).ToList();

            Guid roleId = APP_USER_ROLE.FindAll(x => x.UserId.Equals(userId)).FirstOrDefault().RoleId;
            string roleName = APP_ROLE.FindLast(x => x.Id.Equals(roleId)).Name;

            // Tạo mới
            if (isNew)
            {
                if (roleName == CommonConstants.ROLE_DANGKY)
                {
                    Guid roleDuyetBP = APP_ROLE.FirstOrDefault(x => x.Name == CommonConstants.ROLE_GROUP_LD).Id;
                    List<Guid> lstUserId = APP_USER_ROLE.FindAll(x => x.RoleId.Equals(roleDuyetBP)).Select(x => x.UserId).ToList();

                    lstSendNext = APP_USER.FindAll(x => (x.Department == bophan && lstUserId.Contains(x.Id)) || Userbophanduyets.Contains(x.UserName)).Select(x => x.Email).ToList();

                    if (lstSendNext.Count == 0)
                    {
                        Guid roleDuyetHR = APP_ROLE.FirstOrDefault(x => x.Name == CommonConstants.ROLE_HR).Id;
                        List<Guid> lstUserHR = APP_USER_ROLE.FindAll(x => x.RoleId.Equals(roleDuyetHR)).Select(x => x.UserId).ToList();
                        lstSendNext = APP_USER.FindAll(x => lstUserHR.Contains(x.Id)).Select(x => x.Email).ToList();
                    }
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

        public Dictionary<string, List<string>> GetUserSendMailTaxi(int maDangKy, bool isNew, bool isApprove, bool isUnApprove)
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

            List<string> Userbophanduyets = _BoPhanDuyetRepository.FindAll(x => x.BoPhan.Contains(bophan)).Select(x => x.UserId).ToList();

            Guid roleId = APP_USER_ROLE.FindAll(x => x.UserId.Equals(userId)).FirstOrDefault().RoleId;
            string roleName = APP_ROLE.FindLast(x => x.Id.Equals(roleId)).Name;

            // Tạo mới
            if (isNew)
            {
                if (roleName == CommonConstants.ROLE_DANGKY)
                {
                    Guid roleDuyetBP = APP_ROLE.FirstOrDefault(x => x.Name == CommonConstants.ROLE_GROUP_LD).Id;
                    List<Guid> lstUserId = APP_USER_ROLE.FindAll(x => x.RoleId.Equals(roleDuyetBP)).Select(x => x.UserId).ToList();

                    lstSendNext = APP_USER.FindAll(x => (x.Department == bophan && lstUserId.Contains(x.Id)) || Userbophanduyets.Contains(x.UserName)).Select(x => x.Email).ToList();

                    if (lstSendNext.Count == 0)
                    {
                        Guid roleDuyetHR = APP_ROLE.FirstOrDefault(x => x.Name == CommonConstants.ROLE_HR).Id;
                        List<Guid> lstUserHR = APP_USER_ROLE.FindAll(x => x.RoleId.Equals(roleDuyetHR)).Select(x => x.UserId).ToList();
                        lstSendNext = APP_USER.FindAll(x => lstUserHR.Contains(x.Id)).Select(x => x.Email).ToList();
                    }
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

                    DANG_KY_XE_TAXI xe = _TaxiRepository.FindById(maDangKy);
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

                    DANG_KY_XE_TAXI xe = _TaxiRepository.FindById(maDangKy);
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
                    DANG_KY_XE_TAXI xe = _TaxiRepository.FindById(maDangKy);
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
            var lst = _DangKyXeRepository.FindAll(x => x.XacNhanLV2 == true, x => x.DIEUXE_DANGKY).ToList();
            List<CAR> Xe = _XeRepository.FindAll(x => x.DIEUXE_DANGKY).ToList();
            foreach (var item in lst)
            {
                item.Lxe_BienSo = "";

                foreach (var dx in item.DIEUXE_DANGKY)
                {
                    item.Lxe_BienSo += _LaiXeRepository.FindById(dx.MaLaiXe).HoTen + ": " + _XeRepository.FindById(dx.MaXe).BienSoXe + "\n";
                    item.SoXe = Xe.FirstOrDefault(x => x.Id == dx.MaXe).LoaiXe;
                }
            }

            return lst;
        }

        #region Taxi

        public List<DANG_KY_XE_TAXI> GetDangKyXeTaxiHistory()
        {
            var lst = _TaxiRepository.FindAll(x => x.XacNhanLV2 == true).ToList();
            return lst;
        }

        public List<DANG_KY_XE_TAXI> GetHistoryByUser(string user)
        {
            var lst = _TaxiRepository.FindAll(x => x.MaNV == user && x.XacNhanLV2 == true).OrderByDescending(x => x.NgaySuDung).ToList();
            return lst;
        }

        public List<DANG_KY_XE_TAXI> GetAllDangKyXe_Taxi(string role, string bophan)
        {
            var lst = _TaxiRepository.FindAll(x => x.XacNhanLV3 == false || x.SoTien == null || x.SoTien <= 0).ToList();

            List<string> bophanduyets = new List<string>();
            string userId = GetUserId();
            BOPHAN_DUYET bpd = _BoPhanDuyetRepository.FindSingle(x => x.UserId == userId);

            if (bpd != null)
            {
                bophanduyets = bpd.BoPhan.Split(",").ToList();
            }

            if (role == CommonConstants.ROLE_DANGKY)
            {
                lst = lst.FindAll(x => x.BoPhan == bophan || x.UserCreated == userId || x.UserModified == userId).Where(x => x.XacNhanLV2 == false || (x.XacNhanLV2 == true && x.DateModified.CompareTo(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd HH:mm:ss")) >= 0)).OrderByDescending(x => x.NgaySuDung).ThenBy(x=>x.MaBill).ToList();
            }
            else
            if (role == CommonConstants.ROLE_GROUP_LD)
            {
                lst = lst.FindAll(x => x.BoPhan == bophan || bophanduyets.Contains(x.BoPhan)).Where(x => x.XacNhanLV2 == false || (x.XacNhanLV2 == true && x.DateModified.CompareTo(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd HH:mm:ss")) >= 0)).OrderByDescending(x => x.NgaySuDung).ThenBy(x => x.MaBill).ToList();
            }
            else if (role == CommonConstants.ROLE_HR)
            {
                lst = lst.Where(x => x.XacNhanLV1 == true).OrderByDescending(x => x.NgaySuDung).ThenBy(x => x.MaBill).ToList();
            }
            else if (role == CommonConstants.ROLE_HR_TOP)
            {
                lst = lst.Where(x => x.XacNhanLV2 == true).OrderByDescending(x => x.NgaySuDung).ThenBy(x => x.MaBill).ToList();
            }
            else if (role == CommonConstants.AppRole.AdminRole)
            {
                lst = lst.OrderByDescending(x => x.NgaySuDung).ThenBy(x => x.MaBill).ToList();
            }

            return lst;
        }

        public DANG_KY_XE_TAXI AddDangKyXe_Taxi(DANG_KY_XE_TAXI dangky, string role)
        {
            try
            {
                if (role == CommonConstants.ROLE_DANGKY)
                {
                    //List<APP_USER> APP_USER = ((EFUnitOfWork)_unitOfWork).DBContext().AppUsers.ToList();
                    //List<APP_ROLE> APP_ROLE = ((EFUnitOfWork)_unitOfWork).DBContext().AppRoles.ToList();
                    //List<APP_USER_ROLE> APP_USER_ROLE = ((EFUnitOfWork)_unitOfWork).DBContext().AppUserRoles.ToList();
                    //List<string> Userbophanduyets = _BoPhanDuyetRepository.FindAll(x => x.BoPhan.Contains(dangky.BoPhan)).Select(x => x.UserId).ToList();
                    //Guid roleDuyetBP = APP_ROLE.FirstOrDefault(x => x.Name == CommonConstants.ROLE_GROUP_LD).Id;
                    //List<Guid> lstUserId = APP_USER_ROLE.FindAll(x => x.RoleId.Equals(roleDuyetBP)).Select(x => x.UserId).ToList();

                    //var lstSendNext = APP_USER.FindAll(x => (x.Department == dangky.BoPhan && lstUserId.Contains(x.Id)) || Userbophanduyets.Contains(x.UserName)).Select(x => x.Email).ToList();

                    //if (lstSendNext.Count == 0)
                    //{
                    //    dangky.XacNhanLV1 = true;
                    //    dangky.Nguoi_XacNhanLV1 = GetUserId() + ":Approve" + ":" + GetUserName();
                    //}
                }
                else
                if (role == CommonConstants.ROLE_GROUP_LD)
                {
                    dangky.XacNhanLV1 = true;
                    dangky.Nguoi_XacNhanLV1 = GetUserId() + ":Approve" + ":" + GetUserName();
                }
                else if (role == CommonConstants.ROLE_HR)
                {
                    dangky.XacNhanLV1 = true;
                    dangky.Nguoi_XacNhanLV1 = GetUserId() + ":Approve" + ":" + GetUserName();

                    dangky.XacNhanLV2 = true;
                    dangky.Nguoi_XacNhanLV2 = GetUserId() + ":Approve" + ":" + GetUserName();

                }
                else if (role == CommonConstants.ROLE_HR_TOP)
                {
                    dangky.XacNhanLV1 = true;
                    dangky.Nguoi_XacNhanLV1 = GetUserId() + ":Approve" + ":" + GetUserName();

                    dangky.XacNhanLV2 = true;
                    dangky.Nguoi_XacNhanLV2 = GetUserId() + ":Approve" + ":" + GetUserName();

                    dangky.XacNhanLV3 = true;
                    dangky.Nguoi_XacNhanLV3 = GetUserId() + ":Approve" + ":" + GetUserName();
                }

                dangky.NguoiDangKy = ((EFUnitOfWork)_unitOfWork).DBContext().AppUsers.ToList().FirstOrDefault(x => x.UserName == GetUserId()).FullName;

                _TaxiRepository.Add(dangky);
                Save();
                return dangky;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DANG_KY_XE_TAXI GetDangKyXeTaxiById(int id)
        {
            return _TaxiRepository.FindById(id);
        }

        public DANG_KY_XE_TAXI UpdateDangKyXeTaxi(DANG_KY_XE_TAXI dangky)
        {
            _TaxiRepository.Update(dangky);
            Save();
            return dangky;
        }

        public void DeleteDangKyXeTaxi(int id)
        {
            _TaxiRepository.Remove(id);
            Save();
        }

        public bool UnApproveTaxi(int maDangKy, string role)
        {
            try
            {
                var dangky = _TaxiRepository.FindById(maDangKy);
                if (dangky != null)
                {
                    if (role == CommonConstants.ROLE_GROUP_LD)
                    {
                        dangky.XacNhanLV1 = false;
                        dangky.Nguoi_XacNhanLV1 = GetUserId() + ":UnApprove" + ":" + GetUserName();
                    }
                    else if (role == CommonConstants.ROLE_HR)
                    {
                        dangky.XacNhanLV2 = false;
                        dangky.XacNhanLV1 = false;

                        if (dangky.Nguoi_XacNhanLV1.NullString().Contains(":Approve")) // H20201001:Approve:JO DUKRAE
                        {
                            dangky.Nguoi_XacNhanLV1 = dangky.Nguoi_XacNhanLV1.NullString().Replace("Approve", "UnApprove");
                        }
                        else
                        {
                            dangky.Nguoi_XacNhanLV1 = GetUserId() + ":UnApprove" + ":" + GetUserName();
                        }

                        dangky.Nguoi_XacNhanLV2 = GetUserId() + ":UnApprove" + ":" + GetUserName();
                    }
                    else if (role == CommonConstants.ROLE_HR_TOP || role == CommonConstants.AppRole.AdminRole)
                    {
                        dangky.XacNhanLV2 = false;
                        dangky.XacNhanLV1 = false;
                        dangky.XacNhanLV3 = false;

                        //dangky.Nguoi_XacNhanLV1 = GetUserId() + ":UnApprove" + ":" + GetUserName();
                        //dangky.Nguoi_XacNhanLV2 = GetUserId() + ":UnApprove" + ":" + GetUserName();

                        if (dangky.Nguoi_XacNhanLV1.NullString().Contains(":Approve")) // H20201001:Approve:JO DUKRAE
                        {
                            dangky.Nguoi_XacNhanLV1 = dangky.Nguoi_XacNhanLV1.NullString().Replace("Approve", "UnApprove");
                        }
                        else
                        {
                            dangky.Nguoi_XacNhanLV1 = GetUserId() + ":UnApprove" + ":" + GetUserName();
                        }

                        if (dangky.Nguoi_XacNhanLV2.NullString().Contains(":Approve")) // H20201001:Approve:JO DUKRAE
                        {
                            dangky.Nguoi_XacNhanLV2 = dangky.Nguoi_XacNhanLV2.NullString().Replace("Approve", "UnApprove");
                        }
                        else
                        {
                            dangky.Nguoi_XacNhanLV2 = GetUserId() + ":UnApprove" + ":" + GetUserName();
                        }

                        dangky.Nguoi_XacNhanLV3 = GetUserId() + ":UnApprove" + ":" + GetUserName();
                    }

                    _TaxiRepository.Update(dangky);
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

        public bool ApproveTaxi(int maDangKy, string role)
        {
            try
            {
                var dangky = _TaxiRepository.FindById(maDangKy);
                if (dangky != null)
                {
                    if (role == CommonConstants.ROLE_GROUP_LD)
                    {
                        dangky.XacNhanLV1 = true;
                        dangky.Nguoi_XacNhanLV1 = GetUserId() + ":Approve" + ":" + GetUserName();
                    }
                    else if (role == CommonConstants.ROLE_HR)
                    {
                        if (!dangky.XacNhanLV1)
                        {
                            dangky.XacNhanLV1 = true;
                            dangky.Nguoi_XacNhanLV1 = GetUserId() + ":Approve" + ":" + GetUserName();
                        }

                        dangky.XacNhanLV2 = true;
                        dangky.Nguoi_XacNhanLV2 = GetUserId() + ":Approve" + ":" + GetUserName();
                    }
                    else if (role == CommonConstants.ROLE_HR_TOP || role == CommonConstants.AppRole.AdminRole)
                    {
                        if (!dangky.XacNhanLV1)
                        {
                            dangky.XacNhanLV1 = true;
                            dangky.Nguoi_XacNhanLV1 = GetUserId() + ":Approve" + ":" + GetUserName();
                        }

                        if (!dangky.XacNhanLV2)
                        {
                            dangky.XacNhanLV2 = true;
                            dangky.Nguoi_XacNhanLV2 = GetUserId() + ":Approve" + ":" + GetUserName();
                        }

                        dangky.XacNhanLV3 = true;
                        dangky.Nguoi_XacNhanLV3 = GetUserId() + ":Approve" + ":" + GetUserName();
                    }

                    _TaxiRepository.Update(dangky);
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

        public List<NguoiDungTaxi> GetReportTaxi(string fromDate, string toDate, string boPhan)
        {
            List<DANG_KY_XE_TAXI> lstTaxi = new List<DANG_KY_XE_TAXI>();

            var lstTaxiAll = _TaxiRepository.FindAll(x => x.XacNhanLV1 == true).ToList();
            if (fromDate.NullString() != "" && toDate.NullString() != "")
            {
                foreach (var item in lstTaxiAll.ToList())
                {
                    if (item.NgaySuDung.Value.ToString("yyyy-MM-dd").CompareTo(fromDate) >= 0 && item.NgaySuDung.Value.ToString("yyyy-MM-dd").CompareTo(toDate) <= 0)
                    {
                        lstTaxi.Add(item);
                    }
                }
            }

            if (boPhan.NullString() != "")
            {
                lstTaxi = lstTaxi.FindAll(x => x.BoPhan == boPhan);
            }

            NguoiDungTaxi nguoiDungTaxi = new NguoiDungTaxi();
            List<NguoiDungTaxi> lstNguoiDungTaxi = new List<NguoiDungTaxi>();
            foreach (var us in lstTaxi.ToList())
            {
                if (lstNguoiDungTaxi.Any(x => x.MaNV == us.MaNV))
                {
                    nguoiDungTaxi = lstNguoiDungTaxi.FirstOrDefault(x => x.MaNV == us.MaNV);
                }
                else
                {
                    nguoiDungTaxi = new NguoiDungTaxi();
                    nguoiDungTaxi.SoLanSuDung = 0;
                    nguoiDungTaxi.SoTien = 0;
                }

                nguoiDungTaxi.MaNV = us.MaNV;
                nguoiDungTaxi.TenNV = us.TenNguoiSuDung;
                nguoiDungTaxi.BoPhan = us.BoPhan;

                if (us.SoNguoiSuDung > 0)
                    nguoiDungTaxi.SoTien += (double)((us.SoTien == null ? 0 : us.SoTien) / us.SoNguoiSuDung);
                else
                {
                    nguoiDungTaxi.SoTien += (double)(us.SoTien == null ? 0 : us.SoTien);
                }

                nguoiDungTaxi.SoLanSuDung += 1;

                if (!lstNguoiDungTaxi.Any(x => x.MaNV == us.MaNV))
                {
                    lstNguoiDungTaxi.Add(nguoiDungTaxi);
                }
            }

            return lstNguoiDungTaxi;
        }
        #endregion

        public List<NguoiDungTaxi> GetReportTaxiInYear(string fromDate, string toDate)
        {
            List<DANG_KY_XE_TAXI> lstTaxi = new List<DANG_KY_XE_TAXI>();

          var  lstTaxiAll = _TaxiRepository.FindAll(x => x.XacNhanLV1 == true).ToList();

            if (fromDate.NullString() != "" && toDate.NullString() != "")
            {
                foreach (var item in lstTaxiAll.ToList())
                {
                    if (item.NgaySuDung.Value.ToString("yyyy-MM-dd").CompareTo(fromDate) >= 0 && item.NgaySuDung.Value.ToString("yyyy-MM-dd").CompareTo(toDate) <= 0)
                    {
                        lstTaxi.Add(item);
                    }
                }
            }

            NguoiDungTaxi bpDungTaxi = new NguoiDungTaxi();
            List<NguoiDungTaxi> lstbpDungTaxi = new List<NguoiDungTaxi>();
            foreach (var us in lstTaxi.ToList())
            {
                if (lstbpDungTaxi.Any(x => x.BoPhan == us.BoPhan && x.ThangSuDung.Substring(0,7) == us.NgaySuDung.Value.ToString("yyyy-MM")))
                {
                    bpDungTaxi = lstbpDungTaxi.FirstOrDefault( x=> x.BoPhan == us.BoPhan && x.ThangSuDung.Substring(0, 7) == us.NgaySuDung.Value.ToString("yyyy-MM"));
                }
                else
                {
                    bpDungTaxi = new NguoiDungTaxi();
                    bpDungTaxi.SoTien = 0;
                    bpDungTaxi.ThangSuDung = us.NgaySuDung.Value.ToString("yyyy-MM");
                }
                bpDungTaxi.BoPhan = us.BoPhan;

                if (us.SoNguoiSuDung > 0)
                    bpDungTaxi.SoTien += (double)((us.SoTien == null ? 0 : us.SoTien) / us.SoNguoiSuDung);
                else
                {
                    bpDungTaxi.SoTien += (double)(us.SoTien == null ? 0 : us.SoTien);
                }

                if (!lstbpDungTaxi.Any(x => x.BoPhan == us.BoPhan && x.ThangSuDung.Substring(0, 7) == us.NgaySuDung.Value.ToString("yyyy-MM")))
                {
                    lstbpDungTaxi.Add(bpDungTaxi);
                }
            }

            return lstbpDungTaxi;
        }
        public List<ItemValue> GetListTime()
        {
            List<ItemValue> lst = new List<ItemValue>();
            string mm = "00";
            string mm30 = "30";

            string time = "";
            string time30 = "";

            ItemValue item;
            ItemValue item30;
            for (int i = 0; i <= 23; i++)
            {
                time = "";
                time30 = "";

                if (i <= 9)
                {
                    time = ("0" + i) + ":" + mm;
                    time30 = ("0" + i) + ":" + mm30;

                    item = new ItemValue(time, time + ":00");
                    item30 = new ItemValue(time30, time30 + ":00");
                    lst.Add(item);
                    lst.Add(item30);
                }
                else
                {
                    time = i + ":" + mm;
                    time30 = i + ":" + mm30;

                    item = new ItemValue(time, time + ":00");
                    item30 = new ItemValue(time30, time30 + ":00");
                    lst.Add(item);
                    lst.Add(item30);
                }

            }
            return lst;

        }
    }
}
