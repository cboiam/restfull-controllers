using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace RestfullControllers.Core.Responses
{
    public class Response<TEntity>
    {
        public IEnumerable<Link> Links { get; set; }
        public TEntity Data { get; set; }
        public ValidationProblemDetails Error { get; set; }
    }
}