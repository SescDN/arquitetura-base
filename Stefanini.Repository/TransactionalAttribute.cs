using System;
using System.Collections.Generic;
using System.Text;

namespace Stefanini.Repository
{
    [AttributeUsage(System.AttributeTargets.Method)]
    public class TransactionalAttribute : Attribute
    {
    }
}
