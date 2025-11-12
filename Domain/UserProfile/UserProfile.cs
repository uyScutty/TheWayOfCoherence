using System;

namespace Domain.UserProfile
{
    public class UserProfile
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; } // FK til ApplicationUser.Id
        public string Age { get; private set; }
        public string Gender { get; private set; }
        public string HealthNote { get; private set; }  // fx beskrivelser

        // Konstruktør
        public UserProfile(Guid userId, string age, string gender, string healthNote)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Age = age;
            Gender = gender;
            HealthNote = healthNote;
        }

        // Domain-metode til opdatering (DDD-praksis)
        public void UpdateProfile(string age, string gender, string healthNote)
        {
            Age = age;
            Gender = gender;
            HealthNote = healthNote;
        }
    }
}
