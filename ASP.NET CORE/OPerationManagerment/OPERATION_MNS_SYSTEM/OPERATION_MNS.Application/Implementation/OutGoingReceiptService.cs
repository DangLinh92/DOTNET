using AutoMapper;
using Microsoft.AspNetCore.Http;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Infrastructure.Interfaces;
using OPERATION_MNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace OPERATION_MNS.Application.Implementation
{
    public class OutGoingReceiptService : BaseService, IOutGoingReceiptService
    {
        private IRespository<OUTGOING_RECEIPT_WLP2, int> _OutGoingReceiptResponsitory;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OutGoingReceiptService(IRespository<OUTGOING_RECEIPT_WLP2, int> OutGoingReceiptResponsitory, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _OutGoingReceiptResponsitory = OutGoingReceiptResponsitory;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<OUTGOING_RECEIPT_WLP2> GetAllByDay(string day)
        {
            throw new NotImplementedException();
        }

        public List<OUTGOING_RECEIPT_WLP2> GetAllToDay()
        {
            List<OUTGOING_RECEIPT_WLP2> lstData = new List<OUTGOING_RECEIPT_WLP2>();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            ResultDB resultDB = _OutGoingReceiptResponsitory.ExecProceduce2("GET_OUTGOING_RECEIPT_WLP2", dic);

            if (resultDB.ReturnInt == 0)
            {
                string ngayxuat = "";
                if (int.Parse(DateTime.Now.ToString("HH")) >= 8 && int.Parse(DateTime.Now.ToString("HH")) <= 23)
                {
                    ngayxuat = DateTime.Now.ToString("yyyyMMdd");
                }
                else
                {
                    ngayxuat = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
                }

                var lstOutGoing = _OutGoingReceiptResponsitory.FindAll(x => x.NgayXuat == ngayxuat).ToList();
                OUTGOING_RECEIPT_WLP2 item;
                OUTGOING_RECEIPT_WLP2 outGoing;
                foreach (DataRow row in resultDB.ReturnDataSet.Tables[0].Rows)
                {
                    if (lstData.Exists(x => x.SapCode == row["SapCode"].NullString()))
                    {
                        item = lstData.FirstOrDefault(x => x.SapCode == row["SapCode"].NullString());
                        item.UserModified = GetUserId();
                    }
                    else
                    {
                        item = new OUTGOING_RECEIPT_WLP2();
                        item.SapCode = row["SapCode"].NullString();
                        item.SoLuongYeuCau = float.Parse(row["SoLuongYeuCau"].IfNullIsZero());
                        item.Module = row["Model"].NullString();
                        item.NgayXuat = row["_Day"].NullString();
                    }

                    outGoing = lstOutGoing.FirstOrDefault(x => x.SapCode == item.SapCode && x.NgayXuat.Replace("-", "") == item.NgayXuat);

                    if (row["_Day_Time"].NullString() == "A0812")
                    {
                        if (outGoing != null)
                        {
                            item.LuongDuKien_1 = outGoing.LuongDuKien_1 > 0 ? outGoing.LuongDuKien_1 : float.Parse(row["Shipping_Asum"].IfNullIsZero());
                        }
                        else
                        {
                            item.LuongDuKien_1 = float.Parse(row["Shipping_Asum"].IfNullIsZero());
                        }

                        item.LuongThucTe_1 = float.Parse(row["Shipping_Real"].IfNullIsZero());

                        item.ThoiGianDuKien_1 = "A0812";
                        item.ThoiGianThucTe_1 = "A0812";
                    }
                    else if (row["_Day_Time"].NullString() == "A1220")
                    {
                        if (outGoing != null)
                        {
                            item.LuongDuKien_2 = outGoing.LuongDuKien_2 > 0 ? outGoing.LuongDuKien_2 : float.Parse(row["Shipping_Asum"].IfNullIsZero());
                        }
                        else
                        {
                            item.LuongDuKien_2 = float.Parse(row["Shipping_Asum"].IfNullIsZero());
                        }

                        item.LuongThucTe_2 = float.Parse(row["Shipping_Real"].IfNullIsZero());
                        item.ThoiGianDuKien_2 = "A1220";
                        item.ThoiGianThucTe_2 = "A1220";
                    }
                    else if (row["_Day_Time"].NullString() == "B2024")
                    {
                        if (outGoing != null)
                        {
                            item.LuongDuKien_3 = outGoing.LuongDuKien_3 > 0 ? outGoing.LuongDuKien_3 : float.Parse(row["Shipping_Asum"].IfNullIsZero());
                        }
                        else
                        {
                            item.LuongDuKien_3 = float.Parse(row["Shipping_Asum"].IfNullIsZero());
                        }

                        item.LuongThucTe_3 = float.Parse(row["Shipping_Real"].IfNullIsZero());

                        item.ThoiGianDuKien_3 = "B2024";
                        item.ThoiGianThucTe_3 = "B2024";
                    }
                    else if (row["_Day_Time"].NullString() == "B0008")
                    {
                        if (outGoing != null)
                        {
                            item.LuongDuKien_4 = outGoing.LuongDuKien_4 > 0 ? outGoing.LuongDuKien_4 : float.Parse(row["Shipping_Asum"].IfNullIsZero());
                        }
                        else
                        {
                            item.LuongDuKien_4 = float.Parse(row["Shipping_Asum"].IfNullIsZero());
                        }

                        item.LuongThucTe_4 = float.Parse(row["Shipping_Real"].IfNullIsZero());

                        item.ThoiGianDuKien_4 = "B0008";
                        item.ThoiGianThucTe_4 = "B0008";
                    }

                    item.Note = outGoing?.Note;
                   
                    if (!lstData.Exists(x => x.SapCode == row["SapCode"].NullString()))
                    {
                        item.UserCreated = GetUserId();
                        lstData.Add(item);
                    }
                }
            }

            return lstData;
        }

        public OUTGOING_RECEIPT_WLP2 GetById(int id)
        {
            return _OutGoingReceiptResponsitory.FindById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public OUTGOING_RECEIPT_WLP2 Update(OUTGOING_RECEIPT_WLP2 model)
        {
            OUTGOING_RECEIPT_WLP2 en = _OutGoingReceiptResponsitory.FindSingle(x => x.SapCode + "." + x.NgayXuat.Replace("-", "") == model.Key);

            if(en != null)
            {
                en.CopyPropertiesFrom(model, new List<string>() { "Id","Module", "LotId", "SapCode", "NgayXuat", "ChenhLechDuKien", "ChenhLechThucTe", "Key", "DateCreated", "DateModified", "UserCreated", "UserModified" });
                _OutGoingReceiptResponsitory.Update(en);
            }
            else
            {
                _OutGoingReceiptResponsitory.Add(model);
            }
            
            Save();
            return model;
        }

        public OUTGOING_RECEIPT_WLP2 GetByKey(string key)
        {
            return _OutGoingReceiptResponsitory.FindSingle(x => x.SapCode + "." + x.NgayXuat == key);
        }
    }
}
