using AutoMapper;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using VOC.Application.Interfaces;
using VOC.Application.ViewModels.System;
using VOC.Application.ViewModels.VOC;
using VOC.Data.EF.Extensions;
using VOC.Data.Entities;
using VOC.Infrastructure.Interfaces;
using VOC.Utilities.Dtos;

namespace VOC.Application.Implementation
{
    public class VocMstService : BaseService, IVocMstService
    {
        private IRespository<VOC_MST, int> _vocRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VocMstService(IRespository<VOC_MST, int> vocRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _vocRepository = vocRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public VOC_MSTViewModel Add(VOC_MSTViewModel voc)
        {
            voc.UserCreated = GetUserId();
            var entity = _mapper.Map<VOC_MST>(voc);
            _vocRepository.Add(entity);
            return voc;
        }

        public void Delete(int id)
        {
            _vocRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<VOC_MSTViewModel> GetAll(string filter)
        {
            throw new NotImplementedException();
        }

        public VOC_MSTViewModel GetById(int id)
        {
            return _mapper.Map<VOC_MSTViewModel>(_vocRepository.FindById(id));
        }

        public ResultDB ImportExcel(string filePath, string param)
        {
            ResultDB resultDB = new ResultDB();
            try
            {
                using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = packet.Workbook.Worksheets[1];
                    List<VOC_MST> lstVocMst = new List<VOC_MST>();

                    DataTable table = new DataTable();
                    table.Columns.Add("Received_site");
                    table.Columns.Add("PlaceOfOrigin");
                    table.Columns.Add("ReceivedDept");
                    table.Columns.Add("ReceivedDate");
                    table.Columns.Add("SPLReceivedDate");
                    table.Columns.Add("SPLReceivedDateWeek");
                    table.Columns.Add("Customer");
                    table.Columns.Add("SETModelCustomer");
                    table.Columns.Add("ProcessCustomer");
                    table.Columns.Add("ModelFullname");
                    table.Columns.Add("DefectNameCus");
                    table.Columns.Add("DefectRate");
                    table.Columns.Add("PBA_FAE_Result");
                    table.Columns.Add("PartsClassification");
                    table.Columns.Add("PartsClassification2");
                    table.Columns.Add("ProdutionDateMarking");
                    table.Columns.Add("AnalysisResult");
                    table.Columns.Add("VOCCount");
                    table.Columns.Add("DefectCause");
                    table.Columns.Add("DefectClassification");
                    table.Columns.Add("CustomerResponse");
                    table.Columns.Add("Report_FinalApprover");
                    table.Columns.Add("Report_Sender");
                    table.Columns.Add("Rport_sentDate");
                    table.Columns.Add("VOCState");
                    table.Columns.Add("VOCFinishingDate");
                    table.Columns.Add("VOC_TAT");

                    DataRow row = null;
                    for (int i = worksheet.Dimension.Start.Row + 4; i <= worksheet.Dimension.End.Row; i++)
                    {
                        row = table.NewRow();

                        row["Received_site"] = worksheet.Cells[i, 3].Text.NullString();
                        row["PlaceOfOrigin"] = worksheet.Cells[i, 4].Text.NullString();
                        row["ReceivedDept"] = worksheet.Cells[i, 5].Text.NullString();
                        row["ReceivedDate"] = worksheet.Cells[i, 6].Text.NullString();
                        row["SPLReceivedDate"] = worksheet.Cells[i, 7].Text.NullString();
                        row["SPLReceivedDateWeek"] = worksheet.Cells[i, 8].Text.NullString();
                        row["Customer"] = worksheet.Cells[i, 9].Text.NullString();
                        row["SETModelCustomer"] = worksheet.Cells[i, 10].Text.NullString();
                        row["ProcessCustomer"] = worksheet.Cells[i, 11].Text.NullString();
                        row["ModelFullname"] = worksheet.Cells[i, 12].Text.NullString();
                        row["DefectNameCus"] = worksheet.Cells[i, 13].Text.NullString();
                        row["DefectRate"] = worksheet.Cells[i, 14].Text.NullString();
                        row["PBA_FAE_Result"] = worksheet.Cells[i, 15].Text.NullString();
                        row["PartsClassification"] = worksheet.Cells[i, 16].Text.NullString();
                        row["PartsClassification2"] = worksheet.Cells[i, 17].Text.NullString();
                        row["ProdutionDateMarking"] = worksheet.Cells[i, 18].Text.NullString();
                        row["AnalysisResult"] = worksheet.Cells[i, 19].Text.NullString();
                        row["VOCCount"] = worksheet.Cells[i, 20].Text.NullString();
                        row["DefectCause"] = worksheet.Cells[i, 21].Text.NullString();
                        row["DefectClassification"] = worksheet.Cells[i, 22].Text.NullString();
                        row["CustomerResponse"] = worksheet.Cells[i, 23].Text.NullString();
                        row["Report_FinalApprover"] = worksheet.Cells[i, 24].Text.NullString();
                        row["Report_Sender"] = worksheet.Cells[i, 25].Text.NullString();
                        row["Rport_sentDate"] = worksheet.Cells[i, 26].Text.NullString();
                        row["VOCState"] = worksheet.Cells[i, 27].Text.NullString();
                        row["VOCFinishingDate"] = worksheet.Cells[i, 28].Text.NullString();
                        row["VOC_TAT"] = worksheet.Cells[i, 29].Text.NullString();
                        table.Rows.Add(row);
                    }

                    resultDB = _vocRepository.ExecProceduce("PKG_BUSINESS.PUT_VOC", new Dictionary<string, string>(), "A_DATA", table);
                }
                return resultDB;
            }
            catch (Exception ex)
            {
                resultDB.ReturnInt = -1;
                resultDB.ReturnString = ex.Message;
                return resultDB;
            }
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(VOC_MSTViewModel function)
        {
            function.UserModified = GetUserId();
            var entity = _mapper.Map<VOC_MST>(function);
            _vocRepository.Update(entity);
        }
    }
}
