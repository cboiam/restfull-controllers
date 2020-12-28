using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RestfullControllers.Core.Extensions;

namespace RestfullControllers.Core
{
    [ApiController]
    public abstract class RestfullController<TEntity> : ControllerBase
        where TEntity : class
    {
        public IActionResult HandleGet(TEntity entity)
        {
            if(entity == null)
                return NotFound();
            return Ok(entity);
        }

        public IActionResult HandleGet(IEnumerable<TEntity> entity) => Ok(entity);

        public IActionResult HandleCreate(TEntity entity)
        {
            return Created($"{Request.Path}/{entity.GetId()}", entity);
        }

        public IActionResult HandleUpdate() => NoContent();

        public IActionResult HandleDelete(TEntity entity)
        {
            if(entity == null)
                return NotFound();
            return Ok(entity);
        }

        public IActionResult HandleError(ValidationProblemDetails descriptor) => ValidationProblem(descriptor);
    }
}
