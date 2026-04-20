namespace Process.Application.Roles.GetByClient
{
    public record GetRoleByClientResponse(int Id, string Name, string Status, int Users, long ClientId);
}
