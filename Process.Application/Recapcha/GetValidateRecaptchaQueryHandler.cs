using MediatR;
using Process.Domain.Parameters.Recapcha;
using Process.Domain.Services;

namespace Process.Application.Recapcha
{
    public class GetValidateRecaptchaQueryHandler(IRecaptchaServices recapchaServices) : IRequestHandler<GetValidateRecaptchaCommand, GetValidateRecaptchaResponse>
    {

        private readonly IRecaptchaServices _recapchaServices = recapchaServices;
        public async Task<GetValidateRecaptchaResponse> Handle(GetValidateRecaptchaCommand request, CancellationToken cancellationToken)
        {
            RecaptchaResponse recapchaResponse = await _recapchaServices.GetValidateRecaptchaAsync(request.Response);

            return new()
            {
                success = recapchaResponse.success
            };
        }
    }
}
