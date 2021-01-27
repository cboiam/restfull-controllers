using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                MapNestedResponse(entity);
            }

            return new Response<TEntity>
            {
                Data = entity,
                Links = linkMapper.MapControllerLinks()
            };
        }

        private void MapNestedResponse(object entity)
        {
            if (entity == null)
            {
                return;
            }

            MapNestedObject(entity);
            MapNestedList(entity);
        }

        private void MapNestedList(object entity)
        {
            var properties = entity.GetType().GetProperties().Where(p =>
                typeof(IEnumerable<HateoasResponse>).IsAssignableFrom(p.PropertyType));

            foreach (var property in properties)
            {
                var values = property.GetValue(entity) as IEnumerable<HateoasResponse>;
                
                var listType = typeof(List<>);
                var constructedListType = listType.MakeGenericType(property.PropertyType.GenericTypeArguments[0]);
                var newValues = Activator.CreateInstance(constructedListType) as IList;

                foreach (var value in values)
                {
                    SetNestedLinks(value.GetType(), value);
                    MapNestedResponse(value);
                    newValues.Add(value);                    
                }
                property.SetValue(entity, newValues);
            }
        }

        private void MapNestedObject(object entity)
        {
            var properties = entity.GetType().GetProperties().Where(p =>
            typeof(HateoasResponse).IsAssignableFrom(p.PropertyType));

            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(entity);
                SetNestedLinks(property, propertyValue);
            }
        }

        private void SetNestedLinks(Type type, object propertyValue)
        {
            var links = linkMapper.MapSubEntityLinks(propertyValue);
            type.GetProperty("Links").SetValue(propertyValue, links);
        }

        private void SetNestedLinks(PropertyInfo property, object propertyValue)
        {
            SetNestedLinks(property.PropertyType, propertyValue);
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
                    MapNestedResponse(entity);
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