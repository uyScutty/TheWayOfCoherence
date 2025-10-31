using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Membership
{
    public class Membership
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; } // relation
        public bool IsActive { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime? ExpiresAt { get; private set; }

        public void Activate(TimeSpan duration)
        {
            IsActive = true;
            StartedAt = DateTime.UtcNow;
            ExpiresAt = StartedAt.Add(duration);
        }

        public void Deactivate() => IsActive = false;
    }

}
