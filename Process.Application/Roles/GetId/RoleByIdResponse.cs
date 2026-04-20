namespace Process.Application.Roles.GetId
{
    public class RoleByIdResponse
    {
        public long RoleId { get; set; }
        public string Name { get; set; } = string.Empty; 
        public bool Active { get; set; } 
        public string Status { get; set; } = string.Empty;
        public int Users { get; set; }
    }
}
