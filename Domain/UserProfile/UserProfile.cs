using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.UserProfile
{
    public class UserProfile
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; } // relation
        public int Age { get; private set; }
        public string Gender { get; private set; }
        public string HealthNote { get; private set; }  // fx beskrivelser
    }
}
