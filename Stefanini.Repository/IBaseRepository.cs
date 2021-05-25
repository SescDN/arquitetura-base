using Stefanini.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Stefanini.Repository
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        void Insert(T entity);
        void Update(T entity);
        T GetById(int id);
        void Delete(int id);
        ICollection<T> GetByFilter(Expression<Func<T, bool>> filter);
        ICollection<T> GetAll();
    }
}
