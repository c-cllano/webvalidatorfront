using MediatR;
using Process.Domain.Services;

namespace Process.Application.AgreementProcess.GetByGuid
{
    public class GetValidateGuidQueryHandler(
        ISigningCredentialService credentialService) : IRequestHandler<GetValidateGuidQuery, bool>
    {
        private readonly ISigningCredentialService _credentialService = credentialService ?? throw new ArgumentNullException(nameof(credentialService));

        public async Task<bool> Handle(GetValidateGuidQuery request, CancellationToken cancellationToken)
        {
            string process = await ValidateAndGetProcessFromCredentialService(request.Token);

            if (!string.IsNullOrEmpty(process))
            {
               request.ProcessAgreementGuid = Guid.Parse(process);
            }

            return string.IsNullOrEmpty(process) || process.Equals(request.ProcessAgreementGuid.ToString());

        }

        private async Task<string> ValidateAndGetProcessFromCredentialService(string token)
        {
            string process = await _credentialService.ConsultAgreementProcess(token);
            if (string.IsNullOrEmpty(process))
            {
                throw new InvalidOperationException("No se pudo consultar el proceso en el servicio de credenciales.");
            }

            return process;
        }
    }
}

