namespace Process.Application.Roles.Create
{
    public class CreateRoleRequest
    {
        public Guid ClientGuid { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CreatorUserId { get; set; }
        public bool Active { get; set; }
        public List<Process.Application.Roles.PermissionSelection> Permissions { get; set; } = default!;
    }
}
