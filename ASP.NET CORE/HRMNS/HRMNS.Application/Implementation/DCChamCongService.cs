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
            dmDcChamCongVm.TongSoTien = dmDcChamCongVm.TongSoTien == null ? 0 : dmDcChamCongVm.TongSoTien;
            if (dmDcChamCongVm.ChiTraVaoLuongThang != null)
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
                    return _mapper.Map<List<DCChamCongViewModel>>(_dcChamCongRepository.FindAll(x => string.Compare(x.ChiTraVaoLuongThang2, timeFrom) >= 0 && string.Compare(x.ChiTraVaoLuongThang2, timeTo) <= 0, includeProperties));
                }
                else
                {
                    if (string.IsNullOrEmpty(timeFrom) && string.IsNullOrEmpty(timeTo))
                    {
                        return _mapper.Map<List<DCChamCongViewModel>>(_dcChamCongRepository.FindAll(x => x.TrangThaiChiTra == status, includeProperties));
                    }

                    return _mapper.Map<List<DCChamCongViewModel>>(_dcChamCongRepository.FindAll(x => x.TrangThaiChiTra == status && string.Compare(x.ChiTraVaoLuongThang2, timeFrom) >= 0 && string.Compare(x.ChiTraVaoLuongThang2, timeTo) <= 0, includeProperties));
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

                    return _mapper.Map<List<DCChamCongViewModel>>(_dcChamCongRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept && string.Compare(x.ChiTraVaoLuongThang2, timeFrom) >= 0 && string.Compare(x.ChiTraVaoLuongThang2, timeTo) <= 0, includeProperties));
                }
                else
                {
                    if (string.IsNullOrEmpty(timeFrom) && string.IsNullOrEmpty(timeTo))
                    {
                        return _mapper.Map<List<DCChamCongViewModel>>(_dcChamCongRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept && x.TrangThaiChiTra == status, includeProperties));
                    }

                    return _mapper.Map<List<DCChamCongViewModel>>(_dcChamCongRepository.FindAll(x => x.HR_NHANVIEN.MaBoPhan == dept && x.TrangThaiChiTra == status && string.Compare(x.ChiTraVaoLuongThang2, timeFrom) >= 0 && string.Compare(x.ChiTraVaoLuongThang2, timeTo) <= 0, includeProperties));
                }
            }
        }

        public void Update(DCChamCongViewModel dmDCChamCongVm)
        {
            dmDCChamCongVm.UserModified = GetUserId();
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
                    ExcelWorksheet worksheet = packet.Workbook.Worksheets[0];
                    DC_CHAM_CONG dccong;
                    List<DC_CHAM_CONG> lstAdd = new List<DC_CHAM_CONG>();
                    string manv, thangchitra;

                    for (int i = worksheet.Dimension.Start.Row + 2; i <= worksheet.Dimension.End.Row; i++)
                    {
                        manv = worksheet.Cells[i, 1].Text.NullString();
                        thangchitra = worksheet.Cells[i, 6].Text.NullString();

                        if (manv.NullString() == "" || !DateTime.TryParse(thangchitra + "-01", out _))
                        {
                            break;
                        }

                        dccong = new DC_CHAM_CONG();
                        dccong.MaNV = manv;
                        dccong.NgayDieuChinh = worksheet.Cells[i, 3].Text.NullString();
                        dccong.NoiDungDC = worksheet.Cells[i, 4].Text.NullString();

                        if (worksheet.Cells[i, 5].Text.NullString() != "")
                        {
                            dccong.TongSoTien = double.Parse(worksheet.Cells[i, 5].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 6].Text.NullString() != "")
                        {
                            dccong.ChiTraVaoLuongThang = DateTime.Parse(worksheet.Cells[i, 6].Text.NullString() + "-01");
                            dccong.ChiTraVaoLuongThang2 = DateTime.Parse(worksheet.Cells[i, 6].Text.NullString() + "-01").ToString("yyyy-MM") + "-01";
                        }
                        lstAdd.Add(dccong);
                    }

                    _dcChamCongRepository.AddRange(lstAdd);
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
