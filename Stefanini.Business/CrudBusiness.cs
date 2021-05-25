using FluentValidation;
using FluentValidation.Results;
using Stefanini.Common;
using Stefanini.Domain.Entity;
using Stefanini.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Stefanini.Business
{
    public class CrudBusiness<T> : BaseBusiness<T>, ICrudBusiness<T> where T : BaseEntity
    {
        public CrudBusiness(IBaseRepository<T> repository,
                      IValidator<T> validation,
                      INotification notification)
            : base(repository,
                validation,
                notification)
        { }

        public void Delete(int id)
        {
            this._repository.Delete(id);
        }

        public IEnumerable<T> GetAll()
        {
            return this._repository.GetAll();
        }

        public T GetById(int id)
        {
            return this._repository.GetById(id);
        }

        public void Insert(T entity)
        {
            this.ValidateExecute(this._repository.Insert, "Insert", entity);
        }

        public void Update(T entity)
        {
            this.ValidateExecute(this._repository.Update, "Update", entity);
        }

        public void ValidateExecute(Action<T> func, string ruleSetName, T entity)
        {
            ValidationResult result = this._validation.Validate(entity, ruleSet: ruleSetName);
            if (result.IsValid)
            {
                func.Invoke(entity);
            }
            else
            {
                this._notification.AddFailures(result.Errors);
            }
        }
    }
}
