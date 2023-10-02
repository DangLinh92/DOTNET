using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPERATION_MNS.Areas.OpeationMns.Controllers;
using OPERATION_MNS.Application.Implementation;


namespace OPERATION_MNS.Areas.OpeationMns.Models
{
    public class HomeController : AdminBaseController
    {
        private IRoleAndPermisstionService _roleAndPermisstionService;
        private IFunctionService _functionService;
        private ISCPService _SCPService;

        public HomeController(IRoleAndPermisstionService service, IFunctionService function, ISCPService SCPService)
        {
            _SCPService = SCPService;
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
                if (functions.Any(x => x.Id == item.FunctionId))
                {
                    model.Functions.Add(functions.First(x => x.Id == item.FunctionId));
                }
            }
            return model;
        }
    }
}
