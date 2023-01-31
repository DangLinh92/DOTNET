using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.EHS;
using HRMNS.Data.EF.Extensions;
using HRMNS.Utilities.Common;
using HRMNS.Utilities.Constants;
using HRMNS.Utilities.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace HRMS.Areas.Admin.Controllers
{
    public class EhsDanhMucKeHoachController : AdminBaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IDanhMucKeHoachService _danhMucKeHoachService;
        public EhsDanhMucKeHoachController(IDanhMucKeHoachService danhMucKeHoachService, IWebHostEnvironment hostingEnvironment)
        {
            _danhMucKeHoachService = danhMucKeHoachService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            EhsDanhMucKeHoachPageViewModel model = _danhMucKeHoachService.GetDataDanhMucKeHoachPage(null);
            model.Year = DateTime.Now.Year.ToString();
            return View(model);
        }

        [HttpPost]
        public IActionResult GetDeMucKH(string kehoachID)
        {
            EhsDanhMucKeHoachPageViewModel model = _danhMucKeHoachService.GetDataDanhMucKeHoachPage(Guid.Parse(kehoachID));
            // model.Year = DateTime.Now.Year.ToString();
            return View("Index", model);
        }

        [HttpPost]
        public IActionResult GetNoiDungLuatKeHoach(string kehoachID)
        {
            var lst = _danhMucKeHoachService.GetLuatDinhKeHoach(Guid.Parse(kehoachID));
            string ldinh = lst.FirstOrDefault()?.NoiDungLuatDinh;
            return new OkObjectResult(ldinh);
        }

        [HttpPost]
        public IActionResult AddKeHoach(string nameVN, string nameKR, string luatDinh)
        {
            Guid idKh = Guid.NewGuid();
            int maxOrder = 0;
            if (_danhMucKeHoachService.GetDataDanhMucKeHoachPage(null).EhsDMKeHoachViewModels.OrderByDescending(x => x.OrderDM).FirstOrDefault() != null)
            {
                maxOrder = _danhMucKeHoachService.GetDataDanhMucKeHoachPage(null).EhsDMKeHoachViewModels.OrderByDescending(x => x.OrderDM).FirstOrDefault().OrderDM + 1;
            }

            EhsDMKeHoachViewModel ehsDM = new EhsDMKeHoachViewModel()
            {
                Id = idKh,
                TenKeHoach_VN = nameVN,
                TenKeHoach_KR = nameKR,
                UserCreated = UserName,
                UserModified = UserName,
                OrderDM = maxOrder
            };
            EhsLuatDinhKeHoachViewModel luatDinhKH = new EhsLuatDinhKeHoachViewModel()
            {
                MaKeHoach = idKh,
                NoiDungLuatDinh = luatDinh,
                UserCreated = UserName,
                UserModified = UserName
            };
            ehsDM.EHS_LUATDINH_KEHOACH.Add(luatDinhKH);

            _danhMucKeHoachService.UpdateDMKeHoach(ehsDM);
            _danhMucKeHoachService.Save();

            return new OkObjectResult(idKh);
        }

        [HttpPost]
        public IActionResult GetTenDemucKeHoach(string maDemuc)
        {
            DeMucLuatDinh demucLD = _danhMucKeHoachService.GetTenDeMucKeHoach(Guid.Parse(maDemuc));
            return new OkObjectResult(demucLD);
        }

        [HttpPost]
        public IActionResult GetNoiDungKeHoach(string maNoiDung)
        {
            var noidung = _danhMucKeHoachService.GetNoiDungKeHoach(Guid.Parse(maNoiDung));
            return new OkObjectResult(noidung);
        }

        [HttpPost]
        public IActionResult UpdateNoiDung(string maNoiDung, string noidung)
        {
            _danhMucKeHoachService.UpdateNoiDung(Guid.Parse(maNoiDung), noidung);
            _danhMucKeHoachService.Save();
            return new OkObjectResult(maNoiDung);
        }

        [HttpPost]
        public IActionResult DeleteNoiDung(string maNoiDung)
        {
            _danhMucKeHoachService.DeleteNoiDung(Guid.Parse(maNoiDung));
            _danhMucKeHoachService.Save();
            return new OkObjectResult(maNoiDung);
        }

        [HttpPost]
        public IActionResult UpdateDemucKeHoach(string maDemuc, string tenDemuc, string luatDinh)
        {
            _danhMucKeHoachService.UpdateDeMucKeHoach(Guid.Parse(maDemuc), tenDemuc);
            _danhMucKeHoachService.UpdateLuatDinhDeMucKeHoach(Guid.Parse(maDemuc), luatDinh);
            _danhMucKeHoachService.Save();
            return new OkObjectResult(maDemuc);
        }

        [HttpPost]
        public IActionResult DeleteDemucKeHoach(string maDemuc)
        {
            _danhMucKeHoachService.DeleteDeMucKeHoach(Guid.Parse(maDemuc));
            _danhMucKeHoachService.Save();
            return new OkObjectResult(maDemuc);
        }

        [HttpPost]
        public IActionResult DeleteKeHoach(string maKeHoach)
        {
            _danhMucKeHoachService.DeleteDMKeHoach(Guid.Parse(maKeHoach));
            _danhMucKeHoachService.Save();
            return new OkObjectResult(maKeHoach);
        }

        [HttpPost]
        public IActionResult GetNoiDungChiTiet(string maNoiDung, string year)
        {
            ChiTietNoiDungChiPhiViewModel model = new ChiTietNoiDungChiPhiViewModel();
            if (year.NullString() == "") year = DateTime.Now.Year.ToString();

            var lstNoiDung = _danhMucKeHoachService.GetNoiDungKeHoachByMaNoiDung(maNoiDung, year).OrderBy(x => x.NgayThucHien).ToList();
            var chiphi = _danhMucKeHoachService.GetChiPhiNoiDung(maNoiDung, year);
            if(chiphi != null)
            {
                chiphi.TenNoiDung = lstNoiDung.FirstOrDefault()?.EHS_NOIDUNG.NoiDung;
                model.ChiPhi.Add(chiphi);
            }
            else
            {
                EhsChiPhiByMonthViewModel cphi = new EhsChiPhiByMonthViewModel()
                {
                    MaNoiDung = Guid.Parse(maNoiDung),
                    TenNoiDung = lstNoiDung.FirstOrDefault()?.EHS_NOIDUNG.NoiDung,
                    Year = year
                };

                _danhMucKeHoachService.AddChiPhi(cphi);

                var newChiphi = _danhMucKeHoachService.GetChiPhiNoiDung(maNoiDung, year);
                newChiphi.TenNoiDung = lstNoiDung.FirstOrDefault()?.EHS_NOIDUNG.NoiDung;
                model.ChiPhi.Add(newChiphi);
            }
          
            model.NoiDungChiTiet = lstNoiDung;
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult GetNoiDungChiTietById(string Id)
        {
            var lstNoiDung = _danhMucKeHoachService.GetNoiDungKeHoachById(Id);
            return new OkObjectResult(lstNoiDung);
        }

        [HttpPost]
        public IActionResult UpdateNoiDungChiTiet(EhsNoiDungKeHoachViewModel model)
        {
            var newModel = _danhMucKeHoachService.GetNoiDungKeHoachById(model.Id.ToString());

            if (newModel != null)
            {
                string maNoiDung = newModel.EHS_NOIDUNG.Id.ToString();
                newModel.NhaThau = model.NhaThau;
                newModel.ChuKy = model.ChuKy;
                newModel.ViTri = model.ViTri;
                newModel.SoLuong = model.SoLuong;
                newModel.NgayThucHien = model.NgayThucHien;
                newModel.ThoiGian_ThucHien = model.ThoiGian_ThucHien;
                newModel.YeuCau = model.YeuCau;
                newModel.NgayKhaiBaoThietBi = model.NgayKhaiBaoThietBi;
                newModel.ThoiGianThongBao = model.ThoiGianThongBao;
                newModel.Year = DateTime.Parse(model.NgayThucHien).Year.ToString();

                newModel.MaHieuMayKiemTra = model.MaHieuMayKiemTra;
                newModel.TienDoHoanThanh = model.TienDoHoanThanh;
                newModel.KetQua = model.KetQua;
                newModel.NguoiPhucTrach = model.NguoiPhucTrach;

                newModel.EHS_NOIDUNG = null;
                _danhMucKeHoachService.UpdateNoiDungKeHoach(newModel);
                _danhMucKeHoachService.Save();
                return new OkObjectResult(new
                {
                    MaNoiDung = maNoiDung
                });
            }
            else
            {
                return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
            }
        }

        [HttpPost]
        public IActionResult DeleteNoiDungChiTiet(string Id)
        {
            var maNoiDung = _danhMucKeHoachService.GetNoiDungKeHoachById(Id)?.EHS_NOIDUNG.Id;
            _danhMucKeHoachService.DeleteNoiDungKeHoach(Id);
            _danhMucKeHoachService.Save();
            return new OkObjectResult(new
            {
                MaNoiDung = maNoiDung
            });
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public IActionResult ImportExcel(IList<IFormFile> files, [FromQuery] string param)
        {
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                var filename = ContentDispositionHeaderValue
                                   .Parse(file.ContentDisposition)
                                   .FileName
                                   .Trim('"');

                string folder = _hostingEnvironment.WebRootPath + $@"\uploaded\excels";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, CorrelationIdGenerator.GetNextId() + filename);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                ResultDB result = _danhMucKeHoachService.ImportExcel(filePath, param);

                if (System.IO.File.Exists(filePath))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(filePath);
                }

                if (result.ReturnInt == 0)
                {
                    return new OkObjectResult(filePath);
                }
                else
                {
                    return new BadRequestObjectResult(result.ReturnString);
                }
            }

            return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
        }


        [HttpPost]
        public IActionResult GetFileNoiDungChiTiet(string maKeHoach, string year)
        {
            var lstND = _danhMucKeHoachService.GetNoiDungByKeHoach(Guid.Parse(maKeHoach), year).OrderBy(x => x.MaDeMucKH).ToList();

            if (lstND == null || lstND.Count == 0)
                return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string sFileName = $"EhsNoiDungChiTiet_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
            FileInfo fileSrc = new FileInfo(Path.Combine(Path.Combine(sWebRootFolder, "templates"), "Ehs_NoiDungDemucChiTietTemplate.xlsx"));
            if (file.Exists)
            {
                file.Delete();
            }

            if (fileSrc.Exists)
            {
                fileSrc.CopyTo(file.FullName, true);
            }

            using (ExcelPackage package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets["NoiDungChiTiet"];

                int start = 0;
                List<EhsNoiDungKeHoachViewModel> lstKeHoach = new List<EhsNoiDungKeHoachViewModel>();
                for (int i = 0; i < lstND.Count; i++)
                {
                    if (lstND[i].EHS_NOIDUNG_KEHOACH.Count > 0)
                    {
                        for (int j = 0; j < lstND[i].EHS_NOIDUNG_KEHOACH.Count; j++)
                        {
                            lstKeHoach = lstND[i].EHS_NOIDUNG_KEHOACH.ToList();
                            start += 1;

                            worksheet.Cells["A2:S2"].Copy(worksheet.Cells["A" + (start + 2) + ":S" + (start + 2)]);

                            worksheet.Cells["A" + (start + 2)].Value = lstKeHoach[j].Id.NullString();
                            worksheet.Cells["B" + (start + 2)].Value = lstND[i].Id.NullString();
                            worksheet.Cells["C" + (start + 2)].Value = lstND[i].NoiDung.NullString();

                            worksheet.Cells["D" + (start + 2)].Value = lstKeHoach[j].NhaThau.NullString();
                            worksheet.Cells["E" + (start + 2)].Value = lstKeHoach[j].ChuKy.NullString().Contains("_") ? lstKeHoach[j].ChuKy.ToString().Split("_")[0] : "";
                            worksheet.Cells["F" + (start + 2)].Value = lstKeHoach[j].ChuKy.NullString().Contains("_") ? lstKeHoach[j].ChuKy.ToString().Split("_")[1] : "";
                            worksheet.Cells["G" + (start + 2)].Value = lstKeHoach[j].MaHieuMayKiemTra.NullString();
                            worksheet.Cells["H" + (start + 2)].Value = lstKeHoach[j].SoLuong;
                            worksheet.Cells["I" + (start + 2)].Value = lstKeHoach[j].ViTri.NullString();
                            worksheet.Cells["J" + (start + 2)].Value = lstKeHoach[j].NgayThucHien.NullString();
                            worksheet.Cells["K" + (start + 2)].Value = lstKeHoach[j].ThoiGian_ThucHien.NullString();
                            worksheet.Cells["L" + (start + 2)].Value = lstKeHoach[j].YeuCau.NullString();
                            worksheet.Cells["M" + (start + 2)].Value = lstKeHoach[j].NgayKhaiBaoThietBi.NullString();
                            worksheet.Cells["N" + (start + 2)].Value = lstKeHoach[j].ThoiGianThongBao.NullString().Contains("_") ? lstKeHoach[j].ThoiGianThongBao.NullString().Split("_")[0] : "";
                            worksheet.Cells["O" + (start + 2)].Value = lstKeHoach[j].ThoiGianThongBao.NullString().Contains("_") ? lstKeHoach[j].ThoiGianThongBao.NullString().Split("_")[1] : "";
                            worksheet.Cells["P" + (start + 2)].Value = lstKeHoach[j].TienDoHoanThanh.NullString();
                            worksheet.Cells["Q" + (start + 2)].Value = lstKeHoach[j].KetQua.NullString();
                            worksheet.Cells["R" + (start + 2)].Value = lstKeHoach[j].NguoiPhucTrach.NullString();
                        }
                    }
                    else
                    {
                        start += 1;

                        worksheet.Cells["A2:S2"].Copy(worksheet.Cells["A" + (start + 2) + ":S" + (start + 2)]);

                        worksheet.Cells["A" + (start + 2)].Value = "";
                        worksheet.Cells["B" + (start + 2)].Value = lstND[i].Id.NullString();
                        worksheet.Cells["C" + (start + 2)].Value = lstND[i].NoiDung.NullString();
                    }
                }
                worksheet.DeleteRow(2); // xoa row 2
                package.Save(); //Save the workbook.
            }
            return new OkObjectResult(fileUrl);
        }

        [HttpPost]
        public IActionResult GetChiPhiByNoiDung(string noidungId, string year)
        {
            var obj = _danhMucKeHoachService.GetChiPhiNoiDung(noidungId, year);
            return new OkObjectResult(obj);
        }

        #region Add Update Chi Phi
        [HttpPost]
        public IActionResult Post(string values)
        {
            EhsChiPhiByMonthViewModel model = new EhsChiPhiByMonthViewModel();
            JsonConvert.PopulateObject(values, model);

            _danhMucKeHoachService.AddChiPhi(model);
            return Ok();
        }

        [HttpPut]
        public IActionResult Put(int key, string values)
        {
            var model = _danhMucKeHoachService.GetChiPhiById(key);
            JsonConvert.PopulateObject(values, model);
            _danhMucKeHoachService.UpdateChiPhi(model);
            return Ok();
        }

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(new List<EhsChiPhiByMonthViewModel>(), loadOptions);
        }
        #endregion
    }
}
