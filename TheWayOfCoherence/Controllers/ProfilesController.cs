using Application.Features.UserProfiles.Commands;
using Application.Features.UserProfiles.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TheWayOfCoherence.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfilesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfilesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateProfileRequest request, CancellationToken ct)
        {
            var profileId = await _mediator.Send(new UserProfileSignupCommand(
                request.UserId,
                request.Age,
                request.Gender,
                request.HealthNote), ct);

            return Ok(profileId);
        }

        [HttpPut("{userId:guid}")]
        public async Task<ActionResult<UserProfileDto>> Update(Guid userId, [FromBody] UpdateProfileRequest request, CancellationToken ct)
        {
            var result = await _mediator.Send(new UserProfileUpdateCommand(
                userId,
                request.Age,
                request.Gender,
                request.HealthNote), ct);

            return Ok(result);
        }

        public sealed class CreateProfileRequest
        {
            public Guid UserId { get; set; }
            public string Age { get; set; } = string.Empty;
            public string Gender { get; set; } = string.Empty;
            public string HealthNote { get; set; } = string.Empty;
        }

        public sealed class UpdateProfileRequest
        {
            public string Age { get; set; } = string.Empty;
            public string Gender { get; set; } = string.Empty;
            public string HealthNote { get; set; } = string.Empty;
        }
    }
}
