using MediatR;
using Process.Domain.Entities;
using Process.Domain.Services;

namespace Process.Application.Sso.Configuration
{
    public class ApplicationCreateHandler(ISsoService ssoService) : IRequestHandler<ApplicationCreateQuery, object>
    {
        private readonly ISsoService _ssoService = ssoService;
        public async Task<object> Handle(ApplicationCreateQuery request, CancellationToken cancellationToken)
        {
            ApplicationCreate applicationCreate = new()
            {
                ClientId = request.ClientId,
                DisplayName = request.DisplayName,
                Description = request.Description,
                ClientType = request.ClientType,
                ClientSecret = request.ClientSecret
            };
            var result = await _ssoService.ApplicationCreate(applicationCreate);
            return result!;
        }
    }
}
