using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Contact.Dtos
{
    public record ContactMessageDto(
      Guid Id,
      string Name,
      string Email,
      string Subject,
      string Message,
      DateTime CreatedAt
  );

}
