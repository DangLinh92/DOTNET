using AutoMapper;
using VOC.Application.ViewModels.System;
using VOC.Data.Entities;
using VOC.Application.ViewModels.VOC;

namespace VOC.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<FUNCTION, FunctionViewModel>();
            CreateMap<VOC_DEFECT_TYPE, VOC_DefectTypeViewModel>();
            CreateMap<VOC_MST, VOC_MSTViewModel>();
            CreateMap<VOC_PPM, VocPPMViewModel>();
            CreateMap<VOC_PPM_YEAR, VocPPMYearViewModel>();
        }
    }
}
