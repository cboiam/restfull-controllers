using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using RestfullControllers.Dummy.Api;
using RestfullControllers.Dummy.Api.Entities;
using Xunit;

namespace RestfullControllers.Test
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

            var result = await client.GetFromJsonAsync<List<DummyEntity>>("/dummies");
            result.Should().BeEquivalentTo(expectedResults);
        }

        [Fact]
        public async Task HandleGetList_ReturnsOkEvenWithEmptyListAsync()
        {
            var client = Mock(new List<DummyEntity>()).CreateClient();
            var result = await client.GetFromJsonAsync<List<DummyEntity>>("/dummies");
            result.Should().NotBeNull()
                .And.BeEmpty();
        }

        [Fact]
        public async Task HandleGet_ReturnsOkWithDummyEntityAsync()
        {
            var expectedResult = new DummyEntityFaker().Generate();
            var client = Mock(expectedResult).CreateClient();
            
            var result = await client.GetFromJsonAsync<DummyEntity>($"/dummies/{expectedResult.Id}");
            result.Should().BeEquivalentTo(expectedResult);
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