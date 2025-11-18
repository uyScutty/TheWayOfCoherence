using Application.Features.UserProfiles.Commands;
using Application.Features.UserProfiles.Dtos;
using Application.Features.UserProfiles.Interfaces;
using Application.Features.UserProfiles.Mappers;
using MediatR;

namespace Application.Features.UserProfiles.Handlers {
    public class UserProfileUpdateHandler
            : IRequestHandler<UserProfileUpdateCommand, UserProfileDto>
    {
        private readonly IUserProfileRepository _repo;

        public UserProfileUpdateHandler(IUserProfileRepository repo)
        {
            _repo = repo;
        }

        public async Task<UserProfileDto> Handle(
            UserProfileUpdateCommand cmd,
            CancellationToken ct)
        {
            // 1) Find domæne-entity'en
            var profile = await _repo.GetByIdAsync(cmd.UserId, ct);
            if (profile == null)
                throw new Exception("UserProfile not found");

         
            // 3) Persistér
            await _repo.UpdateAsync(profile, ct);
            await _repo.SaveChangesAsync(ct);

            // 4) Map og returnér den ene DTO
            return UserProfileMapper.ToDto(profile);
        }
    }
}