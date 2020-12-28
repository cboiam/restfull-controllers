using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using RestfullControllers.Dummy.Api;
using Xunit;

namespace RestfullControllers.Test
{
    public class HandleUpdateTests : ApiTest
    {
        public HandleUpdateTests(WebApplicationFactory<Startup> api) : base(api)
        {
        }

        [Fact]
        public async Task HandlePut_ShouldReturnNoContent()
        {
            var entity = new DummyEntityFaker().Generate();
            var client = api.CreateClient();

            var result = await client.PutAsJsonAsync("/dummies", entity);
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task HandlePatch_ShouldReturnNoContent()
        {
            var entity = new DummyEntityFaker().Generate();
            var client = api.CreateClient();

            var result = await client.PatchAsync("/dummies", JsonContent.Create(entity));
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }
    }
}