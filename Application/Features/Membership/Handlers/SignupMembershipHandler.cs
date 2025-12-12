using Application.Features.Membership.Commands;
using Application.Features.Membership.Interfaces;
using Domain.Membership;
using MediatR;

namespace Application.Features.Membership.Handlers
{
    public sealed class SignupMembershipHandler : IRequestHandler<SignupMembershipCommand, Guid>
    {
        private readonly IMembershipRepository _repository;

        public SignupMembershipHandler(IMembershipRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(SignupMembershipCommand request, CancellationToken cancellationToken)
        {
            var membership = MembershipUser.CreateFree(request.UserId);

            await _repository.CreateAsync(membership, cancellationToken);

            return membership.Id;
        }
    }
}
