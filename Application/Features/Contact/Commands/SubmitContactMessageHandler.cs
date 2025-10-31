using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions.Contracts.Gateways;
using Application.Features.Contact.Interfaces;
using MediatR;
using Domain.Contact;

namespace Application.Features.Contact.Commands
{
    public class SubmitContactMessageHandler : IRequestHandler<SubmitContactMessageCommand>
    {
        private readonly IContactMessageRepository _repo;
        private readonly IEmailNotifier _email;

        public SubmitContactMessageHandler(
            IContactMessageRepository repo,
            IEmailNotifier email)
        {
            _repo = repo;
            _email = email;
        }

        public async Task Handle(SubmitContactMessageCommand cmd, CancellationToken ct)
        {
            // 1. Gem beskeden i databasen (simpel persistens)
            var message = new ContactMessage(cmd.Name, cmd.Email, cmd.Subject, cmd.Message);
            await _repo.AddAsync(message, ct);
            await _repo.SaveChangesAsync(ct);

            // 2. Send mail til ejeren
            await _email.SendAsync(
                "owner@thewayofcoherence.dk",
                $"Ny kontakt fra {cmd.Name}",
                $"{cmd.Subject}\n\n{cmd.Message}");
        }
    }
}

