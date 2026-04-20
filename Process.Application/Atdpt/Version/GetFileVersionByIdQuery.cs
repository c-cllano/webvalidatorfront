using MediatR;
using Process.Domain.ValueObjects;

namespace Process.Application.Atdpt.Version
{
    public record GetFileVersionByIdQuery(int AtdpId, Guid agreementGuid) : IRequest<AtdpVersionFile>;
}
