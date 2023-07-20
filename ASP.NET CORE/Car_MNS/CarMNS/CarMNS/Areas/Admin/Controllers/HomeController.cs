using CarMNS.Application.Interfaces;
using CarMNS.Application.ViewModels.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarMNS.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        private IRoleAndPermisstionService _roleAndPermisstionService;
        private IFunctionService _functionService;
        public HomeController(IRoleAndPermisstionService service, IFunctionService function)
        {
            _roleAndPermisstionService = service;
            _functionService = function;
        }

        public IActionResult Index()
        {
            return View();
        }

        private RoleAndPermisstionViewModel GetPermistionByRole()
        {
            string roleName = User.FindFirst(x => x.Type == "Roles").Value.Split(';')[0];
            List<RoleViewModel> roles = _roleAndPermisstionService.GetAllRole(null);
            string roleId = "";
            roleId = roles.FirstOrDefault(x => x.Name == roleName).Id.ToString();
            RoleAndPermisstionViewModel model = new RoleAndPermisstionViewModel();
            model.RoleModels = roles;
            model.RoleActive = roles.FirstOrDefault(x => x.Id + "" == roleId).Name;
            model.PermisstionForRoleModels = _roleAndPermisstionService.GetAllPermisstion(roleId);

            var functions = _functionService.GetAll("").ToList();
            model.Functions = new List<FunctionViewModel>();
            foreach (var item in model.PermisstionForRoleModels)
            {
                if(functions.Any(x=>x.Id == item.FunctionId))
                {
                    model.Functions.Add(functions.First(x => x.Id == item.FunctionId));
                }
            }
            return model;
        }
    }
}
