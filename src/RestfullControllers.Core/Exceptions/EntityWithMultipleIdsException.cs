using System;

namespace RestfullControllers.Core.Exceptions
{
    public class EntityWithMultipleIdsException : Exception
    {
        public EntityWithMultipleIdsException(string entity) 
            : base($"{entity} must not have more than one IdAttribute") { }
    }
}