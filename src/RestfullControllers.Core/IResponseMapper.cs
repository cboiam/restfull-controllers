using System.Collections.Generic;
using RestfullControllers.Core.Responses;

namespace RestfullControllers.Core
{
    public interface IResponseMapper<TEntity> where TEntity : HateoasResponse
    {
        Response<TEntity> MapResponse(TEntity entity = null);
        Response<IEnumerable<TEntity>> MapResponse(IEnumerable<TEntity> entities);
    }
}