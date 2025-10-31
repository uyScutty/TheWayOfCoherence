using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Application.Features.Contact.Dtos;

namespace Application.Features.Contact.Queries
{
    public record ListContactMessagesQuery()
       : IRequest<IEnumerable<ContactMessageDto>>;

}
