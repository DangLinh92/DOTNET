using AutoMapper;
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

        }
    }
}
