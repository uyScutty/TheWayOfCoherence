using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Contact;
using Domain.Contact;
using Application.Features.Contact.Contracts;

namespace Infrastructure.Persistence.Repositories
{
    public class ContactMessageRepository : IContactMessageRepository
    {
        private readonly AppDbContext _db;

        public ContactMessageRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(ContactMessage message, CancellationToken ct)
        {
            await _db.ContactMessages.AddAsync(message, ct);
        }

        public async Task<IEnumerable<ContactMessage>> ListAsync(CancellationToken ct)
        {
            // Simpel liste af beskeder sorteret efter seneste
            return await _db.ContactMessages
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task SaveChangesAsync(CancellationToken ct)
        {
            await _db.SaveChangesAsync(ct);
        }
    }
}
