using MediatR;

namespace Dactyloscopy.Application.GetForensicStatus
{
    public record GetForensicStatusQuery(
        Guid TxGuid
    ) : IRequest<GetForensicStatusResponse>;
}
