using AutoMapper;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;
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
    public class TCardSampleService : BaseService, ITCardSampleService
    {
        private IRespository<TCARD_SAMPLE, int> _TcardResponsitory;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TCardSampleService(IRespository<TCARD_SAMPLE, int> TcardResponsitory, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _TcardResponsitory = TcardResponsitory;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public TCARD_SAMPLE Add(TCARD_SAMPLE model)
        {
            model.OutPutDatePlan = model.OutPutDatePlan2.ToString("yyyy-MM-dd");
            model.InputDatePlan = model.InputDatePlan2.ToString("yyyy-MM-dd");

            TCARD_SAMPLE newEn = _TcardResponsitory.FindSingle(x => x.LotNo == model.LotNo);
            if (newEn != null)
            {
                newEn.CopyPropertiesFrom(model, new List<string>() { "Id", "UserCreated", "DateCreated" });
                _TcardResponsitory.Update(newEn);
            }
            else
            {
                _TcardResponsitory.Add(model);
            }

            return model;
        }

        public void Delete(int Id)
        {
            _TcardResponsitory.Remove(Id);
            Save();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<TCARD_SAMPLE> GetAllData()
        {
            List<TCARD_SAMPLE> rs = _TcardResponsitory.FindAll().ToList();
            return rs;
        }

        public TCARD_SAMPLE GetById(int id)
        {
            return _TcardResponsitory.FindById(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="param">(code) Model_Lot no : file name</param>
        /// <returns></returns>
        public ResultDB ImportExcel(string filePath, string fileName)
        {
            ResultDB resultDB = new ResultDB();

            try
            {
                using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {

                    TCARD_SAMPLE en = null;

                    string sheetNameNomal = "WLP 앞면(HQ)";
                    string sheetNameBDMP = "KO";
                    string typeTcard = "";

                    ExcelWorksheet Sheet = null;

                    for (int x = 1; x <= packet.Workbook.Worksheets.Count; x++)
                    {
                        Sheet = packet.Workbook.Worksheets[x];
                        if (Sheet.Name == sheetNameBDMP)
                        {
                            typeTcard = "BDMP";
                            break;
                        }
                        else if (Sheet.Name == sheetNameNomal)
                        {
                            typeTcard = "NORMAL-ACE";
                            break;
                        }
                    }
                    string code = fileName.Split("_")[0].Substring(1, 1).NullString().ToUpper();
                    string model = fileName.Split("_")[0].Substring(3).NullString();

                    if (typeTcard == "NORMAL-ACE")
                    {
                        en = new TCARD_SAMPLE();
                        en.Code = code.NullString();
                        en.Model = model;
                        en.LotNo = Sheet.Cells["B3"].Text.NullString();
                        en.WaferID = Sheet.Cells["C3"].Text.NullString();
                        en.MucDichInput = Sheet.Cells["B4"].Text.NullString();
                        en.NguoiChiuTrachNhiem = Sheet.Cells["B5"].Text.NullString();
                        en.InputDatePlan = DateTime.Parse(Sheet.Cells["H5"].Text.NullString()).ToString("yyyy-MM-dd");
                        en.OutPutDatePlan = DateTime.Parse(Sheet.Cells["M5"].Text.NullString()).ToString("yyyy-MM-dd");

                        en.InputDatePlan2 = DateTime.Parse(Sheet.Cells["H5"].Text.NullString());
                        en.OutPutDatePlan2 = DateTime.Parse(Sheet.Cells["M5"].Text.NullString());

                        en.UserCreated = GetUserId();
                        en.UserModified = GetUserId();
                    }
                    else if (typeTcard == "BDMP")
                    {
                        en = new TCARD_SAMPLE();
                        en.Code = code.NullString();
                        en.Model = model;
                        en.LotNo = Sheet.Cells["C3"].Text.NullString();
                        en.WaferID = Sheet.Cells["D3"].Text.NullString();
                        en.MucDichInput = Sheet.Cells["C4"].Text.NullString();
                        en.NguoiChiuTrachNhiem = Sheet.Cells["C5"].Text.NullString();
                        en.InputDatePlan = DateTime.Parse(Sheet.Cells["G5"].Text.NullString()).ToString("yyyy-MM-dd");
                        en.OutPutDatePlan = DateTime.Parse(Sheet.Cells["K5"].Text.NullString()).ToString("yyyy-MM-dd");

                        en.InputDatePlan2 = DateTime.Parse(Sheet.Cells["G5"].Text.NullString());
                        en.OutPutDatePlan2 = DateTime.Parse(Sheet.Cells["K5"].Text.NullString());

                        en.UserCreated = GetUserId();
                        en.UserModified = GetUserId();
                    }
                    else
                    {
                        resultDB.ReturnInt = -1;
                        resultDB.ReturnString = "Tcard BDMP cần có sheet name là: 'KO' ,loại khác là 'WLP 앞면(HQ)' !";
                        return resultDB;
                    }

                    if(en != null)
                    {
                        TCARD_SAMPLE newEn = _TcardResponsitory.FindSingle(x => x.LotNo == en.LotNo);
                        if (newEn != null)
                        {
                            newEn.CopyPropertiesFrom(en, new List<string>() { "Id", "UserCreated", "DateCreated" });
                            _TcardResponsitory.Update(newEn);
                        }
                        else
                        {
                            _TcardResponsitory.Add(en);
                        }
                    }
                    else
                    {
                        resultDB.ReturnInt = -1;
                        resultDB.ReturnString = "SheetName invalid!";
                        return resultDB;
                    }
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

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public TCARD_SAMPLE Update(TCARD_SAMPLE model)
        {
            model.OutPutDatePlan = model.OutPutDatePlan2.ToString("yyyy-MM-dd");
            model.InputDatePlan = model.InputDatePlan2.ToString("yyyy-MM-dd");
            _TcardResponsitory.Update(model);
            return model;
        }
    }
}
