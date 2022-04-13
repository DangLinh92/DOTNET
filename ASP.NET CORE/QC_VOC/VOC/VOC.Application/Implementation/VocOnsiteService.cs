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

        public List<VocOnsiteList> SumDataOnsite(int year, string timeWeek)
        {
            List<VOC_ONSITE> lstOnsite;
            List<VocOnsiteList> lstVocOnsite = new List<VocOnsiteList>();
            if (year > 0)
            {
                lstOnsite = _vocRepository.FindAll(x => DateTime.Parse(x.Date).Year == year).ToList();
                var lstGroup = lstOnsite.GroupBy(x => (x.Month, x.Customer, x.Part)).Select(gr => (gr, Total: gr.Count()));

                VocOnsiteList vocOnsite;
                VocOnsiteModel onsiteModel;
                foreach (var group in lstGroup)
                {
                    vocOnsite = new VocOnsiteList();
                    vocOnsite.Customer = group.gr.Key.Customer;

                    onsiteModel = new VocOnsiteModel();
                    onsiteModel.Time = group.gr.Key.Month.NullString();
                    onsiteModel.Part = group.gr.Key.Part;
                    onsiteModel.Customer = group.gr.Key.Customer;

                    foreach (var item in group.gr)
                    {
                        if (!vocOnsite.Parts.Contains(item.Part) && string.IsNullOrEmpty(item.Part))
                        {
                            vocOnsite.Parts.Add(item.Part);
                        }

                        if (item.OK.NullString() != "")
                        {
                            onsiteModel.OK += item.Qty;
                        }

                        if (item.NG.NullString() != "")
                        {
                            onsiteModel.NG += item.Qty;
                        }

                        if (item.Not_Measure.NullString() != "")
                        {
                            onsiteModel.NM += item.Qty;
                        }
                        
                    }
                }
            }
            else
            {

            }
            return lstVocOnsite;
        }
    }
}
