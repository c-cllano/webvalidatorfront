using MediatR;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Domain.Services;

namespace Process.Application.Sso.User
{
    public class UpdateUserHandler(
        ISsoService ssoService,
        IUserOkeyRepository userOkeyRepository,
        IRoleByUserRepository roleByUserRepository) : IRequestHandler<UpdateUserQuery, object>
    {
        private readonly ISsoService _ssoService = ssoService;
        private readonly IUserOkeyRepository _userOkeyRepository = userOkeyRepository;
        private readonly IRoleByUserRepository _roleByUserRepository = roleByUserRepository;

        public async Task<object> Handle(UpdateUserQuery request, CancellationToken cancellationToken)
        {
            var updateUser = new UpdateUser
            {
                FirstName = request.FirstName,
                SecondName = request.SecondName,
                LastName = request.LastName,
                SecondLastName = request.SecondLastName,
                Email = request.Email,
                UserName = request.UserName,
                DocumentType = request.DocumentType,
                DocumentNumber = request.DocumentNumber,
                ClientId = request.ClientId,
                RoleId = request.RoleId,
                Active = request.Status
            };
            
            var result = await _ssoService.UpdateUser(request.UserId, updateUser);
            
        
            await UpdateUserOkeyRecord(request);

            return result!;
        }

        private async Task UpdateUserOkeyRecord(UpdateUserQuery request)
        {
            var userOkey = await _userOkeyRepository.GetByUserIdAsync(request.UserId);


            if (userOkey != null &&  (userOkey.Active != request.Status))
            {
                userOkey.Active = request.Status;
                userOkey.UpdatedDate = DateTime.UtcNow.AddHours(-5);

                await _userOkeyRepository.UpdateAsync(userOkey);
            }
            if (userOkey != null && !string.IsNullOrEmpty(request.CellPhone) && 
                !string.Equals(userOkey.CellPhone, request.CellPhone, StringComparison.InvariantCultureIgnoreCase))
            {
                userOkey.CellPhone = request.CellPhone;
                userOkey.UpdatedDate = DateTime.UtcNow.AddHours(-5);

                await _userOkeyRepository.UpdateAsync(userOkey);
            }
        }
    }
}
