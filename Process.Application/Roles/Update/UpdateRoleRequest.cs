namespace Process.Application.Roles.Update
{
    public record UpdateRoleRequest(
        long RoleId,
        string Name,
        bool Active);
}
