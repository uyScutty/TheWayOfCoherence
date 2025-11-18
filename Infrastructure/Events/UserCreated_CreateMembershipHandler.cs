using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Membership.Interfaces;
using Application.Features.Users.Events;
using MediatR;
using Domain.Membership;


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
