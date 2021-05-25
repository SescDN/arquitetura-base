using Stefanini.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using Stefanini.Repository;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Stefanini.Repository.EntityFramework
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {

        protected readonly DbContext _db;

        public BaseRepository(DbContext db)
        {
            _db = db;
        }

        public void Delete(int id)
        {
            _db.Set<T>().Remove(_db.Set<T>().Where(o => o.Id == id).SingleOrDefault());
        }

        public ICollection<T> GetAll()
        {
            return _db.Set<T>().ToList();
        }

        public ICollection<T> GetByFilter(Expression<Func<T, bool>> filter)
        {
            return _db.Set<T>().Where(filter).ToList();
        }

        public T GetById(int id)
        {
            return _db.Set<T>().Where(o => o.Id == id).SingleOrDefault();
        }

        public void Insert(T entity)
        {
            _db.Set<T>().Add(entity);
            _db.SaveChanges();
        }

        public void Update(T entity)
        {
            _db.Set<T>().Update(entity);
            _db.SaveChanges();
        }
    }
}
