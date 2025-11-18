using MediatR;

namespace Application.Features.Users.Events
{
    public sealed class UserCreatedEvent : INotification
    {
        public Guid UserId { get; }

        public UserCreatedEvent(Guid userId)
        {
            UserId = userId;
        }
    }
}
