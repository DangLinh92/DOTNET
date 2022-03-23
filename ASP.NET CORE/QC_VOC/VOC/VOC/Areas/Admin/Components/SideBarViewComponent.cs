﻿using VOC.Application.Interfaces;
using VOC.Application.ViewModels.System;
using VOC.Utilities.Constants;
using VOC.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace VOC.Areas.Admin.Components
{
    public class SideBarViewComponent: ViewComponent
    {
        private IFunctionService _functionService;
        public SideBarViewComponent(IFunctionService functionService)
        {
            _functionService = functionService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var roles = ((ClaimsPrincipal)User).GetSpecificClaim("Roles");
            List<FunctionViewModel> functions;
            if (roles.Split(";").Contains(CommonConstants.AppRole.AdminRole))
            {
                functions =  _functionService.GetAll(string.Empty);
            }
            else
            {
                //TODO: Get by permission
                functions =  _functionService.GetAll(string.Empty);
            }
            return View(functions);
        }
    }
}
