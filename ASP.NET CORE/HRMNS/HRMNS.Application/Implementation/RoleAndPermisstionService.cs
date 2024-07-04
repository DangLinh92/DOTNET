using AutoMapper;
using AutoMapper.QueryableExtensions;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.System;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMNS.Data.Enums;
using HRMNS.Data.IRepositories;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMNS.Application.Implementation
{
    public class RoleAndPermisstionService : IRoleAndPermisstionService
    {
        private IUnitOfWork _unitOfWork;
        private RoleManager<APP_ROLE> _roleManager;
        IRespository<PERMISSION, int> _PermisstionRepository;
        private IRespository<FUNCTION, string> _functionRepository;
        private readonly IMapper _mapper;

        public RoleAndPermisstionService(RoleManager<APP_ROLE> roleManager, IRespository<PERMISSION, int> permisstionRepository, IRespository<FUNCTION, string> functionRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _roleManager = roleManager;
            _PermisstionRepository = permisstionRepository;
            _functionRepository = functionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task AddRole(RoleViewModel role)
        {
            APP_ROLE appRole = new APP_ROLE()
            {
                Name = role.Name,
                Description = role.Name,
                NormalizedName = role.Name
            };

            bool isExist = await _roleManager.RoleExistsAsync(role.Name);
            if (!isExist)
            {
              var rs = await _roleManager.CreateAsync(appRole);
            }
        }

        public async Task UpdateRole(RoleViewModel role)
        {
            var roleEn = await _roleManager.FindByIdAsync(role.Id.ToString());
            if (roleEn != null)
            {
                roleEn.Name = role.Name;
                roleEn.Description = role.Name;
                roleEn.NormalizedName = role.Name;

                await _roleManager.UpdateAsync(roleEn);
            }
        }

        public async Task DeleteRole(string id)
        {
            var roleEn = await _roleManager.FindByIdAsync(id);
            await _roleManager.DeleteAsync(roleEn);
        }

        public void SaveRole()
        {
            _unitOfWork.Commit();
        }

        public List<RoleViewModel> GetAllRole(string filter)
        {
            return _mapper.Map<List<RoleViewModel>>(_roleManager.Roles);
        }

        public void AddPermisstion(PermisstionViewModel permisstion)
        {
            var entiry1 = _PermisstionRepository.FindById(permisstion.Id);
            if (entiry1 == null)
            {
                var entity = _mapper.Map<PERMISSION>(permisstion);
                _PermisstionRepository.Add(entity);
            }
        }

        public void UpdatePermisstion(PermisstionViewModel permisstion)
        {
            var existObj = _PermisstionRepository.FindById(permisstion.Id);
            existObj.CopyPropertiesFrom(permisstion,new List<string>() { "Id", "AppRole", "Function" });
            if (existObj != null)
            {
                _PermisstionRepository.Update(existObj);
            }
        }

        public void DeletePermisstion(int id)
        {
            var entity = _PermisstionRepository.FindById(id);
            _PermisstionRepository.Remove(entity);
        }

        public void SavePermisstion()
        {
            _unitOfWork.Commit();
        }

        /// <summary>
        /// Get permisstion By Role
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<PermisstionViewModel> GetAllPermisstion(string filter = "")
        {
            if (string.IsNullOrEmpty(filter))
            {
                return _mapper.Map<List<PermisstionViewModel>>(_PermisstionRepository.FindAll());
            }
            else
            {
                return _mapper.Map<List<PermisstionViewModel>>(_PermisstionRepository.FindAll(x => x.RoleId.ToString() == filter,x=>x.Function));
            }
        }

        public Task<bool> CheckPermission(string functionId, string action, string[] roles)
        {
            var functions = _functionRepository.FindAll();
            var permissions = _PermisstionRepository.FindAll();
            var query = from f in functions
                        join p in permissions on f.Id equals p.FunctionId
                        join r in _roleManager.Roles on p.RoleId equals r.Id
                        where roles.Contains(r.Name) && f.Id == functionId
                        && ((p.CanCreate && action == "Create")
                        || (p.CanUpdate && action == "Update")
                        || (p.CanDelete && action == "Delete")
                        || (p.CanRead && action == "Read")
                        || (p.ApproveL1 && action == "ApproveL1")
                        || (p.ApproveL2 && action == "ApproveL2")
                        || (p.ApproveL3 && action == "ApproveL3"))
                        select p;

            return query.AnyAsync();
        }
    }
}
