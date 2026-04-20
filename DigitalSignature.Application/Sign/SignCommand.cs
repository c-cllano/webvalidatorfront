using MediatR;

namespace DigitalSignature.Application.Sign
{
    public record SignCommand : IRequest<SignResponse>
    {
        public long ClientId { get; init; }
        public required string Base64 { get; init; }
        public bool Tsa { get; init; } = true;
        public string NombreFirma { get; init; } = string.Empty;
        public bool LogoDefault { get; init; } = true;
        public string Base64LogoPersonalizado { get; init; } = string.Empty;
    }

}
