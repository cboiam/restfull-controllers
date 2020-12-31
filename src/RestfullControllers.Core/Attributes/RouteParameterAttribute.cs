using System;

namespace RestfullControllers.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RouteParameterAttribute : Attribute
    {
        public string Name { get; set; }
    }
}