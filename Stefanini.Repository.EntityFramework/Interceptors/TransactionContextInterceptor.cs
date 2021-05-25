using System;
using System.Collections.Generic;
using System.Text;
using Castle.DynamicProxy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;

namespace Stefanini.Repository.EntityFramework.Interceptors
{
    public class TransactionContextInterceptor : IInterceptor
    {
        private readonly DbContext _context;

        public TransactionContextInterceptor(DbContext context)
        {
            this._context = context;
        }

        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.GetCustomAttributes(typeof(TransactionalAttribute), true).Length > 0)
            {
                using (IDbContextTransaction transaction = this._context.Database.BeginTransaction())
                {
                    try
                    {
                        invocation.Proceed();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }

                    transaction.Commit();
                }
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}
