using Application.Features.Membership.Dtos;
using Application.Features.Membership.Interfaces;
using Application.Abstractions.Contracts.Gateways;
using Domain.Content.Events;
using Application.Common;
using MediatR;

namespace Application.Features.Membership.Handlers
{
    public class SendNewsletterOnPostPublishedHandler
        : INotificationHandler<DomainEventNotification<PostPublishedEvent>>
    {
        private readonly IMembershipRepository _members;
        private readonly IEmailNotifier _email;

        public SendNewsletterOnPostPublishedHandler(
            IMembershipRepository members,
            IEmailNotifier email)
        {
            _members = members;
            _email = email;
        }

        public async Task Handle(
          DomainEventNotification<PostPublishedEvent> notification,
          CancellationToken ct)
        {
            var evt = notification.DomainEvent;

            var activeMembers = await _members.GetActiveMembersAsync(ct);

            foreach (var member in activeMembers)
            {
                await _email.SendAsync(
                    member.Email,
                    $"New post published: {evt.Title}",
                    $"A new post has been published — go check it out!");
            }
        }

    }
}


