using Castle.DynamicProxy;
using Newtonsoft.Json;
using OpenTracing;
using Stefanini.Tracing.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stefanini.Tracing.Interceptors
{
    public class TraceInterceptor : IInterceptor
    {

        private readonly ITracer _tracer;
        private readonly ISharedTracing _sharedTracing;

        public TraceInterceptor(ITracer tracer, ISharedTracing sharedTracing)
        {
            this._tracer = tracer;
            this._sharedTracing = sharedTracing;
        }

        public void Intercept(IInvocation invocation)
        {
            using (IScope scope = this._sharedTracing.CorrelationId == null ? this._tracer.BuildSpan("TraceInterceptor").StartActive(true) : TracingExtension.StartServerSpan(_tracer, this._sharedTracing.CorrelationId, "TraceInterceptor"))
            {

                try
                {
                    scope.Span.SetTag("arguments", JsonConvert.SerializeObject(invocation.Arguments));
                    scope.Span.Log($"Classe {invocation.TargetType.FullName} executando o metodo {invocation.Method.Name}.");
                    invocation.Proceed();
                    scope.Span.Log($"Classe {invocation.TargetType.FullName} executado o metodo {invocation.Method.Name}.");
                }
                catch (Exception)
                {
                    scope.Span.Log($"Erro com a classe {invocation.TargetType.FullName} executado o metodo {invocation.Method.Name}.");
                    throw;
                }
            }
        }
    }
}
