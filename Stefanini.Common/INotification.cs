using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stefanini.Common
{
    public interface INotification
    {
        void AddException(Exception exception);
        void AddFailures(IEnumerable<ValidationFailure> validationFailures);
        void AddFailure(ValidationFailure validationFailure);
        IEnumerable<Exception> GetException();
        IEnumerable<ValidationFailure> GetFailures();
    }
}
