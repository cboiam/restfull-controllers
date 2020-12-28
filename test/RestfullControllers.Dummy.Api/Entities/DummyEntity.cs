using RestfullControllers.Core.Attributes;

namespace RestfullControllers.Dummy.Api.Entities
{
    public class DummyEntity
    {
        [Id]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}