using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Users
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string FullName { get; private set; }
        public string PasswordHash { get; private set; }
        public string Role { get; private set; }  // fx Owner, User, Admin
        public DateTime CreatedAt { get; private set; }
    }

}
