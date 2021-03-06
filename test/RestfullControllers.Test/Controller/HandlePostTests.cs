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
    public class HandlePostTests : ApiTest
    {
        public HandlePostTests(WebApplicationFactory<Startup> api) : base(api)
        {
        }

        [Fact]
        public async Task HandleCreate_ShouldReturnCreatedEntityWithLocation()
        {
            var expectedResult = new DummyEntityFaker().Generate();
            var client = Mock(expectedResult).CreateClient();
            
            var result = await client.PostAsJsonAsync("/dummies", expectedResult);
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
            result.Headers.Location.Should().Be($"/dummies/{expectedResult.Id}");
            
            var content = await result.Content.ReadFromJsonAsync<Response<DummyEntity>>();
            content.Should().BeEquivalentTo(DummyResponse.GetResponse(expectedResult));
        }
    }
}