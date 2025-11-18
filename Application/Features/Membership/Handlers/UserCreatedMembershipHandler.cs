using Application.Features.Users.Events;
using Application.Features.Membership.Interfaces;
using Domain.Membership;
using MediatR;

namespace Application.Features.Membership.Handlers
{
    public class UserCreatedCreateMembershipHandler
        : INotificationHandler<UserCreatedEvent>
    {
        private readonly IMembershipRepository _repo;

        public UserCreatedCreateMembershipHandler(IMembershipRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(UserCreatedEvent evt, CancellationToken ct)
        {
            var membership = MembershipUser.CreateFree(evt.UserId);

            await _repo.CreateAsync(membership, ct);
        }
    }
}
