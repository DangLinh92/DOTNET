﻿using VOC.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace VOC.Infrastructure.Interfaces
{
    public interface IRespository<T,K> where T:class
    {
        T FindById(K id, params Expression<Func<T, object>>[] includeProperties);

        T FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        void Add(T entity);

        void Update(T entity);

        void Remove(T entity);

        void Remove(K id);

        void RemoveMultiple(List<T> entities);

        void UpdateRange(List<T> entities);

        ResultDB ExecProceduce(string ProcName, Dictionary<string, string> Dictionary, string tableParam, DataTable table);

        string GetMaxDate(Expression<Func<T, string>> selector);
    }
}
