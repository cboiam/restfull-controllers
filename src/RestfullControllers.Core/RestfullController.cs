using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestfullControllers.Core.Exceptions;
using RestfullControllers.Core.Extensions;
using RestfullControllers.Core.Responses;

namespace RestfullControllers.Core
{
    [ApiController]
    public abstract class RestfullController<TEntity> : ControllerBase
        where TEntity : HateoasResponse
    {
        private readonly IResponseMapper<TEntity> responseMapper;

        protected RestfullController(IResponseMapper<TEntity> responseMapper)
        {
            this.responseMapper = responseMapper;
        }

        private static readonly int[] errorStatusCodes = Enum.GetValues<HttpStatusCode>()
                .Select(e => e.GetHashCode())
                .Where(e => e >= StatusCodes.Status400BadRequest)
                .ToArray();

        public IActionResult HandleGet(TEntity entity)
        {
            if (entity == null)
                return NotFound();
            
            var response = responseMapper.MapResponse(entity);
            return Ok(response);
        }

        public IActionResult HandleGet(IEnumerable<TEntity> entities)
        {
            var response = responseMapper.MapResponse(entities);
            return Ok(response);
        }

        public IActionResult HandleCreate(TEntity entity)
        {
            var response = responseMapper.MapResponse(entity);
            return Created($"{Request.Path}/{entity.GetEntityId()}", response);
        }

        public IActionResult HandleUpdate() => NoContent();

        public IActionResult HandleDelete(TEntity entity)
        {
            if (entity == null)
                return NotFound();

            var response = responseMapper.MapResponse(entity);
            return Ok(response);
        }

        public IActionResult HandleError(int statusCode, ValidationProblemDetails problemDetails)
        {
            if (!errorStatusCodes.Contains(statusCode))
            {
                throw new InvalidStatusCodeException(statusCode);
            }
            return StatusCode(statusCode, problemDetails);
        }
    }
}
