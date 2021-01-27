using Bogus;
using RestfullControllers.Dummy.Api.Entities;

namespace RestfullControllers.Test.Fakers
{
    public class DummyEntityFaker : Faker<DummyEntity>
    {
        public DummyEntityFaker()
        {
            AddRule("Id", (faker, entity) => entity.Id = faker.Random.Int(1));
            AddRule("Name", (faker, entity) => entity.Name = faker.Person.FullName);
            AddRule("Active", (faker, entity) => entity.Active = faker.Random.Bool());
            AddRule("Person", (faker, entity) => entity.Person = new PersonFaker().Generate());
            AddRule("People", (faker, entity) => entity.People = new PersonFaker().Generate(3));
        }
    }
}