using System;

namespace RestfullControllers.Core.Exceptions
{
    public class InvalidStatusCodeException : Exception
    {
        public InvalidStatusCodeException(int statusCode)
            : base($"Status {statusCode} is not valid for this operation") { }
    }
}