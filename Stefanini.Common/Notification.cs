using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stefanini.Common
{
    public class Notification : INotification
    {

        private ICollection<Exception> _exceptions;
        private ICollection<ValidationFailure> _failures;

        public Notification()
        {
            this._exceptions = new List<Exception>();
            this._failures = new List<ValidationFailure>();
        }

        public void AddException(Exception exception)
        {
            this._exceptions.Add(exception);
        }

        public void AddFailures(IEnumerable<ValidationFailure> validationFailures)
        {
            foreach (var failure in validationFailures)
            {
                this._failures.Add(failure);
            }
        }

        public void AddFailure(ValidationFailure validationFailure)
        {
            this._failures.Add(validationFailure);
        }

        public IEnumerable<Exception> GetException()
        {
            return this._exceptions;
        }

        public IEnumerable<ValidationFailure> GetFailures()
        {
            return this._failures;
        }
    }
}
