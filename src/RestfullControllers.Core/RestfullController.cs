using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestfullControllers.Core.Exceptions;
using RestfullControllers.Core.Extensions;

namespace RestfullControllers.Core
{
    [ApiController]
    public abstract class RestfullController<TEntity> : ControllerBase
        where TEntity : class
    {
        public IActionResult HandleGet(TEntity entity)
        {
            if (entity == null)
                return NotFound();
            return Ok(entity);
        }

        public IActionResult HandleGet(IEnumerable<TEntity> entity) => Ok(entity);

        public IActionResult HandleCreate(TEntity entity)
        {
            return Created($"{Request.Path}/{entity.GetEntityId()}", entity);
        }

        public IActionResult HandleUpdate() => NoContent();

        public IActionResult HandleDelete(TEntity entity)
        {
            if (entity == null)
                return NotFound();
            return Ok(entity);
        }

        public IActionResult HandleError(int statusCode, ValidationProblemDetails problemDetails)
        {
            var errorStatusCodes = Enum.GetValues<HttpStatusCode>()
                .Select(e => e.GetHashCode())
                .Where(e => e >= StatusCodes.Status400BadRequest);

            if (!errorStatusCodes.Contains(statusCode))
            {
                throw new InvalidStatusCodeException(statusCode);
            }
            return StatusCode(statusCode, problemDetails);
        }
    }
}
