using MediatR;

namespace Process.Application.Roles.GetByClient
{
    public record GetRoleByClientQuery(Guid ClientGuid, string[]? RolName, string? Status, int? PageSize, int? PageNumber)
        : IRequest<List<GetRoleByClientResponse>>;
}
