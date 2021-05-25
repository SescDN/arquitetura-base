using System;
using System.Collections.Generic;
using System.Text;

namespace Stefanini.Repository
{
    public interface IBaseContext
    {
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
