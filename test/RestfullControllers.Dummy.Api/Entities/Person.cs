using System;
using RestfullControllers.Core.Attributes;
using RestfullControllers.Core.Responses;

namespace RestfullControllers.Dummy.Api.Entities
{
    public class Person : HateoasResponse
    {
        [Id]
        [RouteParameter(Name = "id")]
        public string DocumentNumber { get; set; }
        public string Name { get; set; }
        public DateTime Birth { get; set; }
    }
}