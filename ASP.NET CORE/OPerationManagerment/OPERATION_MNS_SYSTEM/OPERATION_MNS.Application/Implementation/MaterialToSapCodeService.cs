using AutoMapper;
using OfficeOpenXml;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Infrastructure.Interfaces;
using OPERATION_MNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.Implementation
{
    public class MaterialToSapCodeService : BaseService, IMaterialToSapCodeService
    {
        private IRespository<MATERIAL_TO_SAP, int> _MaterialToSapResponsitory;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MaterialToSapCodeService(IRespository<MATERIAL_TO_SAP, int> MaterialToSapResponsitory, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _MaterialToSapResponsitory = MaterialToSapResponsitory;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public MaterialToSapViewModel Add(MaterialToSapViewModel model)
        {
            var en = _mapper.Map<MATERIAL_TO_SAP>(model);
            _MaterialToSapResponsitory.Add(en);
            return model;
        }

        public void Delete(int Id)
        {
            _MaterialToSapResponsitory.Remove(Id);
            Save();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<MaterialToSapViewModel> GetAllData(string dept)
        {
            List<MaterialToSapViewModel> rs = _mapper.Map<List<MaterialToSapViewModel>>(_MaterialToSapResponsitory.FindAll(x => x.Department == dept));
            return rs;
        }

        public MaterialToSapViewModel GetById(int id)
        {
            return _mapper.Map<MaterialToSapViewModel>(_MaterialToSapResponsitory.FindById(id));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public MaterialToSapViewModel Update(MaterialToSapViewModel model)
        {
            var en = _mapper.Map<MATERIAL_TO_SAP>(model);
            _MaterialToSapResponsitory.Update(en);
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
                    MATERIAL_TO_SAP en = null;
                    List<MATERIAL_TO_SAP> lstSap = new List<MATERIAL_TO_SAP>();
                    List<MATERIAL_TO_SAP> lstSapUpdate = new List<MATERIAL_TO_SAP>();

                    string material = "";
                    string sapCode = "";

                    for (int i = Sheet.Dimension.Start.Row + 1; i <= Sheet.Dimension.End.Row; i++)
                    {
                        if (Sheet.Cells[i, 1].Text.NullString() == "")
                        {
                            break;
                        }

                        material = Sheet.Cells[i, 1].Text.NullString();
                        sapCode = Sheet.Cells[i, 2].Text.NullString();

                        en = _MaterialToSapResponsitory.FindSingle(x => x.Material == material && x.Department == param);

                        if (en == null)
                        {
                            en = new MATERIAL_TO_SAP()
                            {
                                Material = material,
                                SAP_Code = sapCode,
                                Department = param
                            };

                            lstSap.Add(en);
                        }
                        else
                        {
                            en.SAP_Code = sapCode;
                            lstSapUpdate.Add(en);
                        }
                    }

                    if (lstSap.Count > 0)
                        _MaterialToSapResponsitory.AddRange(lstSap);

                    if (lstSapUpdate.Count > 0)
                        _MaterialToSapResponsitory.UpdateRange(lstSapUpdate);
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
