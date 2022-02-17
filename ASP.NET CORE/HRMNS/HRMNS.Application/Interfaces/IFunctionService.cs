using HRMNS.Application.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMNS.Application.Interfaces
{
    public interface IFunctionService : IDisposable
    {
        void Add(FunctionViewModel function);

        List<FunctionViewModel> GetAll(string filter);

        Task<List<FunctionViewModel>> GetAllByPermission(Guid userId);

        IEnumerable<FunctionViewModel> GetAllWithParentId(string parentId);

        FunctionViewModel GetById(string id);

        void Update(FunctionViewModel function);

        void Delete(string id);

        void Save();

        bool CheckExistedId(string id);

        void UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items);

        void ReOrder(string sourceId, string targetId);
    }
}
