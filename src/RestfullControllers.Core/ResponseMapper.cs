using System;
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

        public Response<TEntity> MapResponse(TEntity entity = null)
        {
            if (entity != null)
            {
                entity.Links = linkMapper.MapEntityLinks(entity);
                MapNestedLinks(entity);
            }

            return new Response<TEntity>
            {
                Data = entity,
                Links = linkMapper.MapControllerLinks()
            };
        }

        private void MapNestedLinks(TEntity entity)
        {
            var properties = entity.GetType().GetProperties().Where(p =>
                typeof(HateoasResponse).IsAssignableFrom(p.PropertyType) ||
                typeof(IEnumerable<HateoasResponse>).IsAssignableFrom(p.PropertyType)
            );

            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(entity);
                var links = linkMapper.MapSubEntityLinks(propertyValue);
                property.PropertyType.GetProperty("Links").SetValue(propertyValue, links);
            }
        }

        public Response<IEnumerable<TEntity>> MapResponse(IEnumerable<TEntity> entities)
        {
            List<TEntity> data = null;
            if (entities != null)
            {
                data = entities.ToList();
                data.ForEach(entity =>
                {
                    entity.Links = linkMapper.MapEntityLinks(entity);
                    MapNestedLinks(entity);
                });
            }

            return new Response<IEnumerable<TEntity>>
            {
                Data = data,
                Links = linkMapper.MapControllerLinks()
            };
        }
    }
}