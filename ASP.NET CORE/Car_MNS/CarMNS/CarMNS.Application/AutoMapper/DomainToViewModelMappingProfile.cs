using AutoMapper;
using CarMNS.Application.ViewModels.System;
using CarMNS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarMNS.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<FUNCTION, FunctionViewModel>();
            CreateMap<APP_ROLE, RoleViewModel>();
            CreateMap<PERMISSION, PermisstionViewModel>();
        }
    }
}
