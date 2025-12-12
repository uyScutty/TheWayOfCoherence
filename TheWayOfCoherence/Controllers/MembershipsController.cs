using Application.Features.Membership.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TheWayOfCoherence.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MembershipsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Signup([FromBody] SignupMembershipRequest request, CancellationToken ct)
        {
            var id = await _mediator.Send(new SignupMembershipCommand(request.UserId), ct);
            return Ok(id);
        }

        public sealed class SignupMembershipRequest
        {
            public Guid UserId { get; set; }
        }
    }
}
