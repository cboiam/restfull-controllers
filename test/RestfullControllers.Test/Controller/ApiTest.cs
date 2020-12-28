using System.Collections.Generic;
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
                    entity
                }));
            }));
        }

        protected WebApplicationFactory<Startup> Mock(List<DummyEntity> entities)
        {
            return api.WithWebHostBuilder(a => a.ConfigureServices(s =>
            {
                s.Add(new ServiceDescriptor(typeof(IEnumerable<DummyEntity>), entities));
            }));
        }

        protected WebApplicationFactory<Startup> Mock(ValidationProblemDetails error)
        {
            return api.WithWebHostBuilder(a => a.ConfigureServices(s =>
            {
                s.Add(new ServiceDescriptor(typeof(ValidationProblemDetails), error));
            }));
        }
    }
}