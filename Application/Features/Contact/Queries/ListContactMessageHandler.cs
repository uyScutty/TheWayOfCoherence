using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Contact.Interfaces;
using Application.Features.Contact.Dtos;
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
            var messages = await _repo.ListAsync(ct);
            return messages.Select(m => new ContactMessageDto(
                m.Id,
                m.Name,
                m.Email,
                m.Subject,
                m.Message,
                m.CreatedAt
            ));
        }
    }

}
