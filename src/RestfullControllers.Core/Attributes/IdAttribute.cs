using System;

namespace RestfullControllers.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IdAttribute : Attribute { }
}