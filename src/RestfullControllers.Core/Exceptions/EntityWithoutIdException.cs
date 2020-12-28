using System;

namespace RestfullControllers.Core.Exceptions
{
    public class EntityWithoutIdException : Exception
    {
        public EntityWithoutIdException(string entity) 
            : base($"{entity} must have an IdAttribute") { }
    }
}