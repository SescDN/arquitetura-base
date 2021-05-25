using System;
using System.Collections.Generic;
using System.Text;

namespace Stefanini.Application
{
    public interface IBaseService
    {
        T Map<T>(object source);
    }
}
