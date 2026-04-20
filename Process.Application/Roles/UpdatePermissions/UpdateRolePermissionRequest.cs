namespace Process.Application.Roles.UpdatePermissions
{
    public record UpdateRolePermissionRequest(long RoleId, List<PermissionSelection> Permissions) {
    }
}
