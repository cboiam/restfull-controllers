using Bogus;
using RestfullControllers.Dummy.Api.Entities;

namespace RestfullControllers.Test
{
    public class DummyEntityFaker : Faker<DummyEntity>
    {
        public DummyEntityFaker()
        {
            AddRule("Id", (faker, entity) => faker.Random.Int(1));
            AddRule("Name", (faker, entity) => faker.Person.FullName);
            AddRule("Active", (faker, entity) => faker.Random.Bool());
        }
    }
}