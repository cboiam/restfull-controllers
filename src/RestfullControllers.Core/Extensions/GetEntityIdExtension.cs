using System.Linq;
using System.Reflection;
using RestfullControllers.Core.Attributes;
using RestfullControllers.Core.Exceptions;

namespace RestfullControllers.Core.Extensions
{
    internal static class GetEntityIdExtension
    {
        public static object GetId<TEntity>(this TEntity entity)
            where TEntity : class
        {
            var ids = entity.GetType().GetProperties()
                .Where(p => p.GetCustomAttribute<IdAttribute>() != null);

            if(!ids.Any()) throw new EntityWithoutIdException(nameof(entity));
            if(ids.Count() > 1) throw new EntityWithMultipleIdsException(nameof(entity));

            return ids.First().GetValue(entity);
        }
    }
}