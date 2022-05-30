using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.System;
using HRMNS.Data.EF.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Areas.Admin.Controllers
{
    public class RoleAndPermisstionController : AdminBaseController
    {
        private IRoleAndPermisstionService _roleAndPermisstionService;
        private IFunctionService _functionService;

        public RoleAndPermisstionController(IRoleAndPermisstionService service, IFunctionService function)
        {
            _roleAndPermisstionService = service;
            _functionService = function;
        }

        public IActionResult Index()
        {
            List<RoleViewModel> roles = _roleAndPermisstionService.GetAllRole(null);
            RoleAndPermisstionViewModel model = new RoleAndPermisstionViewModel();
            model.RoleModels = roles;
            model.Functions = _functionService.GetAll("").Where(x=>x.ParentId != null).ToList();
            string roleId = "";
            if(roles.Count > 1)
            {
                roleId = roles[0].Id.ToString();
            }
            model.PermisstionForRoleModels = _roleAndPermisstionService.GetAllPermisstion(roleId);
            return View(model);
        }

        [HttpPost]
        public IActionResult GetPermisstionByRole(string roleId)
        {
            List<RoleViewModel> roles = _roleAndPermisstionService.GetAllRole(null);
            RoleAndPermisstionViewModel model = new RoleAndPermisstionViewModel();
            model.RoleModels = roles;
            model.RoleActive = roles.FirstOrDefault(x => x.Id + "" == roleId).Name;
            model.Functions = _functionService.GetAll("").Where(x => x.ParentId != null).ToList();
            model.PermisstionForRoleModels = _roleAndPermisstionService.GetAllPermisstion(roleId);
            return View("Index", model);
        }

        [HttpGet]
        public IActionResult GetAllRole()
        {
            List<RoleViewModel> roles = _roleAndPermisstionService.GetAllRole(null);
            RoleAndPermisstionViewModel model = new RoleAndPermisstionViewModel();
            model.RoleModels = roles;
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SavePermission(List<PermisstionViewModel> model)
        {
            foreach (PermisstionViewModel item in model)
            {
                if(item.Id == 0)
                {
                    _roleAndPermisstionService.AddPermisstion(item);
                }
                else
                {
                    _roleAndPermisstionService.UpdatePermisstion(item);
                }
            }

            _roleAndPermisstionService.SavePermisstion();
            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveRole(RoleViewModel role)
        {
            if (role.Id != Guid.Empty)
            {
               await _roleAndPermisstionService.UpdateRole(role);
            }
            else
            {
               await _roleAndPermisstionService.AddRole(role);
            }

            _roleAndPermisstionService.SaveRole();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteRole(string roleId)
        {
            _roleAndPermisstionService.DeleteRole(roleId);
            _roleAndPermisstionService.SaveRole();
            return RedirectToAction("Index");
        }
    }
}
