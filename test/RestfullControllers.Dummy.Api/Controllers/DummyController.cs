using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RestfullControllers.Core;
using RestfullControllers.Dummy.Api.Entities;

namespace RestfullControllers.Dummy.Api.Controllers
{
    [Route("dummies")]
    public class DummyController : RestfullController<DummyEntity>
    {
        [HttpGet]
        public IActionResult Get([FromServices]IEnumerable<DummyEntity> results) => 
            HandleGet(results);

        [HttpGet("{id}")]
        public IActionResult Get(int id, [FromServices]IEnumerable<DummyEntity> results) => 
            HandleGet(results.FirstOrDefault(r => r.Id == id));

        [HttpPost]
        public IActionResult Create(DummyEntity dummy, [FromServices]IEnumerable<DummyEntity> results) => 
            HandleCreate(results.FirstOrDefault(r => r.Name == dummy.Name));

        [HttpPut]
        [HttpPatch]
        public IActionResult Update(DummyEntity dummy) => 
            HandleUpdate();

        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromServices]IEnumerable<DummyEntity> results) => 
            HandleDelete(results.FirstOrDefault(r => r.Id == id));

        [HttpGet("error")]
        public IActionResult Error([FromServices]ValidationProblemDetails error) => 
            HandleError(error.Status.GetValueOrDefault(), error);
    }
}