using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contact.Enums;
using Domain.Contact;

namespace Application.Features.Contact.Interfaces;

public interface IContactMessageRepository
{
    Task AddAsync(ContactMessage message, CancellationToken ct);
    Task<IEnumerable<ContactMessage>> ListAsync(CancellationToken ct);

    Task SaveChangesAsync(CancellationToken ct);

  

}
