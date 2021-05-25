using FluentValidation;
using Stefanini.Common;
using Stefanini.Domain.Entity;
using Stefanini.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stefanini.Business
{
    public class BaseBusiness<T> : IBaseBusiness<T> where T : BaseEntity
    {
        protected readonly IBaseRepository<T> _repository;
        protected readonly IValidator<T> _validation;
        protected readonly INotification _notification;

        public BaseBusiness(IBaseRepository<T> repository,
                            IValidator<T> validation,
                            INotification notification)
        {
            this._repository = repository;
            this._validation = validation;
            this._notification = notification;
        }
    }
}
