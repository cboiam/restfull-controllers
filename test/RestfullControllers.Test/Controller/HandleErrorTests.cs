using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using RestfullControllers.Core.Exceptions;
using RestfullControllers.Dummy.Api;
using RestfullControllers.Dummy.Api.Controllers;
using Xunit;

namespace RestfullControllers.Test.Controller
{
    public class HandleErrorTests : ApiTest
    {
        public HandleErrorTests(WebApplicationFactory<Startup> api) : base(api)
        {
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.Forbidden)]
        [InlineData(HttpStatusCode.PreconditionFailed)]
        public async Task HandleError_ShouldReturnProblemDetailsWithSpecifiedStatus(HttpStatusCode statusCode)
        {
            var errors = new Dictionary<string, string[]>
            {
                { "genericError", new string[] { "error1", "error2", "error3" } }
            };
            var expectedError = new ValidationProblemDetails(errors)
            {
                Status = statusCode.GetHashCode()
            };
            var client = Mock(expectedError).CreateClient();
            
            var result = await client.GetAsync($"/dummies/error");
            result.StatusCode.Should().Be(statusCode.GetHashCode());
            
            var content = await result.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            content.Should().BeEquivalentTo(expectedError);
        }

        [Theory]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.Created)]
        [InlineData(HttpStatusCode.Moved)]
        [InlineData(HttpStatusCode.Continue)]
        public void HandleError_ShouldThrowException_WhenStatusIsNotOfError(HttpStatusCode statusCode)
        {
            var controller = new DummyController();
            Action action = () => controller.HandleError(statusCode.GetHashCode(), new());
            action.Should().Throw<InvalidStatusCodeException>()
                .WithMessage($"Status {statusCode.GetHashCode()} is not valid for this operation");
        }
    }
}