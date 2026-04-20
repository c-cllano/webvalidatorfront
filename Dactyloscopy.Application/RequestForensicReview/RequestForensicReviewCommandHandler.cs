using Dactyloscopy.Domain.Entities;
using Dactyloscopy.Domain.Parameters.ExternalApiClientParameters;
using Dactyloscopy.Domain.Repositories;
using Dactyloscopy.Domain.Services;
using Dactyloscopy.Domain.Utilities;
using MediatR;

namespace Dactyloscopy.Application.RequestForensicReview
{
    public class RequestForensicReviewCommandHandler(
        IForensicService forensicService,
        IStatusForensicRepository statusForensicRepository,
        IForensicReviewProcessRepository forensicReviewProcessRepository
    ) : IRequestHandler<RequestForensicReviewCommand, RequestForensicReviewResponse>
    {
        private readonly IForensicService _forensicService = forensicService;
        private readonly IStatusForensicRepository _statusForensicRepository = statusForensicRepository;
        private readonly IForensicReviewProcessRepository _forensicReviewProcessRepository = forensicReviewProcessRepository;

        public async Task<RequestForensicReviewResponse> Handle(RequestForensicReviewCommand request, CancellationToken cancellationToken)
        {
            object contentBody = GetContentBody(request);

            ForensicReviewResponse response = await _forensicService
                .RequestForensicReviewAsync(contentBody);

            if (response == null || response.Code != 200)
            {
                throw new KeyNotFoundException($"Error en el servicio de Dactiloscopia RequestForensicReview");
            }

            StatusForensic? statusForensic = await _statusForensicRepository
                .GetStatusByDescriptionAsync(Constants.InReview)
                    ?? throw new KeyNotFoundException($"No se encontró estado forense 'En revisión'");

            ForensicReviewProcess forensicReviewProcess = new()
            {
                ValidationProcessId = request.ValidationProcessId,
                TxGuidForense = response.Data.TxGuid,
                StatusForensicId = statusForensic.StatusForensicId,
                CreatedDate = DateTime.UtcNow.AddHours(-5),
                Active = true,
                IsDeleted = false
            };

            await _forensicReviewProcessRepository.SaveForensicReviewProcessAsync(forensicReviewProcess);

            return new()
            {
                TxGuid = response.Data.TxGuid,
                ProcessAgreementId = response.Data.ProcesoConvenioId
            };
        }

        private static object GetContentBody(RequestForensicReviewCommand request)
        {
            var objects = new List<object>();

            foreach (var item in request.Objects)
            {
                objects.Add(new
                {
                    tipoObjeto = item.ObjectType,
                    objeto = item.Object,
                    formatoObjeto = item.ObjectFormat
                });
            }

            object contentBody = new
            {
                guidConvenio = request.AgreementGuid,
                tipoDocumentoId = request.DocumentTypeId,
                numeroDocumento = request.DocumentNumber,
                objetos = objects
            };

            return contentBody;
        }
    }
}
