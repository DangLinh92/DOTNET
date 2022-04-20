using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VOC.Application.Interfaces;
using VOC.Application.ViewModels.VOC;
using VOC.Data.EF.Extensions;
using VOC.Data.Entities;
using VOC.Infrastructure.Interfaces;
using VOC.Utilities.Constants;

namespace VOC.Application.Implementation
{
    public class VocOnsiteService : BaseService, IVocOnsiteSevice
    {
        private IRespository<VOC_ONSITE, int> _vocRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VocOnsiteService(IRespository<VOC_ONSITE, int> vocRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _vocRepository = vocRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<VocOnsiteViewModel> GetAllOnsiteByTime(string fromTime, string toTime)
        {
            return _mapper.Map<List<VocOnsiteViewModel>>(_vocRepository.FindAll(x => string.Compare(x.Date, fromTime) >= 0 && string.Compare(x.Date, toTime) <= 0).OrderByDescending(x => x.DateModified));
        }

        public List<VocOnsiteModel> SumDataOnsite(int year, string customer, string part)
        {
            List<VocOnsiteViewModel> lstOnsite;
            List<VocOnsiteModel> lstVocOnsite = new List<VocOnsiteModel>();
            if (year > 0)
            {
                string _customer = customer;

                if (customer.Contains("result"))
                {
                    _customer = customer.Split('-')[0];
                }

                if (part == CommonConstants.ALL)
                {
                    lstOnsite = _mapper.Map<List<VocOnsiteViewModel>>(_vocRepository.FindAll(x => x.Date.StartsWith(year + "") && x.Customer == _customer).ToList());
                }
                else
                {
                    lstOnsite = _mapper.Map<List<VocOnsiteViewModel>>(_vocRepository.FindAll(x => x.Date.StartsWith(year + "") && x.Part == part && x.Customer == _customer).ToList());
                }

                if (customer.Contains("result"))
                {
                    var lstGroup = lstOnsite.GroupBy(x => (x.Customer, x.Date, x.Week, x.Month, x.Part)).Select(gr => (gr, Total: gr.Sum(x => x.Qty)));

                    VocOnsiteModel vocOnsite;
                    List<VocOnsiteViewModel> itemOnsites;
                    foreach (var item in lstGroup)
                    {
                        vocOnsite = new VocOnsiteModel()
                        {
                            Customer = item.gr.Key.Customer,
                            Date = item.gr.Key.Date,
                            Week = item.gr.Key.Week,
                            Month = item.gr.Key.Month,
                            Part = item.gr.Key.Part,
                            Qty = item.Total
                        };

                        itemOnsites = new List<VocOnsiteViewModel>();
                        foreach (var onsite in item.gr)
                        {
                            itemOnsites.Add(onsite);
                        }

                        vocOnsite.lstVocOnsite.AddRange(itemOnsites.OrderBy(x => x.ProductionDate));
                        lstVocOnsite.Add(vocOnsite);
                    }
                }
                else
                {
                    var lstGroup = lstOnsite.GroupBy(x => (x.Customer, x.Date, x.Week, x.Month, x.Part, x.Customer_Code, x.Wisol_Model)).Select(gr => (gr, Total: gr.Sum(x => x.Qty)));

                    VocOnsiteModel vocOnsite;
                    List<VocOnsiteViewModel> itemOnsites;
                    foreach (var item in lstGroup)
                    {
                        vocOnsite = new VocOnsiteModel()
                        {
                            Customer = item.gr.Key.Customer,
                            Date = item.gr.Key.Date,
                            Week = item.gr.Key.Week,
                            Month = item.gr.Key.Month,
                            Part = item.gr.Key.Part,
                            Qty = item.Total,
                            Wisol_Model = item.gr.Key.Wisol_Model,
                            Customer_Code = item.gr.Key.Customer_Code,
                            OK = 0,
                            NG = 0
                        };

                        itemOnsites = new List<VocOnsiteViewModel>();
                        foreach (var onsite in item.gr)
                        {
                            itemOnsites.Add(onsite);
                            if (onsite.Result == "OK")
                            {
                                vocOnsite.OK += onsite.Qty;
                            }
                            else if (onsite.Result == "NG")
                            {
                                vocOnsite.NG += onsite.Qty;
                            }
                        }

                        vocOnsite.lstVocOnsite.AddRange(itemOnsites.OrderBy(x => x.ProductionDate));
                        lstVocOnsite.Add(vocOnsite);
                    }
                }
            }
            return lstVocOnsite.OrderBy(x => x.Date).ToList();
        }

        public VocOnsiteViewModel UpdateVocOnsite(VocOnsiteViewModel vm)
        {
            VOC_ONSITE entity = _mapper.Map<VOC_ONSITE>(vm);
            entity.UserModified = GetUserId();
            entity.Month = DateTime.Parse(vm.Date).Month;
            int w = (DateTime.Parse(vm.Date).GetWeekOfYear() - 1);
            entity.Week = "W" + (w <= 9 ? "0" + w : "" + w);

            if (entity.Result == "OK")
            {
                entity.OK = "OK";
                entity.NG = "";
                entity.Not_Measure = "";
            }
            else if (entity.Result == "NG")
            {
                entity.NG = "NG";
                entity.OK = "";
                entity.Not_Measure = "";
            }
            else
            {
                entity.Not_Measure = "NM";
                entity.NG = "";
                entity.OK = "";
            }

            _vocRepository.Update(entity);
            return vm;
        }

        public VocOnsiteViewModel AddVocOnsite(VocOnsiteViewModel vm)
        {
            VOC_ONSITE entity = _mapper.Map<VOC_ONSITE>(vm);
            entity.UserModified = GetUserId();
            entity.UserCreated = GetUserId();
            entity.Month = DateTime.Parse(vm.Date).Month;
            int w = (DateTime.Parse(vm.Date).GetWeekOfYear() - 1);
            entity.Week = "W" + (w <= 9 ? "0" + w : "" + w);

            if (entity.Result == "OK")
            {
                entity.OK = "OK";
                entity.NG = "";
                entity.Not_Measure = "";
            }
            else if (entity.Result == "NG")
            {
                entity.NG = "NG";
                entity.OK = "";
                entity.Not_Measure = "";
            }
            else
            {
                entity.Not_Measure = "NM";
                entity.NG = "";
                entity.OK = "";
            }

            _vocRepository.Add(entity);
            return vm;
        }

        public void DeleteVocOnsite(int id)
        {
            _vocRepository.Remove(id);
        }

        public VocOnsiteViewModel FindById(int id)
        {
            return _mapper.Map<VocOnsiteViewModel>(_vocRepository.FindById(id));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
