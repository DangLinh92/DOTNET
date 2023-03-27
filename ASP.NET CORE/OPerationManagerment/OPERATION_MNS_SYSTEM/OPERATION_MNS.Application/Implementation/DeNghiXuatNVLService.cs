using AutoMapper;
using OfficeOpenXml;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Application.ViewModels.Wlp2;
using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Infrastructure.Interfaces;
using OPERATION_MNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.Implementation
{
    public class DeNghiXuatNVLService : BaseService, IDeNghiXuatNVLService
    {
        private IRespository<BOPHAN_DE_NGHI_XUAT_NLIEU, int> _DenghiResponsitory;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeNghiXuatNVLService(IRespository<BOPHAN_DE_NGHI_XUAT_NLIEU, int> DenghiResponsitory, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _DenghiResponsitory = DenghiResponsitory;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public BoPhanDeNghiXuatNVLViewModel Add(BoPhanDeNghiXuatNVLViewModel model)
        {
            var en = _mapper.Map<BOPHAN_DE_NGHI_XUAT_NLIEU>(model);
            _DenghiResponsitory.Add(en);
            return model;
        }

        public void Delete(int Id)
        {
            _DenghiResponsitory.Remove(Id);
            Save();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<BoPhanDeNghiXuatNVLViewModel> GetAllData(string ngay)
        {
            List<BoPhanDeNghiXuatNVLViewModel> rs = _mapper.Map<List<BoPhanDeNghiXuatNVLViewModel>>(_DenghiResponsitory.FindAll(x => x.NgayDeNghi == ngay));
            return rs;
        }

        public BoPhanDeNghiXuatNVLViewModel GetById(int id)
        {
            return _mapper.Map<BoPhanDeNghiXuatNVLViewModel>(_DenghiResponsitory.FindById(id));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public BoPhanDeNghiXuatNVLViewModel Update(BoPhanDeNghiXuatNVLViewModel model)
        {
            var en = _mapper.Map<BOPHAN_DE_NGHI_XUAT_NLIEU>(model);
            _DenghiResponsitory.Update(en);
            return model;
        }

        public ResultDB ImportExcel(string filePath, string param)
        {
            ResultDB resultDB = new ResultDB();

            try
            {
                using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ExcelWorksheet Sheet = packet.Workbook.Worksheets[1];
                    BOPHAN_DE_NGHI_XUAT_NLIEU en = null;
                    List<BOPHAN_DE_NGHI_XUAT_NLIEU> lstSap = new List<BOPHAN_DE_NGHI_XUAT_NLIEU>();
                    List<BOPHAN_DE_NGHI_XUAT_NLIEU> lstSapUpdate = new List<BOPHAN_DE_NGHI_XUAT_NLIEU>();

                    string ngay = "";
                    string sapCode = "";

                    for (int i = Sheet.Dimension.Start.Row + 1; i <= Sheet.Dimension.End.Row; i++)
                    {
                        ngay = Sheet.Cells[i, 1].Text.NullString();
                        sapCode = Sheet.Cells[i, 3].Text.NullString();

                        if (ngay == "" || sapCode == "" )
                        {
                            break;
                        }

                        if(!float.TryParse(Sheet.Cells[i, 6].Value.NullString(),out _))
                        {
                            continue;
                        }

                        ngay = DateTime.Parse(ngay).ToString("yyyy-MM-dd");

                        en = _DenghiResponsitory.FindSingle(x => x.NgayDeNghi == ngay && x.SapCode == sapCode);

                        if (en == null)
                        {
                            en = new BOPHAN_DE_NGHI_XUAT_NLIEU()
                            {
                                NgayDeNghi = ngay,
                                SapCode = sapCode
                            };

                            en.Module = Sheet.Cells[i, 2].Text.NullString();
                            en.DinhMuc = float.Parse(Sheet.Cells[i, 4].Value.IfNullIsZero());
                            en.DonVi = Sheet.Cells[i, 5].Text.NullString();
                            en.SoLuongYeuCau = float.Parse(Sheet.Cells[i, 6].Value.IfNullIsZero());
                            en.Note = Sheet.Cells[i, 7].Text.NullString();
                            lstSap.Add(en);
                        }
                        else
                        {
                            en.Module = Sheet.Cells[i, 2].Text.NullString();
                            en.DinhMuc = float.Parse(Sheet.Cells[i, 4].Value.IfNullIsZero());
                            en.DonVi = Sheet.Cells[i, 5].Text.NullString();
                            en.SoLuongYeuCau = float.Parse(Sheet.Cells[i, 6].Value.IfNullIsZero());
                            en.Note = Sheet.Cells[i, 7].Text.NullString();
                            lstSapUpdate.Add(en);
                        }
                    }

                    if (lstSap.Count > 0)
                        _DenghiResponsitory.AddRange(lstSap);

                    if (lstSapUpdate.Count > 0)
                        _DenghiResponsitory.UpdateRange(lstSapUpdate);
                }
            }
            catch (Exception ex)
            {
                resultDB.ReturnInt = -1;
                resultDB.ReturnString = ex.Message;
                return resultDB;
            }

            resultDB.ReturnInt = 0;
            return resultDB;
        }
    }
}
