using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using timeout_sample.Commands;

namespace timeout_sample.Controllers
{
	public class Request
	{
		public string TestValue { get; set; }
	}

	[Route("[controller]")]
	public class TestController(IMediator mediator) : Controller
	{
		[HttpPost]
        [SwaggerResponse(200)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        public async Task<IActionResult> PostAsync([FromBody] Request req)
		{
			var result = await mediator.Send(new CreateTestCommand(req.TestValue));
			if (result.IsFailed) return BadRequest();
			return Ok(result.Value);
		}
	}
}

