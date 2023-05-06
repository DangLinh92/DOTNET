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
using System.Linq;
using System.Text;

namespace OPERATION_MNS.Application.Implementation
{
    public class ThicknessWlp2Service : BaseService, IThicknessWlp2Service
    {
        private IRespository<THICKNET_MODEL_WLP2, int> _Responsitory;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ThicknessWlp2Service(IRespository<THICKNET_MODEL_WLP2, int> Responsitory, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _Responsitory = Responsitory;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public ThickNetModelWlp2ViewModel Add(ThickNetModelWlp2ViewModel model)
        {
            var en = _mapper.Map<THICKNET_MODEL_WLP2>(model);
            _Responsitory.Add(en);
            return model;
        }

        public void Delete(int Id)
        {
            _Responsitory.Remove(Id);
            Save();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<ThickNetModelWlp2ViewModel> GetAllData()
        {
            List<ThickNetModelWlp2ViewModel> rs = _mapper.Map<List<ThickNetModelWlp2ViewModel>>(_Responsitory.FindAll());
            return rs.OrderBy(x=>x.ThickNet).ToList();
        }

        public ThickNetModelWlp2ViewModel GetById(int id)
        {
            return _mapper.Map<ThickNetModelWlp2ViewModel>(_Responsitory.FindById(id));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public ThickNetModelWlp2ViewModel Update(ThickNetModelWlp2ViewModel model)
        {
            var en = _mapper.Map<THICKNET_MODEL_WLP2>(model);
            _Responsitory.Update(en);
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
                    THICKNET_MODEL_WLP2 en = null;
                    List<THICKNET_MODEL_WLP2> lstSap = new List<THICKNET_MODEL_WLP2>();
                    List<THICKNET_MODEL_WLP2> lstSapUpdate = new List<THICKNET_MODEL_WLP2>();

                    string material = "";
                    float thickness;

                    for (int i = Sheet.Dimension.Start.Row + 1; i <= Sheet.Dimension.End.Row; i++)
                    {
                        if (Sheet.Cells[i, 1].Text.NullString() == "")
                        {
                            break;
                        }

                        if(!float.TryParse(Sheet.Cells[i, 2].Text.NullString(),out _))
                        {
                            continue;
                        }

                        material = Sheet.Cells[i, 1].Text.NullString();
                        thickness = float.Parse(Sheet.Cells[i, 2].Text.NullString());

                        en = _Responsitory.FindSingle(x => x.Material == material);

                        if (en == null)
                        {
                            en = new THICKNET_MODEL_WLP2()
                            {
                                Material = material,
                                ThickNet = thickness
                            };

                            lstSap.Add(en);
                        }
                        else
                        {
                            en.ThickNet = thickness;
                            lstSapUpdate.Add(en);
                        }
                    }

                    if (lstSap.Count > 0)
                        _Responsitory.AddRange(lstSap);

                    if (lstSapUpdate.Count > 0)
                        _Responsitory.UpdateRange(lstSapUpdate);
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
