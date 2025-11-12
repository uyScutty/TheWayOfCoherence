using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Contact.Dtos;
using Application.Features.Contact.Interfaces;
using Domain.Contact;
using MediatR;

namespace Application.Features.Contact.Queries
{
    public class ListContactMessagesHandler
          : IRequestHandler<ListContactMessagesQuery, IEnumerable<ContactMessageDto>>
    {
        private readonly IContactMessageRepository _repo;

        public ListContactMessagesHandler(IContactMessageRepository repo)
            => _repo = repo;

        public async Task<IEnumerable<ContactMessageDto>> Handle(
            ListContactMessagesQuery query,
            CancellationToken ct)
        {
            // 1️⃣ Hent alle beskeder fra repository
            var messages = await _repo.ListAsync(ct);

            // 2️⃣ Mapper hver domain-entity til en DTO
            var dtos = messages.Select(m => new ContactMessageDto(
                m.Id,
                m.Name,
                m.Email,
                m.Subject,
                m.Message,
                m.CreatedAt
            ));

            // 3️⃣ Returnér DTO-liste
            return dtos;
        }
    }
}





