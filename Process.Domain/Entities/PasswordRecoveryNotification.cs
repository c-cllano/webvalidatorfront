namespace Process.Domain.Entities
{
    public class PasswordRecoveryNotification
    {
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public string Token { get; set; } = string.Empty;
    }
}
