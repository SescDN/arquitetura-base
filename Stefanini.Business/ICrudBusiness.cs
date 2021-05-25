using Stefanini.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Stefanini.Business
{
    public interface ICrudBusiness<T> : IBaseBusiness<T> where T : BaseEntity
    {
        void Delete(int id);
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Insert(T entity);
        void Update(T entity);
    }
}
