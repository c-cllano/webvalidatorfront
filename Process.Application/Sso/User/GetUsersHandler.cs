using MediatR;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Domain.Services;

namespace Process.Application.Sso.User
{
    public class GetUsersHandler(
        ISsoService ssoService,
        IUserOkeyRepository userOkeyRepository) : IRequestHandler<GetUsersQuery, SsoServiceResult<List<UserSso>>>
    {
        private readonly ISsoService _ssoService = ssoService;
        private readonly IUserOkeyRepository _userOkeyRepository = userOkeyRepository;
        
        public async Task<SsoServiceResult<List<UserSso>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var userQueryParameters = new Domain.Parameters.Sso.User.UserQueryParameters
            {
                ClientId = request.ClientId,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                SecondName = request.SecondName,
                SecondLastName = request.SecondLastName,
            };
            
            var result = await _ssoService.GetUsers(userQueryParameters);
            
           
            if (result?.Data is not { Count: > 0 })
                return result ?? SsoServiceResult<List<UserSso>>.Ok(new List<UserSso>());

           
            var uniqueUsers = RemoveDuplicateUsers(result.Data);
            result = SsoServiceResult<List<UserSso>>.Ok(uniqueUsers);

            await EnrichUsersWithOkeyData(result.Data!);

            return result;
        }

        /// <summary>
        /// Elimina usuarios duplicados, manteniendo solo la primera ocurrencia de cada UserId
        /// </summary>
        private static List<UserSso> RemoveDuplicateUsers(List<UserSso> users)
        {
            return users
                .GroupBy(u => u.UserId)
                .Select(g => g.First())
                .ToList();
        }

        private async Task EnrichUsersWithOkeyData(List<UserSso> users)
        {
            var userIds = users.Select(u => u.UserId).ToList();
            
            var usersOkey = await _userOkeyRepository.GetUsers(userIds);
            
           
            var usersOkeyDict = usersOkey.ToDictionary(u => (int)u.UserId);
         
            foreach (var user in users)
            {
                if (usersOkeyDict.TryGetValue(user.UserId, out var userOkey))
                {
                   
                    user.CellPhone = userOkey.CellPhone;
                    user.UserGuid = userOkey.UserGuid;
                }
                else
                {
                 
                    var newUserOkey = new UserOkey
                    {
                        UserId = user.UserId,
                        UserGuid = user.UserGuid ?? Guid.NewGuid(),
                        CellPhone = null,
                        CreatorUserId = user.CreatorUserId,
                        CreatedDate = DateTime.UtcNow.AddHours(-5),
                        UpdatedDate = null,
                        Active = true,
                        IsDeleted = false
                    };
                    
                    await _userOkeyRepository.AddAsync(newUserOkey);
                    
                 
                    user.CellPhone = newUserOkey.CellPhone;
                    user.UserGuid = newUserOkey.UserGuid;
                }
            }
        }
    }
}
