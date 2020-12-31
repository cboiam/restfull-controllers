using System.Collections.Generic;
using System.Linq;
using RestfullControllers.Core.Responses;

namespace RestfullControllers.Core
{

    public class ResponseMapper<TEntity> : IResponseMapper<TEntity> where TEntity : HateoasResponse
    {
        private readonly ILinkMapper<TEntity> linkMapper;

        public ResponseMapper(ILinkMapper<TEntity> linkMapper)
        {
            this.linkMapper = linkMapper;
        }

        public Response<TEntity> MapResponse(TEntity entity)
        {
            if (entity != null)
            {
                entity.Links = linkMapper.MapEntityLinks(entity);
            }

            // TODO: get members that can have links AKA inherits from HateoasResponse and build their links as well
            // entity.GetType().IsAssignableTo(typeof(HateoasResponse))
            // entity.GetType().IsAssignableTo(typeof(IEnumerable<HateoasResponse>))

            return new Response<TEntity>
            {
                Data = entity,
                Links = linkMapper.MapControllerLinks()
            };
        }

        public Response<IEnumerable<TEntity>> MapResponse(IEnumerable<TEntity> entities)
        {
            List<TEntity> data = null;
            if (entities != null)
            {
                data = entities.ToList();
                data.ForEach(entity => entity.Links = linkMapper.MapEntityLinks(entity));
            }

            return new Response<IEnumerable<TEntity>>
            {
                Data = data,
                Links = linkMapper.MapControllerLinks()
            };
        }
    }
}