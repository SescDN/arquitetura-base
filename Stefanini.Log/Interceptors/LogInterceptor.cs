using Castle.DynamicProxy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Hosting;

namespace Stefanini.Log.Interceptors
{
    public class LogInterceptor : IInterceptor
    {
        private readonly ILogger<LogInterceptor> _logger;
        // private readonly IWebHostEnvironment _env;

        public LogInterceptor(ILogger<LogInterceptor> logger)
        {
            this._logger = logger;
            // this._env = env;
        }

        public void Intercept(IInvocation invocation)
        {
            try
            {
                this._logger.LogInformation("Classe {nomeClasse} executando o metodo {nomeMetodo} com os argumentos {argumentos}", invocation.TargetType.FullName, invocation.Method.Name, invocation.Arguments);
                invocation.Proceed();
                this._logger.LogInformation("Classe {nomeClasse} executado o metodo {nomeMetodo} com os argumentos {argumentos}", invocation.TargetType.FullName, invocation.Method.Name, invocation.Arguments);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erro com a classe {nomeClasse} executado o metodo {nomeMetodo} com os argumentos {argumentos}", invocation.TargetType.FullName, invocation.Method.Name, invocation.Arguments);

                //if (this._env.IsDevelopment())
                throw ex;
            }
        }
    }
}
