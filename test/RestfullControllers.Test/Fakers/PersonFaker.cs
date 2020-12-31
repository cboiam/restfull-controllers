using System;
using Bogus;
using PersonEntity = RestfullControllers.Dummy.Api.Entities.Person;

namespace RestfullControllers.Test.Fakers
{
    public class PersonFaker : Faker<PersonEntity>
    {
        public PersonFaker()
        {
            AddRule("DocumentNumber", (faker, entity) => Guid.NewGuid().ToString());
            AddRule("Name", (faker, entity) => faker.Person.FirstName);
            AddRule("Birth", (faker, entity) => faker.Date.Past());
        }
    }
}