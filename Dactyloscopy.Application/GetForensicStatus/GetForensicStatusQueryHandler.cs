using Dactyloscopy.Domain.Parameters.ExternalApiClientParameters;
using Dactyloscopy.Domain.Services;
using MediatR;

namespace Dactyloscopy.Application.GetForensicStatus
{
    public class GetForensicStatusQueryHandler(
        IForensicService forensicService
    ) : IRequestHandler<GetForensicStatusQuery, GetForensicStatusResponse>
    {
        private readonly IForensicService _forensicService = forensicService;

        public async Task<GetForensicStatusResponse> Handle(GetForensicStatusQuery request, CancellationToken cancellationToken)
        {
            ForensicStatusResponse response = await _forensicService
                .GetForensicStatusAsync(request.TxGuid);

            if (response == null || response.Code != 200)
            {
                throw new KeyNotFoundException($"Error en el servicio de Dactiloscopia GetForensicStatus");
            }

            return new()
            {
                TxGuid = response.Data.TxGuid,
                Reviewed = response.Data.Revisada,
                ReviewDate = response.Data.FechaRevision,
                Approved = response.Data.Aprobada,
                Score = response.Data.Score,
                MainReason = response.Data.MotivoPrincipal,
                OptionalReason = response.Data.MotivoOpcional,
                Description = response.Data.Descripcion,
                Observation = response.Data.Observacion
            };
        }
    }
}
