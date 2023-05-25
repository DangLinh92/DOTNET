using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.System;
using HRMNS.Data.Enums;
using HRMNS.Utilities.Constants;
using HRMS.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HRMS.Areas.Admin.Components
{
    public class SideBarViewComponent: ViewComponent
    {
        private IFunctionService _functionService;
        private IRoleAndPermisstionService _roleAndPermisstionService;
        private readonly IMemoryCache _memoryCache;

        public SideBarViewComponent(IFunctionService functionService, IRoleAndPermisstionService service, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _functionService = functionService;
            _roleAndPermisstionService = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string key = CacheKeys.MenuLst.ToString()+"-"+ ((ClaimsPrincipal)User).FindFirst(x => x.Type == "Roles").Value.Split(';')[0];
            List<FunctionViewModel> functions = _memoryCache.GetOrCreate(key, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(1);
                return GetPermistionByRole().Functions.OrderBy(x=>x.SortOrder).ToList();
            });
            // var roles = ((ClaimsPrincipal)User).GetSpecificClaim("Roles");
            
            //if (roles.Split(";").Contains(CommonConstants.AppRole.AdminRole))
            //{
            //    functions =  _functionService.GetAll(string.Empty);
            //}
            //else
            //{
            //    //TODO: Get by permission
            //    functions =  _functionService.GetAll(string.Empty);
            //}
            return View(functions);
        }

        private RoleAndPermisstionViewModel GetPermistionByRole()
        {
            string roleName = ((ClaimsPrincipal)User).FindFirst(x => x.Type == "Roles").Value.Split(';')[0];
            List<RoleViewModel> roles = _roleAndPermisstionService.GetAllRole(null);
            string roleId = "";
            roleId = roles.FirstOrDefault(x => x.Name == roleName).Id.ToString();
            RoleAndPermisstionViewModel model = new RoleAndPermisstionViewModel();
            model.RoleModels = roles;
            model.RoleActive = roles.FirstOrDefault(x => x.Id + "" == roleId).Name;
            model.PermisstionForRoleModels = _roleAndPermisstionService.GetAllPermisstion(roleId);

            var functions = _functionService.GetAll("").Where(x=>x.Area.ToLower().Contains("admin")).ToList();
            model.Functions = new List<FunctionViewModel>();

            foreach (var item in model.PermisstionForRoleModels)
            {
                if (functions.Any(x => x.Id == item.FunctionId) && item.CanRead)
                {
                    model.Functions.Add(functions.First(x => x.Id == item.FunctionId));
                }
            }

            if(model.Functions.Count == 0 && roles[0].Name == "Admin")
            {
                model.Functions.AddRange(functions);
            }

            return model;
        }
    }
}
