using MediatR;

namespace Process.Application.Recapcha
{
    public record GetValidateRecaptchaCommand(string Response) : IRequest<GetValidateRecaptchaResponse>;

}
