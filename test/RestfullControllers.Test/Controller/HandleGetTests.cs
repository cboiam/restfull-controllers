using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using RestfullControllers.Core.Responses;
using RestfullControllers.Dummy.Api;
using RestfullControllers.Dummy.Api.Entities;
using RestfullControllers.Test.Fakers;
using Xunit;

namespace RestfullControllers.Test.Controller
{
    public class HandleGetTests : ApiTest
    {
        public HandleGetTests(WebApplicationFactory<Startup> api) : base(api)
        {
        }

        [Fact]
        public async Task HandleGetList_ReturnsOkWithFilledListAsync()
        {
            var expectedResults = new DummyEntityFaker().Generate(3);
            var client = Mock(expectedResults).CreateClient();

            var result = await client.GetFromJsonAsync<Response<List<DummyEntity>>>("/dummies");
            result.Should().BeEquivalentTo(DummyResponse.GetResponse(expectedResults));
        }

        [Fact]
        public async Task HandleGetList_ReturnsOkEvenWithEmptyListAsync()
        {
            var client = Mock(new List<DummyEntity>()).CreateClient();
            var result = await client.GetFromJsonAsync<Response<List<DummyEntity>>>("/dummies");
            result.Should().BeEquivalentTo(DummyResponse.GetResponse(new List<DummyEntity>()));
        }

        [Fact]
        public async Task HandleGet_ReturnsOkWithDummyEntityAsync()
        {
            var expectedResult = new DummyEntityFaker().Generate();
            var client = Mock(expectedResult).CreateClient();

            var result = await client.GetFromJsonAsync<Response<DummyEntity>>($"/dummies/{expectedResult.Id}");
            result.Should().BeEquivalentTo(DummyResponse.GetResponse(expectedResult));
        }

        [Fact]
        public async Task HandleGet_ReturnsNotFound_WhenEntityIsNullAsync()
        {
            var client = Mock(new List<DummyEntity>()).CreateClient();
            var result = await client.GetAsync("/dummies/1");
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }
}