namespace Process.Domain.Entities
{
    public class UserInfo
    {
        public long Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
    }
}