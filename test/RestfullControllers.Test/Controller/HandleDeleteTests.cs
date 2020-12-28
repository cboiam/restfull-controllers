using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using RestfullControllers.Dummy.Api;
using RestfullControllers.Dummy.Api.Entities;
using Xunit;

namespace RestfullControllers.Test.Controller
{
    public class HandleDeleteTests : ApiTest
    {
        public HandleDeleteTests(WebApplicationFactory<Startup> api) : base(api)
        {
        }

        [Fact]
        public async Task HandleDelete_ShouldReturnDeletedEntity()
        {
            var expectedResult = new DummyEntityFaker().Generate();
            var client = Mock(expectedResult).CreateClient();

            var result = await client.DeleteAsync($"/dummies/{expectedResult.Id}");
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var content = await result.Content.ReadFromJsonAsync<DummyEntity>();
            content.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task HandleDelete_ShouldReturnNotFound_WhenEntityNotRegistered()
        {
            var client = Mock(new List<DummyEntity>()).CreateClient();

            var result = await client.DeleteAsync($"/dummies/1");
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }
}