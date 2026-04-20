namespace Process.Domain.Parameters.Sso.Account
{
    public class ForgotPasswordRequest
    {
        public required string Email { get; init; }
        public required Guid Token { get; init; }
    }
}
