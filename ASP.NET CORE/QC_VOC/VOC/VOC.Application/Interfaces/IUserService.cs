using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VOC.Application.ViewModels.System;
using VOC.Utilities.Dtos;

namespace VOC.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> AddAsync(AppUserViewModel userVm);

        Task DeleteAsync(string id);

        List<AppUserViewModel> GetAllAsync();

        PagedResult<AppUserViewModel> GetAllPagingAsync(string keyword, int page, int pageSize);

        Task<AppUserViewModel> GetById(string id);


        Task UpdateAsync(AppUserViewModel userVm);
    }
}
