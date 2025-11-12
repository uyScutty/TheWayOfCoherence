using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Contact;
using Application.Features.Contact.Interfaces;
using Domain.Contact;
using Domain.Contact.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ContactMessageRepository : IContactMessageRepository
    {
        private readonly AppDbContext _db;
        public ContactMessageRepository(AppDbContext db) => _db = db;

        public async Task AddAsync(ContactMessage message, CancellationToken ct)
            => await _db.ContactMessages.AddAsync(message, ct);

        public async Task<IEnumerable<ContactMessage>> GetAllAsync(CancellationToken ct)
            => await _db.ContactMessages.AsNoTracking().ToListAsync(ct);

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);

       public async Task<IEnumerable<ContactMessage>> ListAsync(CancellationToken ct)
            => await _db.ContactMessages.ToListAsync(ct);

    }

}

