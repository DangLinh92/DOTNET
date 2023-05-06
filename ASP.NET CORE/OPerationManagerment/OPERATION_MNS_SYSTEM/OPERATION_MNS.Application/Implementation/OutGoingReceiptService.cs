using AutoMapper;
using Microsoft.AspNetCore.Http;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Data.EF;
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

        public List<OUTGOING_RECEIPT_WLP2> GetAllToDay()
        {
            throw new NotImplementedException();
        }

        public List<OUTGOING_RECEIPT_WLP2> GetAllByDay(string ngayxuat)
        {
            List<OUTGOING_RECEIPT_WLP2> lstData = new List<OUTGOING_RECEIPT_WLP2>();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("A_DAY_GET", ngayxuat);

            ResultDB resultDB = _OutGoingReceiptResponsitory.ExecProceduce2("GET_OUTGOING_RECEIPT_WLP2", dic);

            if (resultDB.ReturnInt == 0)
            {
                DataTable data = resultDB.ReturnDataSet.Tables[0];

                var _lstOutGoing = _OutGoingReceiptResponsitory.FindAll(x => x.NgayXuat == ngayxuat).ToList();
                List<OUTGOING_RECEIPT_WLP2> lstUpdate = new List<OUTGOING_RECEIPT_WLP2>();
                List<OUTGOING_RECEIPT_WLP2> lstAdd = new List<OUTGOING_RECEIPT_WLP2>();
                OUTGOING_RECEIPT_WLP2 _outGoig;
                foreach (DataRow row in data.Rows)
                {
                    if (_lstOutGoing.Any(x => x.SapCode == row["SapCode"].NullString() && x.NgayXuat == row["_Day"].NullString()))
                    {
                        _outGoig = _lstOutGoing.FirstOrDefault(x => x.SapCode == row["SapCode"].NullString() &&
                                                                x.NgayXuat == row["_Day"].NullString());

                        _outGoig.LuongDuKien_1 = float.Parse(row["Shipping_Asum"].IfNullIsZero());

                        if (!lstUpdate.Exists(x => x.SapCode == _outGoig.SapCode && x.NgayXuat == _outGoig.NgayXuat))
                            lstUpdate.Add(_outGoig);
                    }
                    else
                    {
                        if (!lstAdd.Any(x => x.SapCode == row["SapCode"].NullString() && x.NgayXuat == row["_Day"].NullString()))
                        {
                            _outGoig = new OUTGOING_RECEIPT_WLP2()
                            {
                                Module = row["Model"].NullString(),
                                SapCode = row["SapCode"].NullString(),
                                NgayXuat = row["_Day"].NullString(),
                                SoLuongYeuCau = float.Parse(row["SoLuongYeuCau"].IfNullIsZero()),
                                LuongDuKien_1 = float.Parse(row["Shipping_Asum"].IfNullIsZero()),
                                ThoiGianDuKien_1 = "A0812",
                                ThoiGianDuKien_2 = "A1220",
                                ThoiGianDuKien_3 = "B2024",
                                ThoiGianDuKien_4 = "B0008",

                                ThoiGianThucTe_1 = "A0812",
                                ThoiGianThucTe_2 = "A1220",
                                ThoiGianThucTe_3 = "B2024",
                                ThoiGianThucTe_4 = "B0008",
                            };
                            lstAdd.Add(_outGoig);
                        }
                    }
                }

                if (lstUpdate.Count > 0)
                    _OutGoingReceiptResponsitory.UpdateRange(lstUpdate);

                if (lstAdd.Count > 0)
                    _OutGoingReceiptResponsitory.AddRange(lstAdd);

                Save();

                var lstOutGoing = _OutGoingReceiptResponsitory.FindAll(x => x.NgayXuat == ngayxuat).ToList();

                OUTGOING_RECEIPT_WLP2 item;
                OUTGOING_RECEIPT_WLP2 outGoing;
                foreach (DataRow row in data.Rows)
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
                    item.NguoiGiao = outGoing?.NguoiGiao;

                    if (outGoing != null)
                    {
                        item.LuongDuKien_1 = outGoing.LuongDuKien_1 > 0 ? outGoing.LuongDuKien_1 : float.Parse(row["Shipping_Asum"].IfNullIsZero());
                        item.LuongDuKien_2 = outGoing.LuongDuKien_2 > 0 ? outGoing.LuongDuKien_2 : 0;
                        item.LuongDuKien_3 = outGoing.LuongDuKien_3 > 0 ? outGoing.LuongDuKien_3 : 0;
                        item.LuongDuKien_4 = outGoing.LuongDuKien_4 > 0 ? outGoing.LuongDuKien_4 : 0;
                    }
                    else
                    {
                        item.LuongDuKien_1 = float.Parse(row["Shipping_Asum"].IfNullIsZero());
                        item.LuongDuKien_2 = 0;
                        item.LuongDuKien_3 = 0;
                        item.LuongDuKien_4 = 0;
                    }

                    item.ThoiGianDuKien_1 = "A0812";
                    item.ThoiGianDuKien_2 = "A1220";
                    item.ThoiGianDuKien_3 = "B2024";
                    item.ThoiGianDuKien_4 = "B0008";

                    if (row["_Day_Time"].NullString() == "A0812")
                    {
                        item.LuongThucTe_1 = float.Parse(row["Shipping_Real"].IfNullIsZero());
                        item.ThoiGianThucTe_1 = "A0812";
                    }
                    else if (row["_Day_Time"].NullString() == "A1220")
                    {
                        item.LuongThucTe_2 = float.Parse(row["Shipping_Real"].IfNullIsZero());
                        item.ThoiGianThucTe_2 = "A1220";
                    }
                    else if (row["_Day_Time"].NullString() == "B2024")
                    {
                        item.LuongThucTe_3 = float.Parse(row["Shipping_Real"].IfNullIsZero());
                        item.ThoiGianThucTe_3 = "B2024";
                    }
                    else if (row["_Day_Time"].NullString() == "B0008")
                    {
                        item.LuongThucTe_4 = float.Parse(row["Shipping_Real"].IfNullIsZero());
                        item.ThoiGianThucTe_4 = "B0008";
                    }

                    item.Note = outGoing?.Note;

                    if (!lstData.Exists(x => x.SapCode == row["SapCode"].NullString()))
                    {
                        item.UserCreated = GetUserId();
                        lstData.Add(item);
                    }
                }
                outGoing = null;
            }

            return lstData.ToList();
        }

        public OUTGOING_RECEIPT_WLP2 GetById(int id)
        {
            return _OutGoingReceiptResponsitory.FindById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public OUTGOING_RECEIPT_WLP2 Update(OUTGOING_RECEIPT_WLP2 model, string key)
        {
            _OutGoingReceiptResponsitory.Update(model);
            Save();
            return model;
        }

        public OUTGOING_RECEIPT_WLP2 GetByKey(string key)
        {
            return _OutGoingReceiptResponsitory.FindSingle(x => x.SapCode + "." + x.NgayXuat == key);
        }
    }
}
