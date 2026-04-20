using MediatR;
using Process.Domain.Repositories;
using Process.Domain.Services;

namespace Process.Application.Sso.User
{
    public class DeleteUserHandler(
        ISsoService ssoService,
        IUserOkeyRepository userOkeyRepository,
        IRoleByUserRepository roleByUserRepository,
        IRoleAgreementByUserRepository roleAgreementByUserRepository
        ) : IRequestHandler<DeleteUserQuery, object>
    {
        private readonly ISsoService _ssoService = ssoService;
        private readonly IUserOkeyRepository _userOkeyRepository = userOkeyRepository;
        private readonly IRoleByUserRepository _roleByUserRepository = roleByUserRepository;
        private readonly IRoleAgreementByUserRepository _roleAgreementByUserRepository = roleAgreementByUserRepository;

        public async Task<object> Handle(DeleteUserQuery request, CancellationToken cancellationToken)
        {
             
            var result = await _ssoService.DeleteUser(request.userId);
            
          
            await DeactivateUserOkeyRecord(request.userId);
            
           
            await DeactivateUserRoles(request.userId);

            await DeactivateUserRolesNew(request.userId);

            return result!;
        }

        /// <summary>
        /// Desactiva el registro UserOkey del usuario
        /// </summary>
        private async Task DeactivateUserOkeyRecord(int userId)
        {
            var userOkey = await _userOkeyRepository.GetByUserIdAsync(userId);
            
            if (userOkey == null)
                return;  
            
            
            userOkey.Active = false;
            userOkey.IsDeleted = true;
            userOkey.UpdatedDate = DateTime.UtcNow.AddHours(-5);
            
            await _userOkeyRepository.UpdateAsync(userOkey);
        }

        /// <summary>
        /// Desactiva todos los roles asignados al usuario
        /// </summary>
        private async Task DeactivateUserRoles(int userId)
        {
            await _roleByUserRepository.UpdateUserRoles(userId, [], 0);
        }

        private async Task DeactivateUserRolesNew(int userId)
        {
            await _roleAgreementByUserRepository.UpdateUserRoles(userId, [], 0);
        }
    }
}
