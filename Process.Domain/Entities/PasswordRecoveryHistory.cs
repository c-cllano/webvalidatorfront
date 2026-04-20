namespace Process.Domain.Entities
{
    public class PasswordRecoveryHistory
    {
        public string Email { get; set; } = null!;
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public string Token { get; set; } = string.Empty;
    }
}
