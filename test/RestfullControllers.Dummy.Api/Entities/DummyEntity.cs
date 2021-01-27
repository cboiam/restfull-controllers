using System.Collections.Generic;
using RestfullControllers.Core.Attributes;
using RestfullControllers.Core.Responses;

namespace RestfullControllers.Dummy.Api.Entities
{
    public class DummyEntity : HateoasResponse
    {
        [Id]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public Person Person { get; set; }
        public IEnumerable<Person> People { get; set; }
    }
}