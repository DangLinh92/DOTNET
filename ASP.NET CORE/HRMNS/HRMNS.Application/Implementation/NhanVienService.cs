using AutoMapper;
using AutoMapper.QueryableExtensions;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using HRMNS.Data.Entities;
using HRMNS.Data.Enums;
using HRMNS.Data.IRepositories;
using HRMNS.Utilities.Constants;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class NhanVienService : BaseService, INhanVienService
    {
        private IRespository<HR_NHANVIEN, string> _nhanvienRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NhanVienService(IRespository<HR_NHANVIEN, string> nhanVienRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _nhanvienRepository = nhanVienRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public NhanVienViewModel Add(NhanVienViewModel nhanVienVm)
        {
            var nhanvien = _mapper.Map<NhanVienViewModel, HR_NHANVIEN>(nhanVienVm);
            nhanvien.Status = Status.Active.ToString();
            nhanvien.DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            nhanvien.DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            nhanvien.UserCreated = GetUserId();
            nhanvien.IsDelete = "N";
            _nhanvienRepository.Add(nhanvien);
            return nhanVienVm;
        }

        public void Delete(string id)
        {
            _nhanvienRepository.Remove(id);
        }

        public List<NhanVienViewModel> GetAll()
        {
            return _mapper.ProjectTo<NhanVienViewModel>(_nhanvienRepository.FindAll(x => x.IsDelete != CommonConstants.IsDelete)).OrderByDescending(x => x.DateModified).ToList();
        }

        public List<NhanVienViewModel> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _mapper.ProjectTo<NhanVienViewModel>(_nhanvienRepository.FindAll(x => x.TenNV.Contains(keyword) || x.Id.Contains(keyword))).ToList();
            else
                return _mapper.ProjectTo<NhanVienViewModel>(_nhanvienRepository.FindAll()).ToList();
        }

        public NhanVienViewModel GetById(string id)
        {
            return _mapper.Map<HR_NHANVIEN, NhanVienViewModel>(_nhanvienRepository.FindById(id));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(NhanVienViewModel nhanVienVm)
        {
            var nhanvien = _mapper.Map<NhanVienViewModel, HR_NHANVIEN>(nhanVienVm);
            _nhanvienRepository.Update(nhanvien);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<NhanVienViewModel> Search(string id, string name, string dept)
        {
            if (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(name) && string.IsNullOrEmpty(dept))
            {
                return GetAll();
            }

            List<NhanVienViewModel> lstNV = new List<NhanVienViewModel>();

            if (!string.IsNullOrEmpty(id))
            {
                NhanVienViewModel nv = _mapper.Map<HR_NHANVIEN, NhanVienViewModel>(_nhanvienRepository.FindById(id));
                if (nv != null)
                {
                    lstNV.Add(nv);
                    return lstNV;
                }
            }

            if (!string.IsNullOrEmpty(name))
            {
                lstNV = _mapper.ProjectTo<NhanVienViewModel>(_nhanvienRepository.FindAll(x => x.TenNV.Contains(name))).ToList();
            }
            else
            if (!string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(name))
            {
                lstNV = _mapper.ProjectTo<NhanVienViewModel>(_nhanvienRepository.FindAll(x => x.TenNV.Contains(name) && x.MaBoPhan.Contains(dept))).ToList();
            }

            return lstNV;
        }

        public void UpdateSingle(NhanVienViewModel nhanVienVm)
        {
            nhanVienVm.UserModified = GetUserId();
            HR_NHANVIEN nHANVIEN = ((Data.EF.EFUnitOfWork)_unitOfWork).DBContext().HrNhanVien.First(x => x.Id == nhanVienVm.Id);
            var nhanvien = _mapper.Map(nhanVienVm, nHANVIEN);
        }

        public void ImportExcel(string filePath)
        {
            using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = packet.Workbook.Worksheets[1];
                HR_NHANVIEN nhanvien; 
                for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                {
                    nhanvien = new HR_NHANVIEN();
                    nhanvien.Id = worksheet.Cells[i, 2].Value.ToString();
                    nhanvien.MaChucDanh = worksheet.Cells[i, 3].Value.ToString();
                    nhanvien.MaBoPhan = worksheet.Cells[i, 4].Value.ToString();
                    nhanvien.TenNV = worksheet.Cells[i, 5].Value.ToString();
                    nhanvien.GioiTinh = worksheet.Cells[i, 6].Value.ToString();
                    nhanvien.NgaySinh = worksheet.Cells[i, 7].Value.ToString();
                    nhanvien.NoiSinh = worksheet.Cells[i, 8].Value.ToString();
                    nhanvien.TinhTrangHonNhan = worksheet.Cells[i, 9].Value.ToString();
                    nhanvien.DanToc = worksheet.Cells[i, 10].Value.ToString();
                    nhanvien.DiaChiThuongTru = worksheet.Cells[i, 11].Value.ToString();
                    nhanvien.Email = worksheet.Cells[i, 12].Value.ToString();
                    nhanvien.SoDienThoai = worksheet.Cells[i, 13].Value.ToString();
                    nhanvien.SoDienThoaiNguoiThan = worksheet.Cells[i, 14].Value.ToString();
                    nhanvien.QuanHeNguoiThan = worksheet.Cells[i, 15].Value.ToString();
                    nhanvien.CMTND= worksheet.Cells[i, 16].Value.ToString();
                    nhanvien.NgayCapCMTND = worksheet.Cells[i, 17].Value.ToString();
                    nhanvien.NoiCapCMTND = worksheet.Cells[i, 18].Value.ToString();
                    nhanvien.TenNganHang = worksheet.Cells[i, 19].Value.ToString();
                    nhanvien.SoTaiKhoanNH = worksheet.Cells[i, 20].Value.ToString();
                    nhanvien.TruongDaoTao = worksheet.Cells[i, 21].Value.ToString();
                    nhanvien.NgayVao = worksheet.Cells[i, 22].Value.ToString();
                    nhanvien.NguyenQuan = worksheet.Cells[i, 23].Value.ToString();
                    nhanvien.DChiHienTai = worksheet.Cells[i, 24].Value.ToString();
                    nhanvien.KyLuatLD = worksheet.Cells[i, 25].Value.ToString();
                    nhanvien.Note = worksheet.Cells[i, 26].Value.ToString();
                    nhanvien.MaBHXH= worksheet.Cells[i, 27].Value.ToString();
                    nhanvien.MaSoThue = worksheet.Cells[i, 28].Value.ToString();
                    nhanvien.SoNguoiGiamTru = worksheet.Cells[i, 29].Value.ToString() == "" ? 0 : int.Parse(worksheet.Cells[i, 29].Value.ToString());
                    nhanvien.Status = Status.Active.ToString();
                    nhanvien.IsDelete = "N";
                    nhanvien.UserCreated = GetUserId();
                    nhanvien.UserModified = GetUserId();

                    _nhanvienRepository.Add(nhanvien);
                }
            }
        }
    }
}
