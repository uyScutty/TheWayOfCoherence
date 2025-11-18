using Application.Abstractions.Contracts;
using Application.Features.Users.Commands;
using Application.Features.Users.Events;
using MediatR;

public class UserSignupHandler : IRequestHandler<UserSignupCommand, Guid>
{
    private readonly IIdentityService _identity;
    private readonly IMediator _mediator;

    public UserSignupHandler(IIdentityService identity, IMediator mediator)
    {
        _identity = identity;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(UserSignupCommand cmd, CancellationToken ct)
    {
        var userId = await _identity.CreateUserAsync(cmd.Email, cmd.Password, cmd.FullName, ct);

        await _mediator.Publish(new UserCreatedEvent(userId), ct);


        return userId;
    }
}
