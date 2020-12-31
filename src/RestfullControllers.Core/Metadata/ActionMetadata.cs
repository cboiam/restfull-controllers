using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Routing;

namespace RestfullControllers.Core.Metadata
{
    public class ActionMetadata
    {
        public MethodInfo Action { get; internal set; }
        public IEnumerable<HttpMethodAttribute> Methods { get; internal set; }
    }
}