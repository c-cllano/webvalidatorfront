using MediatR; 
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Domain.Services;
using Process.Domain.Utilities;

namespace Process.Application.Sso.User
{
    public class RegisterHandler(
        ISsoService ssoService,
        IUserOkeyRepository userOkeyRepository,
        IRoleByUserRepository roleByUserRepository,
        IAgreementByUserRepository agreementByUser,
        IRoleAgreementByUserRepository roleAgreementByUser
        ) : IRequestHandler<RegisterQuery, object>
    {
        private readonly ISsoService _ssoService = ssoService;
        private readonly IUserOkeyRepository _userOkeyRepository = userOkeyRepository;
        private readonly IRoleByUserRepository _roleByUserRepository = roleByUserRepository;
        private readonly IAgreementByUserRepository _agreementByUser = agreementByUser;
        private readonly IRoleAgreementByUserRepository _roleAgreementByUserRepository = roleAgreementByUser;

        public async Task<object> Handle(RegisterQuery request, CancellationToken cancellationToken)
        {
           
            var existingUser = await FindExistingUser(request);
            
            UserSsoCreated userSso;
            
            if (existingUser != null)
            {                
                userSso = MapUserSsoToUserSsoCreated(existingUser);
            }
            else
            {
                // Usuario no existe, crearlo
                var createUser = new CreateUser
                {
                    DocumentType = request.DocumentType,
                    DocumentNumber = request.DocumentNumber,
                    FirstName = request.FirstName,
                    SecondName = request.SecondName,
                    LastName = request.LastName,
                    SecondLastName = request.SecondLastName,
                    Email = request.Email,
                    UserName = request.UserName,
                    Password = Constants.DefaultPassword,
                    EmailConfirmed = request.EmailConfirmed,
                    CreatedUserId = request.CreatedUserId,
                    ConfirmPassword = Constants.DefaultPassword,
                    ClientId = request.ClientId,
                    RoleId = request.RoleId
                };
                
                var result = await _ssoService.Register(createUser);
        
                if (result is not UserSsoCreated createdUser)
                    return result!;
                
                userSso = createdUser;
                
                // Crear registro UserOkey solo si es un usuario nuevo
                await CreateUserOkeyRecord(userSso, request);
            }

            // Asignar roles y convenios (tanto para usuario nuevo como existente)
            await NewAssignRolesAndAgreementsToUser(userSso.UserId, request);
            return userSso;
        }

        /// <summary>
        /// Busca un usuario existente por email, username o documento
        /// </summary>
        private async Task<UserSso?> FindExistingUser(RegisterQuery request)
        {
            
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                var usersByEmail = await _ssoService.GetUsers(new Domain.Parameters.Sso.User.UserQueryParameters
                {
                    ClientId = request.ClientId,
                    Email = request.Email,
                    PageNumber = 1,
                    PageSize = 1000
                });

                if (usersByEmail?.Data?.Count > 0)
                    return usersByEmail.Data[0];
            }           
            
            return null;
        }

        /// <summary>
        /// Mapea un UserSso a UserSsoCreated para mantener consistencia
        /// </summary>
        private static UserSsoCreated MapUserSsoToUserSsoCreated(UserSso user)
        {
            return new UserSsoCreated
            {
                UserId = user.UserId,
                UserStatusId = user.UserStatusId,
                UserGuid = user.UserGuid ?? Guid.NewGuid(),
                UserStatus = user.UserStatus,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                FirstLastName = user.FirstLastName,
                SecondLastName = user.SecondLastName,
                Email = user.Email,
                UserName = user.UserName,
                DocumentType = user.DocumentType,
                DocumentNumber = user.DocumentNumber,
                LastLoginDate = user.LastLoginDate,
                CreationDate = user.CreationDate,
                CreatorUserId = user.CreatorUserId,
                IsDeleted = user.IsDeleted
            };
        }

        private async Task CreateUserOkeyRecord(UserSsoCreated userSso, RegisterQuery request)
        {
            // Verificar si ya existe un registro UserOkey para este usuario
            var existingUserOkey = await _userOkeyRepository.GetByUserIdAsync(userSso.UserId);
            
            if (existingUserOkey != null)
                return; // Ya existe, no crear duplicado
            
            var userOkey = new UserOkey
            {
                UserId = userSso.UserId,
                UserGuid = userSso.UserGuid,
                CellPhone = request.CellPhone,
                CreatorUserId = request.CreatedUserId,
                CreatedDate = DateTime.UtcNow.AddHours(-5),
                UpdatedDate = null,
                Active = true,
                IsDeleted = false
            };
            
            await _userOkeyRepository.AddAsync(userOkey);
        }

        private async Task AssignRolesAndAgreementsToUser(int userId, RegisterQuery request)
        {
            if (request.AssignmentRoleAgreements is not { Count: > 0 })
                return;

            var createdDate = DateTime.UtcNow.AddHours(-5);
            
         
            foreach (var assignment in request.AssignmentRoleAgreements.Where(a => a.RoleId > 0 && a.AgreementId > 0))
            {
               
                var existingRoleByUser = await _roleByUserRepository.GetByUserIdAndRoleIdAsync(userId, assignment.RoleId);
                if (existingRoleByUser == null)
                {
                    var roleByUser = new RoleByUser
                    {
                        RoleId = assignment.RoleId,
                        UserId = userId,
                        CreatorUserId = request.CreatedUserId,
                        CreatedDate = createdDate,
                        Active = true,
                        IsDeleted = false
                    };
                    
                    await _roleByUserRepository.AddAsync(roleByUser);
                }



                var agreement  = await _agreementByUser.GetByUserIdAgreementAsync(userId, assignment.AgreementId);
                if (agreement is null)
                {
                    var roleByAgreement = new Domain.Entities.AgreementByUser
                    {
                        UserId = userId,
                        AgreementId = assignment.AgreementId,
                        CreatedUserId = request.CreatedUserId,
                        CreatedDate = createdDate,
                        Active = true,
                        IsDelete = false
                    };
                    
                    await _agreementByUser.AddAsync(roleByAgreement);
                }
            }
        }


        /**** NUEVA IMPLEMENTACION***/

        private async Task NewAssignRolesAndAgreementsToUser(int userId, RegisterQuery request)
        {
            if (request.AssignmentRoleAgreements is not { Count: > 0 })
                return;

            var createdDate = DateTime.UtcNow.AddHours(-5);


            foreach (var assignment in request.AssignmentRoleAgreements.Where(a => a.RoleId > 0 && a.AgreementId > 0))
            {
                var existingRoleAgreementByUser = await _roleAgreementByUserRepository.GetByUserRoleAndAgreemenmt(userId, assignment.RoleId, assignment.AgreementId);

                if (existingRoleAgreementByUser.Count() == 0)
                {
                    var ObjRoleAgreementByUser = new RoleAgreementByUser
                    {
                        RoleId = assignment.RoleId,
                        UserId = userId,
                        AgreementId = assignment.AgreementId,
                        CreatorUserId = request.CreatedUserId,
                        CreatedDate = createdDate,
                        Active = true,
                        IsDeleted = false
                    };

                    await _roleAgreementByUserRepository.AddAsync(ObjRoleAgreementByUser);
                }
            }
        }
    }
}
