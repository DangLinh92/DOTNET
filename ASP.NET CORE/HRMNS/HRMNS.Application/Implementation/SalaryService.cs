using HRMNS.Application.Interfaces;
using HRMNS.Data.EF;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMNS.Data.Entities.Payroll;
using HRMNS.Data.Enums;
using HRMNS.Utilities.Dtos;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class SalaryService : BaseService, ISalaryService
    {
        private IRespository<HR_SALARY, int> _salaryRepository;
        private IPayrollRespository<HR_SALARY_PR, int> _salarySQLiteRepository;
        private IRespository<HR_SALARY_PHATSINH, int> _salaryPhatSinhRepository;
        private IPayrollRespository<HR_SALARY_PHATSINH_PR, int> _salaryPhatSinhSQLiteRepository;
        private IRespository<HR_SALARY_DANHMUC_PHATSINH, int> _salaryDanhMucPhatSinhRepository;
        private IRespository<HR_NHANVIEN, string> _nhanvienRespository;
        private IUnitOfWork _unitOfWork;
        private IPayrollUnitOfWork _payrollUnitOfWork;
        private IRespository<HR_CHUCDANH, string> _chucDanhRepository;
        private IRespository<PHUCAP_DOC_HAI, int> _phucapdochaiRepository;

        public SalaryService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork,
            IRespository<HR_SALARY, int> salaryRepository, IRespository<HR_SALARY_PHATSINH, int> salaryPhatSinhRepository,
            IRespository<HR_SALARY_DANHMUC_PHATSINH, int> salaryDanhMucPhatSinhRepository,
            IRespository<HR_NHANVIEN, string> nhanvienRespository,
            IRespository<HR_CHUCDANH, string> chucDanhRepository,
            IRespository<PHUCAP_DOC_HAI, int> phucapdochaiRepository,
            IPayrollRespository<HR_SALARY_PR, int> salarySQLiteRepository,
            IPayrollRespository<HR_SALARY_PHATSINH_PR, int> salaryPhatSinhSQLiteRepository,
            IPayrollUnitOfWork payrollUnitOfWork)
        {
            _payrollUnitOfWork = payrollUnitOfWork;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _salaryRepository = salaryRepository;
            _salaryPhatSinhRepository = salaryPhatSinhRepository;
            _salaryDanhMucPhatSinhRepository = salaryDanhMucPhatSinhRepository;
            _nhanvienRespository = nhanvienRespository;
            _chucDanhRepository = chucDanhRepository;
            _phucapdochaiRepository = phucapdochaiRepository;
            _salarySQLiteRepository = salarySQLiteRepository;
            _salaryPhatSinhSQLiteRepository = salaryPhatSinhSQLiteRepository;
        }

        public HR_SALARY AddSalary(HR_SALARY salary)
        {
            //salary.UserCreated = GetUserId();

            //HR_SALARY_PR sl = new HR_SALARY_PR();
            //sl.CopyPropertiesFrom(salary, new List<string>() { "Id" });
            //_salarySQLiteRepository.Add(sl);
            //_payrollUnitOfWork.Commit();

            //salary.BasicSalary = 0;
            //salary.LivingAllowance = 0;
            //salary.AbilityAllowance = 0;
            //_salaryRepository.Add(salary);

            return salary;
        }

        public void DeleteSalary(int id)
        {
            using var transaction = ((EFUnitOfWork)_unitOfWork).DBContext().Database.BeginTransaction();
            try
            {
                string manv = _salaryRepository.FindById(id).MaNV;
                var en = _salarySQLiteRepository.FindSingle(x => x.MaNV == manv);

                _salaryRepository.Remove(id);
                Save();

                _salarySQLiteRepository.Remove(en);
                _payrollUnitOfWork.Commit();

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<HR_SALARY> GetAllSalary()
        {
            List<HR_SALARY> lst = _salaryRepository.FindAll(x => x.HR_NHANVIEN, y => y.HR_NHANVIEN.HR_BO_PHAN_DETAIL, z => z.HR_NHANVIEN.HR_CHUCDANH)
                .Where(x => x.HR_NHANVIEN.MaBoPhan != "KOREA").ToList();

            List<HR_SALARY_PR> lstPR = _salarySQLiteRepository.FindAll().ToList();
            HR_SALARY_PR en;
            foreach (var item in lst.ToList())
            {
                en = lstPR.FirstOrDefault(x => x.MaNV == item.MaNV);

                if(en != null)
                {
                    item.BasicSalary = en.BasicSalary;
                    item.LivingAllowance = en.LivingAllowance;
                    item.AbilityAllowance = en.AbilityAllowance;
                }

                item.PositionAllowance = (decimal)_chucDanhRepository.FindById(item.HR_NHANVIEN.MaChucDanh).PhuCap;
                item.HarmfulAllowance = (decimal)_phucapdochaiRepository.FindSingle(x => x.BoPhan == item.HR_NHANVIEN.MaBoPhan).PhuCap * item.BasicSalary;
                if (item.HR_NHANVIEN.Status == Status.InActive.ToString() && DateTime.Now.Subtract(DateTime.Parse(item.HR_NHANVIEN.NgayNghiViec)).TotalDays > 60)
                {
                    lst.Remove(item);
                }
            }

            return lst;
        }
        public HR_SALARY GetById(int id)
        {
            return _salaryRepository.FindById(id);
        }

        public HR_SALARY GetByMaNV(string manv)
        {
            return _salaryRepository.FindSingle(x => x.MaNV == manv, y => y.HR_NHANVIEN);
        }

        public ResultDB ImportExcel(string filePath)
        {
            using var transaction = ((EFUnitOfWork)_unitOfWork).DBContext().Database.BeginTransaction();
            ResultDB resultDB = new ResultDB();
            try
            {
                using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = packet.Workbook.Worksheets[1];
                    HR_SALARY salary;
                    List<HR_SALARY> lstUpdate = new List<HR_SALARY>();
                    List<HR_SALARY_PR> lstAddPr = new List<HR_SALARY_PR>();
                    List<HR_SALARY_PR> lstUpdatePr = new List<HR_SALARY_PR>();
                    string manv;
                    HR_SALARY_PR sl;
                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                    {
                        manv = worksheet.Cells[i, 1].Text.NullString();
                        if (manv.NullString() == "")
                        {
                            break;
                        }

                        salary = GetByMaNV(manv);
                        sl = _salarySQLiteRepository.FindSingle(x => x.MaNV == manv);

                        if (salary == null)
                        {
                            continue;
                        }

                        if (sl == null)
                        {
                            sl = new HR_SALARY_PR()
                            {
                                MaNV = manv
                            };
                            lstAddPr.Add(sl);
                        }
                        else
                        {
                            lstUpdatePr.Add(sl);
                        }

                        // --- thong tin luong bao mat k luu tai sql server 97
                        if (worksheet.Cells[i, 3].Text.NullString() != "")
                        {
                            sl.BasicSalary = decimal.Parse(worksheet.Cells[i, 3].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 4].Text.NullString() != "")
                        {
                            sl.LivingAllowance = decimal.Parse(worksheet.Cells[i, 4].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 5].Text.NullString() != "")
                        {
                            sl.AbilityAllowance = decimal.Parse(worksheet.Cells[i, 5].Text.IfNullIsZero());
                        }
                        //----

                        if (worksheet.Cells[i, 6].Text.NullString() != "")
                        {
                            salary.IncentiveStandard = decimal.Parse(worksheet.Cells[i, 6].Text.IfNullIsZero());
                            sl.IncentiveStandard = decimal.Parse(worksheet.Cells[i, 6].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 7].Text.NullString() != "")
                        {
                            salary.IncentiveLanguage = decimal.Parse(worksheet.Cells[i, 7].Text.IfNullIsZero());
                            sl.IncentiveLanguage = decimal.Parse(worksheet.Cells[i, 7].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 8].Text.NullString() != "")
                        {
                            salary.IncentiveTechnical = decimal.Parse(worksheet.Cells[i, 8].Text.IfNullIsZero());
                            sl.IncentiveTechnical = decimal.Parse(worksheet.Cells[i, 8].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 9].Text.NullString() != "")
                        {
                            salary.IncentiveOther = decimal.Parse(worksheet.Cells[i, 9].Text.IfNullIsZero());
                            sl.IncentiveOther = decimal.Parse(worksheet.Cells[i, 9].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 10].Text.NullString() != "")
                        {
                            salary.IncentiveSixMonth1 = worksheet.Cells[i, 10].Text.NullString();
                            sl.IncentiveSixMonth1 = worksheet.Cells[i, 10].Text.NullString();
                        }

                        if (worksheet.Cells[i, 11].Text.NullString() != "")
                        {
                            salary.IncentiveSixMonth2 = worksheet.Cells[i, 11].Text.NullString();
                            sl.IncentiveSixMonth2 = worksheet.Cells[i, 11].Text.NullString();
                        }

                        if (worksheet.Cells[i, 12].Text.NullString() != "")
                        {
                            salary.HoTroCongDoan = decimal.Parse(worksheet.Cells[i, 12].Text.IfNullIsZero());
                            sl.HoTroCongDoan = decimal.Parse(worksheet.Cells[i, 12].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 13].Text.NullString() != "")
                        {
                            salary.PCCC_CoSo = decimal.Parse(worksheet.Cells[i, 13].Text.IfNullIsZero());
                            sl.PCCC_CoSo = decimal.Parse(worksheet.Cells[i, 13].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 14].Text.NullString() != "")
                        {
                            salary.HoTroATVS_SinhVien = decimal.Parse(worksheet.Cells[i, 14].Text.IfNullIsZero());
                            sl.HoTroATVS_SinhVien = decimal.Parse(worksheet.Cells[i, 14].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 15].Text.NullString() != "")
                        {
                            salary.ThuocDoiTuongBaoHiemXH = worksheet.Cells[i, 15].Text.NullString();
                            sl.ThuocDoiTuongBaoHiemXH = worksheet.Cells[i, 15].Text.NullString();
                        }

                        if (worksheet.Cells[i, 16].Text.NullString() != "")
                        {
                            salary.SoNguoiPhuThuoc = decimal.Parse(worksheet.Cells[i, 16].Text.IfNullIsZero());
                            sl.SoNguoiPhuThuoc = decimal.Parse(worksheet.Cells[i, 16].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 17].Text.NullString() != "")
                        {
                            salary.Note = worksheet.Cells[i, 17].Text.NullString();
                            sl.Note = worksheet.Cells[i, 17].Text.NullString();
                        }

                        if (worksheet.Cells[i, 18].Text.NullString() != "")
                        {
                            salary.DoiTuongTruyThuBHYT = worksheet.Cells[i, 18].Text.NullString();
                            sl.DoiTuongTruyThuBHYT = worksheet.Cells[i, 18].Text.NullString();
                        }

                        if (worksheet.Cells[i, 19].Text.NullString() != "")
                        {
                            salary.SoConNho = int.Parse(worksheet.Cells[i, 19].Text.IfNullIsZero());
                            sl.SoConNho = int.Parse(worksheet.Cells[i, 19].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 20].Text.NullString() != "")
                        {
                            salary.DoiTuongPhuCapDocHai = worksheet.Cells[i, 20].Text.NullString();
                            sl.DoiTuongPhuCapDocHai = worksheet.Cells[i, 20].Text.NullString();
                        }

                        lstUpdate.Add(salary);
                    }

                    _salaryRepository.UpdateRange(lstUpdate);
                    Save();

                    // update sqlite
                    if (lstAddPr.Count > 0)
                        _salarySQLiteRepository.AddRange(lstAddPr);

                    if (lstUpdatePr.Count > 0)
                        _salarySQLiteRepository.UpdateRange(lstUpdatePr);

                    _payrollUnitOfWork.Commit();

                    transaction.Commit();
                    resultDB.ReturnInt = 0;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                resultDB.ReturnInt = -1;
                resultDB.ReturnString = ex.Message;
            }
            return resultDB;
        }

        public ResultDB ImportTaiKhoanNHExcel(string filePath)
        {
            ResultDB resultDB = new ResultDB();
            try
            {
                using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = packet.Workbook.Worksheets[1];
                    List<HR_NHANVIEN> lstUpdate = new List<HR_NHANVIEN>();
                    HR_NHANVIEN nhanvien;
                    string manv;
                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                    {
                        manv = worksheet.Cells[i, 1].Text.NullString();
                        if (manv.NullString() == "")
                        {
                            break;
                        }

                        nhanvien = _nhanvienRespository.FindById(manv);

                        if (nhanvien == null)
                        {
                            continue;
                        }

                        if (worksheet.Cells[i, 3].Text.NullString() != "")
                        {
                            nhanvien.TenNganHang = worksheet.Cells[i, 3].Text.NullString();
                        }

                        if (worksheet.Cells[i, 4].Text.NullString() != "")
                        {
                            nhanvien.SoTaiKhoanNH = worksheet.Cells[i, 4].Text.NullString();
                        }

                        lstUpdate.Add(nhanvien);
                    }

                    _nhanvienRespository.UpdateRange(lstUpdate);
                    resultDB.ReturnInt = 0;
                }
            }
            catch (Exception ex)
            {
                resultDB.ReturnInt = -1;
                resultDB.ReturnString = ex.Message;
            }
            return resultDB;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public HR_SALARY UpdateSalary(HR_SALARY salary)
        {
            using var transaction = ((EFUnitOfWork)_unitOfWork).DBContext().Database.BeginTransaction();
            try
            {
                salary.UserModified = GetUserId();
                HR_SALARY_PR sl = _salarySQLiteRepository.FindSingle(x => x.MaNV == salary.MaNV);
                sl.CopyPropertiesFrom(salary, new List<string>() { "Id" });

                salary.BasicSalary = 0;
                salary.LivingAllowance = 0;
                salary.AbilityAllowance = 0;
                _salaryRepository.Update(salary);
                Save();

                _salarySQLiteRepository.Update(sl);
                _payrollUnitOfWork.Commit();

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            return salary;
        }

        #region Chi Phi Phat Sinh
        HR_SALARY_PHATSINH ISalaryService.AddSalaryPhatSinh(HR_SALARY_PHATSINH salary)
        {
            using var transaction = ((EFUnitOfWork)_unitOfWork).DBContext().Database.BeginTransaction();
            try
            {
                HR_SALARY_PHATSINH_PR pr = new HR_SALARY_PHATSINH_PR();

                salary.Key = Guid.NewGuid();
                salary.UserCreated = GetUserId();
                if (salary.ThoiGianApDung_From.Value != null)
                    salary.FromTime = salary.ThoiGianApDung_From.Value.ToString("yyyy-MM-dd");

                if (salary.ThoiGianApDung_To.Value != null)
                    salary.ToTime = salary.ThoiGianApDung_To.Value.ToString("yyyy-MM-dd");

                pr.CopyPropertiesFrom(salary, new List<string>() { "Id" });

                salary.SoTien = 0;
                _salaryPhatSinhRepository.Add(salary);
                Save();

                _salaryPhatSinhSQLiteRepository.Add(pr);
                _payrollUnitOfWork.Commit();

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
           
            return salary;
        }

        void ISalaryService.DeleteSalaryPhatSinh(int id)
        {
            using var transaction = ((EFUnitOfWork)_unitOfWork).DBContext().Database.BeginTransaction();
            try
            {
                Guid key  = _salaryPhatSinhRepository.FindById(id).Key;
                _salaryPhatSinhRepository.Remove(id);
                Save();

                HR_SALARY_PHATSINH_PR en = _salaryPhatSinhSQLiteRepository.FindSingle(x => x.Key.Equals(key));
                _salaryPhatSinhSQLiteRepository.Remove(en);
                _payrollUnitOfWork.Commit();

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
           
        }

        List<HR_SALARY_PHATSINH> ISalaryService.GetAllSalaryPhatSinh(int year)
        {
            List<HR_SALARY_PHATSINH> lst = _salaryPhatSinhRepository.FindAll(x => x.HR_NHANVIEN, y => y.HR_SALARY_DANHMUC_PHATSINH,z => z.HR_NHANVIEN.HR_BO_PHAN_DETAIL).Where(x => x.ThoiGianApDung_From.Value.Year == year).ToList();
            foreach (var item in lst)
            {
                item.SoTien = _salaryPhatSinhSQLiteRepository.FindSingle(x => x.Key.Equals(item.Key)).SoTien;
            }
            return lst;
        }

        List<HR_SALARY_DANHMUC_PHATSINH> ISalaryService.GetDanhMucPhatSinh()
        {
            return _salaryDanhMucPhatSinhRepository.FindAll().ToList();
        }

        HR_SALARY_PHATSINH ISalaryService.GetPhatSinhById(int id)
        {
            return _salaryPhatSinhRepository.FindById(id);
        }

        HR_SALARY_PHATSINH ISalaryService.UpdateSalaryPhatSinh(HR_SALARY_PHATSINH salary)
        {
            using var transaction = ((EFUnitOfWork)_unitOfWork).DBContext().Database.BeginTransaction();
            try
            {
                HR_SALARY_PHATSINH_PR pr = _salaryPhatSinhSQLiteRepository.FindSingle(x => x.Key.Equals(salary.Key));

                salary.UserModified = GetUserId();
                if (salary.ThoiGianApDung_From.Value != null)
                    salary.FromTime = salary.ThoiGianApDung_From.Value.ToString("yyyy-MM-dd");

                if (salary.ThoiGianApDung_To.Value != null)
                    salary.ToTime = salary.ThoiGianApDung_To.Value.ToString("yyyy-MM-dd");

                pr.CopyPropertiesFrom(salary, new List<string>() { "Id" });

                salary.SoTien = 0;
                _salaryPhatSinhRepository.Update(salary);
                Save();

                _salaryPhatSinhSQLiteRepository.Update(pr);
                _payrollUnitOfWork.Commit();

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
          
            return salary;
        }
        public HR_SALARY_PHATSINH GetPhatSinhByMaNV(string manv)
        {
            return _salaryPhatSinhRepository.FindSingle(x => x.MaNV == manv, y => y.HR_NHANVIEN, z =>z.HR_NHANVIEN.HR_BO_PHAN_DETAIL);
        }

        public ResultDB ImportPhatSinhExcel(string filePath)
        {
            ResultDB resultDB = new ResultDB();
            using var transaction = ((EFUnitOfWork)_unitOfWork).DBContext().Database.BeginTransaction();
            try
            {
                using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = packet.Workbook.Worksheets[1];
                    HR_SALARY_PHATSINH salary;
                    HR_SALARY_PHATSINH_PR pr;
                    List<HR_SALARY_PHATSINH> lstUpdate = new List<HR_SALARY_PHATSINH>();
                    List<HR_SALARY_PHATSINH_PR> lstUpdatePr = new List<HR_SALARY_PHATSINH_PR>();
                    string manv;
                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                    {
                        manv = worksheet.Cells[i, 1].Text.NullString();
                        if (manv.NullString() == "")
                        {
                            break;
                        }

                        Guid Key = Guid.NewGuid();
                        salary = new HR_SALARY_PHATSINH() { MaNV = manv, Key = Key };
                        pr = new HR_SALARY_PHATSINH_PR() { MaNV = manv,Key= Key };

                        if (worksheet.Cells[i, 3].Text.NullString() == "THUONG") // thưởng
                        {
                            salary.DanhMucPhatSinh = 2; // Id
                            pr.DanhMucPhatSinh = 2; // Id
                        }
                        else if (worksheet.Cells[i, 3].Text.NullString() == "PI") // PI
                        {
                            salary.DanhMucPhatSinh = 4; // Id
                            pr.DanhMucPhatSinh = 4; // Id
                        }
                        else if (worksheet.Cells[i, 3].Text.NullString() == "CI") // CI
                        {
                            salary.DanhMucPhatSinh = 3; // Id
                            pr.DanhMucPhatSinh = 3; // Id
                        }
                        else if (worksheet.Cells[i, 3].Text.NullString() == "QUY_PHONG_CHONG_THIEN_TAI") // Trừ quỹ phòng chống thiên tai 천해예방기금 차감
                        {
                            salary.DanhMucPhatSinh = 1; // Id
                            pr.DanhMucPhatSinh = 1; // Id
                        }
                        else if (worksheet.Cells[i, 3].Text.NullString() == "TT_TIEN_GIOI_THIEU") // Trừ quỹ phòng chống thiên tai 천해예방기금 차감
                        {
                            salary.DanhMucPhatSinh = 6; // Id
                            pr.DanhMucPhatSinh = 6; // Id
                        }

                        if (worksheet.Cells[i, 4].Text.NullString() != "")
                        {
                            pr.SoTien = double.Parse(worksheet.Cells[i, 4].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 5].Text.NullString() != "")
                        {
                            salary.ThoiGianApDung_From = DateTime.Parse(worksheet.Cells[i, 5].Text.NullString());
                            pr.ThoiGianApDung_From = DateTime.Parse(worksheet.Cells[i, 5].Text.NullString());
                        }

                        if (worksheet.Cells[i, 6].Text.NullString() != "")
                        {
                            salary.ThoiGianApDung_To = DateTime.Parse(worksheet.Cells[i, 6].Text.NullString());
                            pr.ThoiGianApDung_To = DateTime.Parse(worksheet.Cells[i, 6].Text.NullString());
                        }

                        if (salary.ThoiGianApDung_From.Value != null)
                        {
                            salary.FromTime = salary.ThoiGianApDung_From.Value.ToString("yyyy-MM-dd");
                            pr.FromTime = salary.ThoiGianApDung_From.Value.ToString("yyyy-MM-dd");
                        }

                        if (salary.ThoiGianApDung_To.Value != null)
                        {
                            salary.ToTime = salary.ThoiGianApDung_To.Value.ToString("yyyy-MM-dd");
                            pr.ToTime = salary.ThoiGianApDung_To.Value.ToString("yyyy-MM-dd");
                        }

                        lstUpdate.Add(salary);
                        lstUpdatePr.Add(pr);
                    }

                    if(lstUpdate.Count > 0)
                    {
                        _salaryPhatSinhRepository.AddRange(lstUpdate);
                        Save();
                    }
                   
                    if(lstUpdatePr.Count > 0)
                    {
                        _salaryPhatSinhSQLiteRepository.AddRange(lstUpdatePr);
                        _payrollUnitOfWork.Commit();
                    }

                    transaction.Commit();
                    resultDB.ReturnInt = 0;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                resultDB.ReturnInt = -1;
                resultDB.ReturnString = ex.Message;
            }
            return resultDB;
        }
        #endregion
    }
}
