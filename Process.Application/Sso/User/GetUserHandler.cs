using MediatR;
using Process.Application.Sso.User.Dto;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Domain.Services;
using System.ComponentModel.Design;

namespace Process.Application.Sso.User
{
    public class GetUserHandler(
        ISsoService ssoService,
        IUserOkeyRepository userOkeyRepository,
        IRoleByUserRepository roleByUserRepository,
        IRoleAgreementByUserRepository roleAgreementByUserRepository) : IRequestHandler<GetUserQuery, UserSummaryDto>
    {
        private readonly ISsoService _ssoService = ssoService;
        private readonly IUserOkeyRepository _userOkeyRepository = userOkeyRepository;
        private readonly IRoleByUserRepository _roleByUserRepository = roleByUserRepository;
        private readonly IRoleAgreementByUserRepository _roleAgreementByUserRepository = roleAgreementByUserRepository;

        public async Task<UserSummaryDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            // Obtener datos básicos del usuario desde SSO
            var userSso = await _ssoService.GetUser(request.UserId);
            
            // Obtener información adicional de UserOkey
            var userOkey = await _userOkeyRepository.GetByUserIdAsync(request.UserId);
            
            // Obtener roles y agreements del usuario en una sola consulta optimizada
            var rolesAndAgreementsOld = await _roleByUserRepository.GetUserRolesAndAgreementsAsync(request.UserId);
            var rolesAndAgreements = await _roleAgreementByUserRepository.GetUserRolesAndAgreementsAsync(request.UserId);

            // Construir el DTO con la información completa
            var userSummary = new UserSummaryDto
            {
                Active = userOkey!.Active,
                DocumentType = userSso.DocumentType,
                DocumentNumber = userSso.DocumentNumber,
                FirstName = userSso.FirstName,
                SecondName= userSso.SecondName,
                FirstLastName = userSso.FirstLastName,
                SecondLastName = userSso.SecondLastName,
                UserId = userSso.UserId,
                UserFullName = BuildFullName(userSso.FirstName, userSso.SecondName, 
                                            userSso.FirstLastName, userSso.SecondLastName),
                Email = userSso.Email,
                User = string.Join(" ", userSso.FirstName, userSso.FirstLastName),
                CellPhone = userOkey?.CellPhone,
                UserStatus = userSso.UserStatus,
                AssignRoleAgreement = [.. rolesAndAgreements
                    .Select(ra => new AssignRoleAgreementDto
                    {
                        RoleAgreementByUserId = ra.RoleAgreementByUserId,
                       // RoleByAgreementId  = ra.RoleByAgreementId,
                        RoleId = (int)ra.RoleId,
                        RoleName = ra.RoleName,
                        AgreementId = ra.AgreementId,
                        AgreementName = ra.AgreementName
                    })]
            };
            
            return userSummary;
        }

        /// <summary>
        /// Construye el nombre completo del usuario eliminando espacios múltiples
        /// </summary>
        private static string BuildFullName(string firstName, string secondName, string firstLastName, string secondLastName)
        {
            var parts = new[] { firstName, secondName, firstLastName, secondLastName }
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Select(p => p.Trim());
            
            return string.Join(" ", parts);
        }
    }
}
