using AutoMapper;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;
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
    public class PostOprationShippingService : IPostOprationShippingService
    {
        private IRespository<POST_OPERATION_SHIPPING, int> _PostOpeationRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PostOprationShippingService(IRespository<POST_OPERATION_SHIPPING, int> PostOpeationRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _PostOpeationRepository = PostOpeationRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<PostOpeationShippingViewModel> GetPostOpeationShipping(string fromTime, string toTime)
        {
            List<PostOpeationShippingViewModel> posts = new List<PostOpeationShippingViewModel>();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("A_FROM", fromTime);
            dic.Add("A_TO", toTime);
            ResultDB result = _PostOpeationRepository.ExecProceduce2("PKG_BUSINESS.GET_PostOperationShipping", dic);
            if (result.ReturnInt == 0)
            {
                DataTable table = result.ReturnDataSet.Tables[0];
                if (table.Rows.Count > 0)
                {
                    PostOpeationShippingViewModel post;
                    foreach (DataRow item in table.Rows)
                    {
                        post = new PostOpeationShippingViewModel()
                        {
                            MoveOutTime = item["Move Out Time"].NullString(),
                            LotID = item["Lot ID"].NullString(),
                            Module = item["Module"].NullString(),
                            CassetteID = item["Cassette ID"].NullString(),
                            Model = item["Material"].NullString(),
                            WaferId = item["Wafer ID"].NullString(),
                            DefaultChipQty = double.Parse(item["Default Chip Qty"].IfNullIsZero()),
                            OutputQty = double.Parse(item["Output Qty"].IfNullIsZero()),
                            ChipMapQty = double.Parse(item["ChipMapQty"].IfNullIsZero()),
                            ChipMesQty = double.Parse(item["ChipMesQty"].IfNullIsZero()),
                            DiffMapMes = double.Parse(item["ChipMapQty"].IfNullIsZero()) - double.Parse(item["ChipMesQty"].IfNullIsZero()),
                            Rate = double.Parse(item["Rate"].IfNullIsZero()),
                            VanDeDacBiet = item["VanDeDacBiet"].NullString(),
                            WaferQty = double.Parse(item["WaferQty"].IfNullIsZero()),
                            ChipQty = double.Parse(item["ChipQty"].IfNullIsZero()),
                            NguoiXuat = item["NguoiXuat"].NullString(),
                            NguoiKiemTraFA = item["NguoiKiemTraFA"].NullString(),
                            NguoiNhan = item["NguoiNhan"].NullString(),
                            NguoiKiemTra = item["NguoiKiemTra"].NullString(),
                            GhiChu_XH2 = item["GhiChu_XH2"].NullString(),
                            GhiChu_XH3 = item["GhiChu_XH3"].NullString(),
                            KetQuaFAKiemTra = item["KetQuaFAKiemTra"].NullString(),
                            WaferId_Mes = item["WaferId_Mes"].NullString(),
                            InDB = item["InDB"].NullString()
                        };
                        posts.Add(post);
                    }
                }
            }

            return posts;
        }

        public List<XuatHangViewModel> GetXuatHangViewModel(string fromTime, string toTime)
        {
            List<PostOpeationShippingViewModel> data = GetPostOpeationShipping(fromTime, toTime);
            XuatHangViewModel result = new XuatHangViewModel();
            result.DataXH = data;

            var xh3 = from t in data
                      group t by new
                      {
                          t.Module,
                          t.Model
                      } into gr
                      select new XuatHang3ViewModel
                      {
                          Model = gr.Key.Model,
                          Module = gr.Key.Module,
                          WaferQty = gr.Count(),
                          ChipQty = gr.Sum(x => x.OutputQty),
                          GhiChu = ""
                      };

            result.XuatHang3ViewModels.AddRange(xh3.ToList());

            
            var xh2 = from t in data
                      group t by new
                      {
                          t.CassetteID,
                          t.Module,
                          t.Model,
                          t.MoveOutTime,
                          //t.WaferId,
                          t.NguoiXuat,
                          t.NguoiNhan,
                          t.KetQuaFAKiemTra,
                          t.NguoiKiemTra,
                          t.GhiChu_XH2
                      } into gr
                      select new XuatHang2ViewModel
                      {
                          Ngay = gr.Key.MoveOutTime,
                          Model = gr.Key.Model,
                          Module = gr.Key.Module,
                          CasstteID = gr.Key.CassetteID,
                          WaferQty = gr.Count(),
                          ChipQty = gr.Sum(x => x.OutputQty),
                          //WaferID = gr.Key.WaferId,
                          NguoiXuat = gr.Key.NguoiXuat,
                          KetQuaFAKiemTra = gr.Key.KetQuaFAKiemTra,
                          NguoiKiemTra = gr.Key.NguoiKiemTra,
                          NguoiNhan = gr.Key.NguoiNhan,
                          GhiChu = gr.Key.GhiChu_XH2,
                      };

            int stt = 0;
            var lst = xh2.ToList();
            foreach (var item in lst)
            {
                item.STT = ++stt;
                foreach (var sub in data.OrderBy(x=>x.LotID))
                {
                    if(item.CasstteID == sub.CassetteID 
                        && item.Module == sub.Module 
                        && item.Model == sub.Model
                        && item.Ngay == sub.MoveOutTime)
                    {
                        item.WaferID += sub.LotID.Substring(sub.LotID.Length - 3, 1)+".";
                    }
                }

                if (item.WaferID.EndsWith("."))
                {
                    item.WaferID = item.WaferID.Substring(0,item.WaferID.Length - 1);
                }
            }

            result.XuatHang2ViewModels.AddRange(lst);
            XuatHang1ViewModel xuatHang1;

            int Srno = 0;
            foreach (var item in data)
            {
                xuatHang1 = new XuatHang1ViewModel()
                {
                    STT = ++Srno,
                    NgayXuat = item.MoveOutTime,
                    LotID = item.LotID,
                    Module = item.Module,
                    Model = item.Model,
                    WaferID = item.WaferId_Mes,
                    CasstteID = item.CassetteID,
                    DefaultChipQty = item.DefaultChipQty,
                    ChipMesQty = item.ChipMesQty,
                    ChipMapQty = item.ChipMapQty,
                    DiffMapMes = item.DiffMapMes,
                    Rate = item.Rate,
                    VanDeDacBiet = item.VanDeDacBiet
                };
                result.XuatHang1ViewModels.Add(xuatHang1);
            }

            return new List<XuatHangViewModel>() { result };
        }

        public PostOpeationShippingViewModel Add(PostOpeationShippingViewModel model)
        {
           var en  = _mapper.Map<POST_OPERATION_SHIPPING>(model);
            _PostOpeationRepository.Add(en);
            return model;
        }

        public PostOpeationShippingViewModel Update(PostOpeationShippingViewModel model)
        {
            var en = _mapper.Map<POST_OPERATION_SHIPPING>(model);
            _PostOpeationRepository.Update(en);
            return model;
        }

        public void Delete(PostOpeationShippingViewModel model)
        {
            throw new NotImplementedException();
        }

        public PostOpeationShippingViewModel FindItemXH1(string key)
        {
            List<PostOpeationShippingViewModel> lst = new List<PostOpeationShippingViewModel>();
            lst = _mapper.Map<List<PostOpeationShippingViewModel>>(_PostOpeationRepository.FindAll(x => x.MoveOutTime + x.LotID == key));
            return lst.FirstOrDefault();
        }

        public List<PostOpeationShippingViewModel> FindItemXH2(string key)
        {
            List<PostOpeationShippingViewModel> lst = new List<PostOpeationShippingViewModel>();
            lst = _mapper.Map<List<PostOpeationShippingViewModel>>(_PostOpeationRepository.FindAll(x => x.MoveOutTime + x.Module + x.Model + x.CassetteID == key)); //Ngay + Module + Model + CasstteID
            return lst;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
