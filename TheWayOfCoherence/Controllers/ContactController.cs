using Application.Features.Contact.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TheWayOfCoherence.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContactController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] SubmitContactMessageRequest request, CancellationToken ct)
        {
            await _mediator.Send(new SubmitContactMessageCommand(
                request.Name,
                request.Email,
                request.Subject,
                request.Message), ct);

            return Ok();
        }

        public sealed class SubmitContactMessageRequest
        {
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Subject { get; set; } = string.Empty;
            public string Message { get; set; } = string.Empty;
        }
    }
}
