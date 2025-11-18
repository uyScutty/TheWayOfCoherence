using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Membership.Dtos
{
    public class MemberDto
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
