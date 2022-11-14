using AutoMapper;
using OPERATION_MNS.Application.ViewModels.System;
using OPERATION_MNS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {

            CreateMap<RoleViewModel, APP_ROLE>()
          .ConstructUsing(c => new APP_ROLE(c.Name, c.Description));

            CreateMap<PermisstionViewModel, PERMISSION>()
         .ConstructUsing(c => new PERMISSION(c.Id, c.RoleId, c.FunctionId, c.CanCreate, c.CanRead, c.CanUpdate, c.CanDelete, c.ApproveL1, c.ApproveL2, c.ApproveL3));


        }
    }
}
