using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Membership
{
    public class MembershipUser
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private MembershipUser() { } // EF

        public static MembershipUser CreateFree(Guid userId)
            => new(userId);

        private MembershipUser(Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
            IsActive = true; // prototype auto-active
        }
    }
}
