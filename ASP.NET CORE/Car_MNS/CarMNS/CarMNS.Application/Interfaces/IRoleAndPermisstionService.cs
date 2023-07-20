using CarMNS.Application.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarMNS.Application.Interfaces
{
    public interface IRoleAndPermisstionService : IDisposable
    {
        // Role
        Task AddRole(RoleViewModel role);
        Task UpdateRole(RoleViewModel role);
        Task DeleteRole(string id);
        void SaveRole();
        List<RoleViewModel> GetAllRole(string filter);

        Task<bool> CheckPermission(string functionId, string action, string[] roles);

        // permisstion
        void AddPermisstion(PermisstionViewModel permisstion);
        void UpdatePermisstion(PermisstionViewModel permisstion);
        void DeletePermisstion(int id);
        void SavePermisstion();
        List<PermisstionViewModel> GetAllPermisstion(string filter = "");
    }
}
