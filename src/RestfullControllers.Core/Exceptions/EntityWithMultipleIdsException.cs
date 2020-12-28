using System;

namespace RestfullControllers.Core.Exceptions
{
    public class EntityWithMultipleIdsException<TEntity> : Exception
    {
        public EntityWithMultipleIdsException(TEntity entity) 
            : base($"{entity.GetType().Name} must not have more than one IdAttribute") { }
    }
}