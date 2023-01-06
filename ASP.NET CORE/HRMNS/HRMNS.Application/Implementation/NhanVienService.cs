using AutoMapper;
using AutoMapper.QueryableExtensions;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMNS.Data.Enums;
using HRMNS.Data.IRepositories;
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
    public class NhanVienService : BaseService, INhanVienService
    {
        private IRespository<HR_NHANVIEN, string> _nhanvienRepository;
        private IRespository<HR_BO_PHAN_DETAIL, int> _bophanDetailRepository;
        private IRespository<HR_LOAIHOPDONG, int> _loaiHDRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NhanVienService(IRespository<HR_NHANVIEN, string> nhanVienRepository, IRespository<HR_LOAIHOPDONG, int> loaiHDRepository, IRespository<HR_BO_PHAN_DETAIL, int> bophanDetailRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _nhanvienRepository = nhanVienRepository;
            _bophanDetailRepository = bophanDetailRepository;
            _loaiHDRepository = loaiHDRepository;
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

        public List<NhanVienViewModel> GetAll(params Expression<Func<HR_NHANVIEN, object>>[] includeProperties)
        {
            var er = _nhanvienRepository.FindAll(x => x.IsDelete != CommonConstants.IsDelete, includeProperties).OrderByDescending(o => o.DateModified).ToList();
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

        public void ImportExcel(string filePath, string param)
        {
            using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
            {
                if (param == CommonConstants.IMPORT_BASIC_EMP)
                {
                    ImportBasicInfo(packet);
                }
                else if (param == CommonConstants.IMPORT_DETAIL_EMP) // IMPORT CONTRACT
                {
                    ImportDetailInfo(packet);
                }
                else if (param == CommonConstants.IMPORT_CHUCVU2_EMP) // import chuc vu : gian tiep sx, truc tiep sx
                {
                    ImportChucVu2Info(packet);
                }
            }
        }

        private void ImportChucVu2Info(ExcelPackage packet)
        {
            ResultDB resultDB = new ResultDB();
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("MaNV");
                table.Columns.Add("TenNV");
                table.Columns.Add("ChucVu2");
                table.Columns.Add("TrucTiepSX");
                DataRow row = null;

                ExcelWorksheet worksheet = packet.Workbook.Worksheets[1];
                for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                {
                    row = table.NewRow();

                    if (string.IsNullOrEmpty(worksheet.Cells[i, 1].Text.NullString().ToUpper()) || _nhanvienRepository.FindById(worksheet.Cells[i, 1].Text.NullString().ToUpper()) == null)
                    {
                        continue;
                    }

                    row["MaNV"] = worksheet.Cells[i, 1].Text.NullString().ToUpper();
                    row["TenNV"] = worksheet.Cells[i, 2].Text.NullString();
                    row["ChucVu2"] = worksheet.Cells[i, 3].Text.NullString();
                    row["TrucTiepSX"] = worksheet.Cells[i, 4].Text.NullString();

                    table.Rows.Add(row);
                }

                resultDB = _nhanvienRepository.ExecProceduce("PKG_BUSINESS.PUT_NHANVIEN_CHUCVU2", new Dictionary<string, string>(), "A_DATA", table);
            }
            catch (Exception ex)
            {
                resultDB.ReturnInt = -1;
                resultDB.ReturnString = ex.Message;
            }
        }

        // Import basic info of employee
        private void ImportBasicInfo(ExcelPackage packet)
        {
            ExcelWorksheet worksheet = packet.Workbook.Worksheets[1];
            HR_NHANVIEN nhanvien;
            for (int i = worksheet.Dimension.Start.Row + 2; i <= worksheet.Dimension.End.Row; i++)
            {
                nhanvien = new HR_NHANVIEN();
                nhanvien.Id = worksheet.Cells[i, 2].Text.NullString().ToUpper();

                if (string.IsNullOrEmpty(nhanvien.Id) || _nhanvienRepository.FindById(nhanvien.Id) != null)
                {
                    continue;
                }

                nhanvien.MaChucDanh = worksheet.Cells[i, 3].Text.NullString();
                nhanvien.MaBoPhan = worksheet.Cells[i, 4].Text.NullString();
                nhanvien.TenNV = worksheet.Cells[i, 5].Text.NullString();
                nhanvien.GioiTinh = worksheet.Cells[i, 6].Text.NullString();

                if (DateTime.TryParse(worksheet.Cells[i, 7].Text.NullString(), out var ngaysinh))
                {
                    nhanvien.NgaySinh = ngaysinh.ToString("yyyy-MM-dd");
                }
                else
                {
                    nhanvien.NgaySinh = "";
                }

                nhanvien.NoiSinh = worksheet.Cells[i, 8].Text.NullString();
                nhanvien.TinhTrangHonNhan = worksheet.Cells[i, 9].Text.NullString();
                nhanvien.DanToc = worksheet.Cells[i, 10].Text.NullString();
                nhanvien.DiaChiThuongTru = worksheet.Cells[i, 11].Text.NullString();
                nhanvien.Email = worksheet.Cells[i, 12].Text.NullString();
                nhanvien.SoDienThoai = worksheet.Cells[i, 13].Text.NullString();
                nhanvien.SoDienThoaiNguoiThan = worksheet.Cells[i, 14].Text.NullString();
                nhanvien.QuanHeNguoiThan = worksheet.Cells[i, 15].Text.NullString();
                nhanvien.CMTND = worksheet.Cells[i, 16].Text.NullString();

                if (DateTime.TryParse(worksheet.Cells[i, 17].Text.NullString(), out var ngayCapCMTND))
                {
                    nhanvien.NgayCapCMTND = ngayCapCMTND.ToString("yyyy-MM-dd");
                }
                else
                {
                    nhanvien.NgayCapCMTND = "";
                }

                nhanvien.NoiCapCMTND = worksheet.Cells[i, 18].Text.NullString();
                nhanvien.TenNganHang = worksheet.Cells[i, 19].Text.NullString();
                nhanvien.SoTaiKhoanNH = worksheet.Cells[i, 20].Text.NullString();
                nhanvien.TruongDaoTao = worksheet.Cells[i, 21].Text.NullString();

                if (DateTime.TryParse(worksheet.Cells[i, 22].Text.NullString(), out var ngayVao))
                {
                    nhanvien.NgayVao = ngayVao.ToString("yyyy-MM-dd");
                }
                else
                {
                    nhanvien.NgayVao = "";
                }

                nhanvien.NguyenQuan = worksheet.Cells[i, 23].Text.NullString();
                nhanvien.DChiHienTai = worksheet.Cells[i, 24].Text.NullString();
                nhanvien.KyLuatLD = worksheet.Cells[i, 25].Text.NullString();
                nhanvien.Note = worksheet.Cells[i, 26].Text.NullString();
                nhanvien.MaBHXH = worksheet.Cells[i, 27].Text.NullString();

                DateTime.TryParse(worksheet.Cells[i, 28].Text.NullString(), out var ngayThamGiaBH);
                DateTime.TryParse(worksheet.Cells[i, 29].Text.NullString(), out var ngayKetThucBH);

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

                nhanvien.MaSoThue = worksheet.Cells[i, 30].Text.NullString();
                int.TryParse(worksheet.Cells[i, 31].Text.NullString(), out var songuoigiamtru);
                nhanvien.SoNguoiGiamTru = songuoigiamtru;
                nhanvien.TonGiao = worksheet.Cells[i, 32].Text.NullString();

                float totalPhepNam = 12;
                float remainPhepNam = 0;
                float.TryParse(worksheet.Cells[i, 33].Text.NullString(), out totalPhepNam);
                float.TryParse(worksheet.Cells[i, 34].Text.NullString(), out remainPhepNam);

                nhanvien.HR_PHEP_NAM.Add(new HR_PHEP_NAM()
                {
                    MaNhanVien = nhanvien.Id,
                    SoPhepNam = totalPhepNam,
                    SoPhepConLai = remainPhepNam,
                    Year = DateTime.Now.Year,
                    DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    UserCreated = GetUserId()
                });

                nhanvien.MaBoPhanChiTiet = GetBoPhanChiTiet(worksheet.Cells[i, 35].Text.NullString());
                nhanvien.NoiTuyenDung = worksheet.Cells[i, 37].Text.NullString();

                if (worksheet.Cells[i, 36].Text.NullString() != "" && DateTime.TryParse(worksheet.Cells[i, 36].Text.NullString(), out DateTime ngaynghiviec))
                {
                    nhanvien.NgayNghiViec = ngaynghiviec.ToString("yyyy-MM-dd");
                    nhanvien.Status = Status.InActive.NullString();
                }
                else
                {
                    nhanvien.Status = Status.Active.NullString();
                }

                nhanvien.IsDelete = "N";
                nhanvien.UserCreated = GetUserId();
                nhanvien.UserModified = GetUserId();

                _nhanvienRepository.Add(nhanvien);
            }
        }

        // Import detail of employee
        private void ImportDetailInfo(ExcelPackage packet)
        {
            // Update hop dong info of employee
            ExcelWorksheet worksheet = packet.Workbook.Worksheets[1];
            List<HR_NHANVIEN> lstNhanVien = new List<HR_NHANVIEN>();
            HR_NHANVIEN nhanvien;
            string maNV = "";
            for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
            {
                maNV = worksheet.Cells[i, 1].Text.NullString().ToUpper();
                nhanvien = lstNhanVien.FirstOrDefault(x => x.Id == maNV);

                if (nhanvien == null)
                {
                    nhanvien = _nhanvienRepository.FindById(maNV);
                }

                if (nhanvien == null || string.IsNullOrEmpty(maNV))
                {
                    continue;
                }

                int? loaiHd = 0;
                string strLoaiHD = worksheet.Cells[i, 4].Text.NullString();
                loaiHd = _loaiHDRepository.FindAll(x => x.TenLoaiHD == strLoaiHD).FirstOrDefault().Id;

                //switch (strLoaiHD)
                //{
                //    case "Hợp Đồng Thử Việc":
                //        loaiHd = 14;
                //        break;
                //    case "Hợp Đồng 1 năm lần 1":
                //        loaiHd = 15;
                //        break;
                //    case "Hợp Đồng 1 năm lần 2":
                //        loaiHd = 13;
                //        break;
                //    case "Hợp Đồng Không Thời Hạn":
                //        loaiHd = 16;
                //        break;
                //    default:
                //        loaiHd = 15;
                //        break;
                //}

                string ngayKetThuc = worksheet.Cells[i, 7].Text.NullString().ToYYYYMMDD();
                if (loaiHd == 5 || strLoaiHD == "Hợp Đồng Không Thời Hạn")
                {
                    ngayKetThuc = DateTime.Parse(ngayKetThuc).AddYears(30).ToString("yyyy-MM-dd");
                }

                Random rd = new Random();
                HR_HOPDONG hopdong = new HR_HOPDONG()
                {
                    MaNV = maNV,
                    MaHD = maNV + "/" + DateTime.Now.Year + "/" + loaiHd + "_" + rd.Next(1000), //worksheet.Cells[i, 2].Value.NullString(),
                    TenHD = worksheet.Cells[i, 3].Text.NullString(),
                    LoaiHD = loaiHd,
                    NgayKy = worksheet.Cells[i, 5].Text.NullString().ToYYYYMMDD(),
                    NgayHieuLuc = worksheet.Cells[i, 6].Text.NullString().ToYYYYMMDD(),
                    NgayHetHieuLuc = ngayKetThuc,
                    NgayTao = worksheet.Cells[i, 5].Text.NullString().ToYYYYMMDD(),
                    DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    UserCreated = GetUserId()
                };

                nhanvien.HR_HOPDONG.Add(hopdong);
                nhanvien.UserModified = GetUserId();
                nhanvien.DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (!lstNhanVien.Any(x => x.Id == maNV))
                {
                    lstNhanVien.Add(nhanvien);
                }
            }

            // update qua trinh cong tac
            ExcelWorksheet worksheet2 = packet.Workbook.Worksheets[2];
            for (int i = worksheet2.Dimension.Start.Row + 1; i <= worksheet2.Dimension.End.Row; i++)
            {
                maNV = worksheet2.Cells[i, 1].Text.NullString();
                nhanvien = lstNhanVien.FirstOrDefault(x => x.Id == maNV);

                if (nhanvien == null)
                {
                    nhanvien = _nhanvienRepository.FindById(maNV);
                }

                if (nhanvien == null || string.IsNullOrEmpty(maNV))
                {
                    continue;
                }

                HR_QUATRINHLAMVIEC quatrinhCtac = new HR_QUATRINHLAMVIEC()
                {
                    MaNV = maNV,
                    TieuDe = worksheet2.Cells[i, 2].Text.NullString(),
                    ThơiGianBatDau = worksheet2.Cells[i, 3].Text.NullString().ToYYYYMMDD(),
                    ThoiGianKetThuc = worksheet2.Cells[i, 4].Text.NullString().ToYYYYMMDD(),
                    Note = worksheet2.Cells[i, 5].Text.NullString(),
                    DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    UserCreated = GetUserId()
                };

                nhanvien.HR_QUATRINHLAMVIEC.Add(quatrinhCtac);
                nhanvien.UserModified = GetUserId();
                nhanvien.DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (!lstNhanVien.Any(x => x.Id == maNV))
                {
                    lstNhanVien.Add(nhanvien);
                }
            }

            // update tinh trang hso
            ExcelWorksheet worksheet3 = packet.Workbook.Worksheets[3];
            for (int i = worksheet3.Dimension.Start.Row + 1; i <= worksheet3.Dimension.End.Row; i++)
            {
                maNV = worksheet3.Cells[i, 1].Text.NullString();
                nhanvien = lstNhanVien.FirstOrDefault(x => x.Id == maNV);

                if (nhanvien == null)
                {
                    nhanvien = _nhanvienRepository.FindById(maNV);
                }

                if (nhanvien == null || string.IsNullOrEmpty(maNV))
                {
                    continue;
                }

                HR_TINHTRANGHOSO tinhtranghs = new HR_TINHTRANGHOSO()
                {
                    MaNV = maNV,
                    SoYeuLyLich = worksheet3.Cells[i, 2].Text.NullString().Contains("X"),
                    CMTND = worksheet3.Cells[i, 3].Text.NullString().Contains("X"),
                    SoHoKhau = worksheet3.Cells[i, 4].Text.NullString().Contains("X"),
                    GiayKhaiSinh = worksheet3.Cells[i, 5].Text.NullString().Contains("X"),
                    BangTotNghiep = worksheet3.Cells[i, 6].Text.NullString().Contains("X"),
                    XacNhanDanSu = worksheet3.Cells[i, 7].Text.NullString().Contains("X"),
                    AnhThe = worksheet3.Cells[i, 8].Text.NullString().Contains("X"),
                    DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    UserCreated = GetUserId()
                };

                if (tinhtranghs.SoYeuLyLich &&
                    tinhtranghs.CMTND &&
                    tinhtranghs.SoHoKhau &&
                    tinhtranghs.GiayKhaiSinh &&
                    tinhtranghs.BangTotNghiep &&
                    tinhtranghs.XacNhanDanSu &&
                    tinhtranghs.AnhThe)
                {
                    tinhtranghs.Status = CommonConstants.Y;
                }
                else
                {
                    tinhtranghs.Status = CommonConstants.N;
                }

                nhanvien.HR_TINHTRANGHOSO.Add(tinhtranghs);
                nhanvien.UserModified = GetUserId();
                nhanvien.DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (!lstNhanVien.Any(x => x.Id == maNV))
                {
                    lstNhanVien.Add(nhanvien);
                }
            }

            // save all changed
            foreach (var item in lstNhanVien)
            {
                _nhanvienRepository.Update(item);
            }
        }

        private int? GetBoPhanChiTiet(string tenbp)
        {
            return _bophanDetailRepository.FindAll(x => x.TenBoPhanChiTiet == tenbp).FirstOrDefault()?.Id;
        }
    }
}
