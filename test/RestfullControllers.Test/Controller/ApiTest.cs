using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using RestfullControllers.Dummy.Api;
using RestfullControllers.Dummy.Api.Entities;
using Xunit;

namespace RestfullControllers.Test.Controller
{
    public class ApiTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        protected readonly WebApplicationFactory<Startup> api;

        public ApiTest(WebApplicationFactory<Startup> api)
        {
            this.api = api;
        }

        protected WebApplicationFactory<Startup> Mock(DummyEntity entity)
        {
            return api.WithWebHostBuilder(a => a.ConfigureServices(s =>
            {
                s.Add(new ServiceDescriptor(typeof(IEnumerable<DummyEntity>), new List<DummyEntity>
                {
                    Copy(entity)
                }));
            }));
        }

        protected WebApplicationFactory<Startup> Mock(List<DummyEntity> entities)
        {
            return api.WithWebHostBuilder(a => a.ConfigureServices(s =>
            {
                s.Add(new ServiceDescriptor(typeof(IEnumerable<DummyEntity>), 
                    entities.Select(Copy)));
            }));
        }

        protected WebApplicationFactory<Startup> Mock(ValidationProblemDetails error)
        {
            return api.WithWebHostBuilder(a => a.ConfigureServices(s =>
            {
                s.Add(new ServiceDescriptor(typeof(ValidationProblemDetails), error));
            }));
        }

        private DummyEntity Copy(DummyEntity entity)
        {
            return new DummyEntity
            {
                Id = entity.Id,
                Name = entity.Name,
                Active = entity.Active,
                Person = new Person
                {
                    DocumentNumber = entity.Person.DocumentNumber,
                    Name = entity.Person.Name,
                    Birth = entity.Person.Birth
                }
            };
        }
    }
}