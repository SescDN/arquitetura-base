using System;
using System.Collections.Generic;
using System.Text;

namespace Stefanini.Tracing
{
    public interface ISharedTracing
    {
        IDictionary<string, string> CorrelationId { get; set; }
    }
}
