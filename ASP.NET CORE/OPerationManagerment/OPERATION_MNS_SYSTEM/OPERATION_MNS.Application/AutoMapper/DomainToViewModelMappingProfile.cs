using AutoMapper;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Application.ViewModels.System;
using OPERATION_MNS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<FUNCTION, FunctionViewModel>();
            CreateMap<APP_ROLE, RoleViewModel>();
            CreateMap<PERMISSION, PermisstionViewModel>();
            CreateMap<GOC_PLAN, GocPlanViewModel>();
            CreateMap<GOC_STANDAR_QTY, GocPlanViewModel>();
            CreateMap<SETTING_ITEMS, SettingItemsViewModel>();
            CreateMap<DATE_OFF_LINE, DateOffLineViewModel>();
            CreateMap<YIELD_OF_MODEL, YieldOfModelViewModel>();
            CreateMap<MATERIAL_TO_SAP, MaterialToSapViewModel>();
            CreateMap<CTQ_SETTING, CTQSettingViewModel>();
            CreateMap<CTQ_EMAIL_RECEIV, CTQEmailReceivViewModel>();
            CreateMap<POST_OPERATION_SHIPPING, PostOpeationShippingViewModel>();
        }
    }
}
