using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarMNS.Application.Interfaces;
using CarMNS.Application.ViewModels.System;
using CarMNS.Data.EF.Extensions;
using CarMNS.Data.Entities;
using CarMNS.Data.Enums;
using CarMNS.Data.IRepositories;
using CarMNS.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarMNS.Application.Implementation
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
            var entity = _mapper.Map<FUNCTION>(function);
            _functionRepository.Add(entity);
        }

        public bool CheckExistedId(string id)
        {
            return _functionRepository.FindById(id) != null;
        }

        public void Delete(string id)
        {
            var entity = _functionRepository.FindById(id);
            _functionRepository.Remove(entity);
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
            return _mapper.Map<FunctionViewModel>(_functionRepository.FindById(id));
        }

        public void ReOrder(string sourceId, string targetId)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(FunctionViewModel function)
        {
            var entity = _functionRepository.FindById(function.Id);
            entity.CopyPropertiesFrom(function, new List<string>() { "Id" });
            _functionRepository.Update(entity);
        }

        public void UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items)
        {
            throw new NotImplementedException();
        }
    }
}
