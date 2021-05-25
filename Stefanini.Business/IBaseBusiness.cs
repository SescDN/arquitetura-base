using Stefanini.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stefanini.Business
{
    public interface IBaseBusiness<T> where T : BaseEntity
    {
    }
}
