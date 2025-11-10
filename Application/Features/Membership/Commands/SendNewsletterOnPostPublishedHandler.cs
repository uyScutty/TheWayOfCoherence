using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions.Contracts.Gateways;
using Domain.Content.Events;

namespace Application.Features.Membership.Commands
{
    public class SendNewsletterOnPostPublishedHandler : IDomainEventHandler<PostPublishedEvent>
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

        public async Task HandleAsync(PostPublishedEvent evt)
        {
            var activeMembers = await _members.GetActiveMembersAsync();

            foreach (var member in activeMembers)
            {
                await _email.SendAsync(
                    member.Email,
                    $"New post: {evt.Title}",
                    $"A new post has been published — check it out on the site!");
            }
        }
    }

    public interface IDomainEventHandler<TEvent>
    {
        Task HandleAsync(TEvent evt);
    }
}

