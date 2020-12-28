using System;

namespace RestfullControllers.Core.Exceptions
{
    public class EntityWithoutIdException<TEntity> : Exception
    {
        public EntityWithoutIdException(TEntity entity) 
            : base($"{entity.GetType().Name} must have an IdAttribute") { }
    }
}