using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contact.Enums;

namespace Domain.Contact
{
    public class ContactMessage
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Subject { get; private set; }
        public string Message { get; private set; }
        public ContactStatus Status { get; private set; } = ContactStatus.New;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public ContactMessage(string name, string email, string subject, string message)
        {
            Name = name;
            Email = email;
            Subject = subject;
            Message = message;
        }

        public void MarkHandled() => Status = ContactStatus.Handled;

      
    }
}