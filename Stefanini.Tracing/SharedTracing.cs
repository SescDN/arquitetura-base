using System;
using System.Collections.Generic;
using System.Text;

namespace Stefanini.Tracing
{
    public class SharedTracing : ISharedTracing
    {
        public IDictionary<string, string> CorrelationId { get; set; }
    }
}
