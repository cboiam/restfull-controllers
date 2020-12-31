using Microsoft.AspNetCore.Mvc;
using RestfullControllers.Core;
using RestfullControllers.Dummy.Api.Entities;

namespace RestfullControllers.Dummy.Api.Controllers
{
    [Route("people")]
    public class PersonController : RestfullController<Person>
    {
        public PersonController(IResponseMapper<Person> responseMapper) : base(responseMapper)
        {
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return Ok();
        }
    }
}