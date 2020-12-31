using System.Collections.Generic;

namespace RestfullControllers.Core.Responses
{
    public abstract class HateoasResponse
    {
        public IEnumerable<Link> Links { get; set; }
    }
}