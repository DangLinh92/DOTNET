using AutoMapper;
using VOC.Application.Interfaces;
using VOC.Application.ViewModels.System;
using VOC.Data.Entities;
using VOC.Data.Enums;
using VOC.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VOC.Application.Implementation
{
    public class FunctionService : IFunctionService
    {
        private IUnitOfWork _unitOfWork;
        IRespository<FUNCTION, string> _functionRepository;
        private readonly IMapper _mapper;

        public FunctionService(IRespository<FUNCTION, string> functionRepository,IUnitOfWork unitOfWork, IMapper mapper)
        {
            _functionRepository = functionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void Add(FunctionViewModel function)
        {
            throw new NotImplementedException();
        }

        public bool CheckExistedId(string id)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<FunctionViewModel> GetAll(string filter)
        {
            var query = _functionRepository.FindAll(x => x.Status == Status.Active);
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(x => x.Name.Contains(filter));

            return _mapper.Map<List<FunctionViewModel>>(query.OrderBy(x => x.SortOrder));
        }

        public Task<List<FunctionViewModel>> GetAllByPermission(Guid userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FunctionViewModel> GetAllWithParentId(string parentId)
        {
            throw new NotImplementedException();
        }

        public FunctionViewModel GetById(string id)
        {
            throw new NotImplementedException();
        }

        public void ReOrder(string sourceId, string targetId)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(FunctionViewModel function)
        {
            throw new NotImplementedException();
        }

        public void UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items)
        {
            throw new NotImplementedException();
        }
    }
}
