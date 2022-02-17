using AutoMapper;
using AutoMapper.QueryableExtensions;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using HRMNS.Data.EF.Extensions;
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
using System.Linq.Expressions;
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

            if (string.IsNullOrEmpty(nhanvien.Status))
            {
                nhanvien.Status = Status.Active.ToString();
            }

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
            var er = _nhanvienRepository.FindAll(x => x.IsDelete != CommonConstants.IsDelete).OrderByDescending(o => o.DateModified).ToList();
            return (List<NhanVienViewModel>)_mapper.Map(er, typeof(List<HR_NHANVIEN>), typeof(List<NhanVienViewModel>));
            //return _mapper.ProjectTo<NhanVienViewModel>(null).ToList();
        }

        public List<NhanVienViewModel> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _mapper.Map<List<NhanVienViewModel>>(_nhanvienRepository.FindAll(x => x.TenNV.Contains(keyword) || x.Id.Contains(keyword)));
            else
                return _mapper.Map<List<NhanVienViewModel>>(_nhanvienRepository.FindAll());
        }

        //public NhanVienViewModel GetById(string id)
        //{
        //    return _mapper.Map<HR_NHANVIEN, NhanVienViewModel>(_nhanvienRepository.FindById(id));
        //}

        public NhanVienViewModel GetById(string id, params Expression<Func<HR_NHANVIEN, object>>[] includeProperties)
        {
            return _mapper.Map<HR_NHANVIEN, NhanVienViewModel>(_nhanvienRepository.FindById(id, includeProperties));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(NhanVienViewModel nhanVienVm)
        {
            var nhanvien = _mapper.Map<NhanVienViewModel, HR_NHANVIEN>(nhanVienVm);
            nhanvien.UserModified = GetUserId();
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
                lstNV = _mapper.Map<List<NhanVienViewModel>>(_nhanvienRepository.FindAll(x => x.TenNV.Contains(name)));
            }
            else
            if (!string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(name))
            {
                lstNV = _mapper.Map<List<NhanVienViewModel>>(_nhanvienRepository.FindAll(x => x.TenNV.Contains(name) && x.MaBoPhan.Contains(dept)));
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
                for (int i = worksheet.Dimension.Start.Row + 2; i <= worksheet.Dimension.End.Row; i++)
                {
                    nhanvien = new HR_NHANVIEN();
                    nhanvien.Id = worksheet.Cells[i, 2].Value.NullString();

                    if (string.IsNullOrEmpty(nhanvien.Id))
                    {
                        continue;
                    }

                    nhanvien.MaChucDanh = worksheet.Cells[i, 3].Value.NullString();
                    nhanvien.MaBoPhan = worksheet.Cells[i, 4].Value.NullString();
                    nhanvien.TenNV = worksheet.Cells[i, 5].Value.NullString();
                    nhanvien.GioiTinh = worksheet.Cells[i, 6].Value.NullString();

                    if (DateTime.TryParse(worksheet.Cells[i, 7].Value.NullString(), out var ngaysinh))
                    {
                        nhanvien.NgaySinh = ngaysinh.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        nhanvien.NgaySinh = "";
                    }

                    nhanvien.NoiSinh = worksheet.Cells[i, 8].Value.NullString();
                    nhanvien.TinhTrangHonNhan = worksheet.Cells[i, 9].Value.NullString();
                    nhanvien.DanToc = worksheet.Cells[i, 10].Value.NullString();
                    nhanvien.DiaChiThuongTru = worksheet.Cells[i, 11].Value.NullString();
                    nhanvien.Email = worksheet.Cells[i, 12].Value.NullString();
                    nhanvien.SoDienThoai = worksheet.Cells[i, 13].Value.NullString();
                    nhanvien.SoDienThoaiNguoiThan = worksheet.Cells[i, 14].Value.NullString();
                    nhanvien.QuanHeNguoiThan = worksheet.Cells[i, 15].Value.NullString();
                    nhanvien.CMTND = worksheet.Cells[i, 16].Value.NullString();

                    if (DateTime.TryParse(worksheet.Cells[i, 17].Value.NullString(), out var ngayCapCMTND))
                    {
                        nhanvien.NgayCapCMTND = ngayCapCMTND.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        nhanvien.NgayCapCMTND = "";
                    }

                    nhanvien.NoiCapCMTND = worksheet.Cells[i, 18].Value.NullString();
                    nhanvien.TenNganHang = worksheet.Cells[i, 19].Value.NullString();
                    nhanvien.SoTaiKhoanNH = worksheet.Cells[i, 20].Value.NullString();
                    nhanvien.TruongDaoTao = worksheet.Cells[i, 21].Value.NullString();

                    if (DateTime.TryParse(worksheet.Cells[i, 22].Value.NullString(), out var ngayVao))
                    {
                        nhanvien.NgayVao = ngayVao.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        nhanvien.NgayVao = "";
                    }

                    nhanvien.NguyenQuan = worksheet.Cells[i, 23].Value.NullString();
                    nhanvien.DChiHienTai = worksheet.Cells[i, 24].Value.NullString();
                    nhanvien.KyLuatLD = worksheet.Cells[i, 25].Value.NullString();
                    nhanvien.Note = worksheet.Cells[i, 26].Value.NullString();
                    nhanvien.MaBHXH = worksheet.Cells[i, 27].Value.NullString();

                    DateTime.TryParse(worksheet.Cells[i, 28].Value.NullString(), out var ngayThamGiaBH);
                    DateTime.TryParse(worksheet.Cells[i, 29].Value.NullString(), out var ngayKetThucBH);

                    if (ngayThamGiaBH == null)
                    {
                        ngayThamGiaBH = DateTime.Now;
                    }

                    if (ngayKetThucBH == null)
                    {
                        ngayKetThucBH = DateTime.Now;
                    }

                    if (!string.IsNullOrEmpty(nhanvien.MaBHXH))
                    {
                        nhanvien.HR_BHXH.Add(new HR_BHXH()
                        {
                            Id = nhanvien.MaBHXH,
                            MaNV = nhanvien.Id,
                            NgayThamGia = ngayThamGiaBH.ToString("yyyy-MM-dd"),
                            NgayKetThuc = ngayKetThucBH.ToString("yyyy-MM-dd"),
                            DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            UserCreated = GetUserId()
                        });
                    }

                    nhanvien.MaSoThue = worksheet.Cells[i, 30].Value.NullString();
                    int.TryParse(worksheet.Cells[i, 31].Value.NullString(), out var songuoigiamtru);
                    nhanvien.SoNguoiGiamTru = songuoigiamtru;
                    nhanvien.TonGiao = worksheet.Cells[i, 32].Value.NullString();

                    float totalPhepNam = 12;
                    float remainPhepNam = 0;
                    float.TryParse(worksheet.Cells[i, 33].Value.NullString(), out totalPhepNam);
                    float.TryParse(worksheet.Cells[i, 34].Value.NullString(), out remainPhepNam);

                    nhanvien.HR_PHEP_NAM.Add(new HR_PHEP_NAM()
                    {
                        MaNhanVien = nhanvien.Id,
                        SoPhepNam = totalPhepNam,
                        SoPhepConLai = remainPhepNam,
                        DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        UserCreated = GetUserId()
                    });

                    nhanvien.Status = Status.Active.NullString();
                    nhanvien.IsDelete = "N";

                    nhanvien.UserCreated = GetUserId();
                    nhanvien.UserModified = GetUserId();

                    _nhanvienRepository.Add(nhanvien);
                }
            }
        }
    }
}
