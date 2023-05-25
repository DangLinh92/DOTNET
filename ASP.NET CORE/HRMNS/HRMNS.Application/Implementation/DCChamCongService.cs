using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMNS.Utilities.Dtos;
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
    public class DCChamCongService : BaseService, IDCChamCongService
    {
        private IUnitOfWork _unitOfWork;
        IRespository<DC_CHAM_CONG, int> _dcChamCongRepository;
        private readonly IMapper _mapper;

        public DCChamCongService(IRespository<DC_CHAM_CONG, int> respository, IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _dcChamCongRepository = respository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public DCChamCongViewModel Add(DCChamCongViewModel dmDcChamCongVm)
        {
            dmDcChamCongVm.UserCreated = GetUserId();
            dmDcChamCongVm.NgayDieuChinh2 = dmDcChamCongVm.NgayDieuChinh.Value.ToString("yyyy-MM-dd");
            if(dmDcChamCongVm.ChiTraVaoLuongThang != null)
            {
                dmDcChamCongVm.ChiTraVaoLuongThang2 = dmDcChamCongVm.ChiTraVaoLuongThang.Value.ToString("yyyy-MM") + "-01";
            }
            var entity = _mapper.Map<DC_CHAM_CONG>(dmDcChamCongVm);
            _dcChamCongRepository.Add(entity);
            Save();
            return _mapper.Map<DCChamCongViewModel>(entity);
        }

        public void Delete(int id)
        {
            _dcChamCongRepository.Remove(id);
            Save();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<DCChamCongViewModel> GetAll(string keyword, params Expression<Func<DC_CHAM_CONG, object>>[] includeProperties)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return _mapper.Map<List<DCChamCongViewModel>>(_dcChamCongRepository.FindAll(x => x.Id > 0, includeProperties).OrderBy(x => x.DateModified));
            }
            else
            {
                return _mapper.Map<List<DCChamCongViewModel>>(_dcChamCongRepository.FindAll(x => x.NoiDungDC.Contains(keyword), includeProperties).OrderBy(x => x.DateModified));
            }
        }

        public DCChamCongViewModel GetById(int id, params Expression<Func<DC_CHAM_CONG, object>>[] includeProperties)
        {
            return _mapper.Map<DCChamCongViewModel>(_dcChamCongRepository.FindById(id));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public List<DCChamCongViewModel> Search(string status, string dept, string timeFrom, string timeTo, params Expression<Func<DC_CHAM_CONG, object>>[] includeProperties)
        {
            if (string.IsNullOrEmpty(dept))
            {
                if (string.IsNullOrEmpty(status))
                {
                    if (string.IsNullOrEmpty(timeFrom) && string.IsNullOrEmpty(timeTo))
                    {
                        return GetAll("", includeProperties);
                    }
                    return _mapper.Map<List<DCChamCongViewModel>>(_dcChamCongRepository.FindAll(x => string.Compare(x.NgayDieuChinh2, timeFrom) >= 0 && string.Compare(x.NgayDieuChinh2, timeTo) <= 0, includeProperties));
                }
                else
                {
                    if (string.IsNullOrEmpty(timeFrom) && string.IsNullOrEmpty(timeTo))
                    {
                        return _mapper.Map<List<DCChamCongViewModel>>(_dcChamCongRepository.FindAll(x => x.TrangThaiChiTra == status, includeProperties));
                    }

                    return _mapper.Map<List<DCChamCongViewModel>>(_dcChamCongRepository.FindAll(x => x.TrangThaiChiTra == status && string.Compare(x.NgayDieuChinh2, timeFrom) >= 0 && string.Compare(x.NgayDieuChinh2, timeTo) <= 0, includeProperties));
                }
            }
            else
            {
                if (string.IsNullOrEmpty(status))
                {
                    if (string.IsNullOrEmpty(timeFrom) && string.IsNullOrEmpty(timeTo))
                    {
                        return _mapper.Map<List<DCChamCongViewModel>>(_dcChamCongRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept, includeProperties));
                    }

                    return _mapper.Map<List<DCChamCongViewModel>>(_dcChamCongRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept && string.Compare(x.NgayDieuChinh2, timeFrom) >= 0 && string.Compare(x.NgayDieuChinh2, timeTo) <= 0, includeProperties));
                }
                else
                {
                    if (string.IsNullOrEmpty(timeFrom) && string.IsNullOrEmpty(timeTo))
                    {
                        return _mapper.Map<List<DCChamCongViewModel>>(_dcChamCongRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept && x.TrangThaiChiTra == status, includeProperties));
                    }

                    return _mapper.Map<List<DCChamCongViewModel>>(_dcChamCongRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept && x.TrangThaiChiTra == status && string.Compare(x.NgayDieuChinh2, timeFrom) >= 0 && string.Compare(x.NgayDieuChinh2, timeTo) <= 0, includeProperties));
                }
            }
        }

        public void Update(DCChamCongViewModel dmDCChamCongVm)
        {
            dmDCChamCongVm.UserModified = GetUserId();
            dmDCChamCongVm.NgayDieuChinh2 = dmDCChamCongVm.NgayDieuChinh.Value.ToString("yyyy-MM-dd");
            if (dmDCChamCongVm.ChiTraVaoLuongThang != null)
            {
                dmDCChamCongVm.ChiTraVaoLuongThang2 = dmDCChamCongVm.ChiTraVaoLuongThang.Value.ToString("yyyy-MM") + "-01";
            }
            var entity = _mapper.Map<DC_CHAM_CONG>(dmDCChamCongVm);
            _dcChamCongRepository.Update(entity);
            Save();
        }

        public ResultDB ImportExcel(string filePath)
        {
            ResultDB resultDB = new ResultDB();
            try
            {
                using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = packet.Workbook.Worksheets[1];
                    DC_CHAM_CONG dccong;
                    List<DC_CHAM_CONG> lstUpdate = new List<DC_CHAM_CONG>();
                    List<DC_CHAM_CONG> lstAdd = new List<DC_CHAM_CONG>();
                    string manv, ngaydieuchinh;

                    for (int i = worksheet.Dimension.Start.Row + 2; i <= worksheet.Dimension.End.Row; i++)
                    {
                        manv = worksheet.Cells[i, 1].Text.NullString();
                        ngaydieuchinh = worksheet.Cells[i, 3].Text.NullString();

                        if (manv.NullString() == "")
                        {
                            break;
                        }

                        dccong = _dcChamCongRepository.FindSingle(x => x.MaNV == manv && x.NgayDieuChinh2 == ngaydieuchinh);

                        if (dccong == null)
                        {
                            dccong = new DC_CHAM_CONG();
                            lstAdd.Add(dccong);
                        }
                        else
                        {
                            lstUpdate.Add(dccong);
                        }

                        dccong.MaNV = manv;

                        if (worksheet.Cells[i, 3].Text.NullString() != "")
                        {
                            dccong.NgayDieuChinh2 = DateTime.Parse(worksheet.Cells[i, 3].Text.NullString()).ToString("yyyy-MM-dd");
                            dccong.NgayDieuChinh = DateTime.Parse(worksheet.Cells[i, 3].Text.NullString());
                        }

                        if (worksheet.Cells[i, 4].Text.NullString() != "")
                        {
                            dccong.NgayCong = float.Parse(worksheet.Cells[i, 4].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 5].Text.NullString() != "")
                        {
                            dccong.DSNS = float.Parse(worksheet.Cells[i, 5].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 6].Text.NullString() != "")
                        {
                            dccong.NSBH = float.Parse(worksheet.Cells[i, 6].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 7].Text.NullString() != "")
                        {
                            dccong.DC85 = float.Parse(worksheet.Cells[i, 7].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 8].Text.NullString() != "")
                        {
                            dccong.DC100 = float.Parse(worksheet.Cells[i, 8].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 9].Text.NullString() != "")
                        {
                            dccong.DC150 = float.Parse(worksheet.Cells[i, 9].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 10].Text.NullString() != "")
                        {
                            dccong.DC190 = float.Parse(worksheet.Cells[i, 10].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 11].Text.NullString() != "")
                        {
                            dccong.DC200 = float.Parse(worksheet.Cells[i, 11].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 12].Text.NullString() != "")
                        {
                            dccong.DC210 = float.Parse(worksheet.Cells[i, 12].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 13].Text.NullString() != "")
                        {
                            dccong.DC270 = float.Parse(worksheet.Cells[i, 13].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 14].Text.NullString() != "")
                        {
                            dccong.DC300 = float.Parse(worksheet.Cells[i, 14].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 15].Text.NullString() != "")
                        {
                            dccong.DC390 = float.Parse(worksheet.Cells[i, 15].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 16].Text.NullString() != "")
                        {
                            dccong.ELLC = float.Parse(worksheet.Cells[i, 16].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 17].Text.NullString() != "")
                        {
                            dccong.Other = float.Parse(worksheet.Cells[i, 17].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 18].Text.NullString() != "")
                        {
                            dccong.NoiDungDC = worksheet.Cells[i, 18].Text.NullString();
                        }

                        if (worksheet.Cells[i, 19].Text.NullString() != "")
                        {
                            dccong.TongSoTien = double.Parse(worksheet.Cells[i, 19].Text.NullString());
                        }

                        if (worksheet.Cells[i, 20].Text.NullString() != "")
                        {
                            dccong.ChiTraVaoLuongThang = DateTime.Parse(worksheet.Cells[i, 20].Text.NullString());
                            dccong.ChiTraVaoLuongThang2 = DateTime.Parse(worksheet.Cells[i, 20].Text.NullString()).ToString("yyyy-MM") + "-01";
                        }
                    }

                    _dcChamCongRepository.AddRange(lstAdd);
                    _dcChamCongRepository.UpdateRange(lstUpdate);
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
    }
}
