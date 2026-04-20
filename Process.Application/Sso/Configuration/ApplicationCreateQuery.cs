using MediatR;

namespace Process.Application.Sso.Configuration
{
    public class ApplicationCreateQuery : IRequest<object>
    {
        public string ClientId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ClientType { get; set; } = "Public";
        public string? ClientSecret { get; set; }
    }
}
