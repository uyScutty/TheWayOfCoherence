using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions.Contracts.Gateways;
using Application.Features.Contact.Contracts;
using MediatR;

namespace Application.Features.Membership.Commands
{
    public class SignupMembershipHandler : IRequestHandler<SignupMembershipCommand>
    {
        
        private readonly ISignupRepository _repo;
       

        public SignupMembershipHandler(
            IContactMessageRepository repo,
         )
        {
            _repo = repo;
            
        }
    }
}
