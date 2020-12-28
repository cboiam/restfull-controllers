using System.Linq;
using System.Reflection;
using RestfullControllers.Core.Attributes;
using RestfullControllers.Core.Exceptions;

namespace RestfullControllers.Core.Extensions
{
    public static class GetEntityIdExtension
    {
        public static object GetEntityId<TEntity>(this TEntity entity)
            where TEntity : class
        {
            var ids = entity.GetType().GetProperties()
                .Where(p => p.GetCustomAttribute<IdAttribute>() != null);

            if(!ids.Any()) throw new EntityWithoutIdException<TEntity>(entity);
            if(ids.Count() > 1) throw new EntityWithMultipleIdsException<TEntity>(entity);

            return ids.First().GetValue(entity);
        }
    }
}