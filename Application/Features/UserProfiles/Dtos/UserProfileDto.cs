using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Features.UserProfiles.Dtos
{
    public record UserProfileDto
    (
       
        Guid Id,
    Guid UserId,
    string Age,
    String Gender,
    string HealthNote
    )
    {
    }
}



