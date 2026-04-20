using MediatR;

namespace Process.Application.ValidationProcesses.CompareFaces
{
    public record CompareFacesCommand(
        long? ValidationProcessId,
        string Face1,
        string Format1,
        string Face2,
        string Format2,
        bool SaveTrace
    ) : IRequest<CompareFacesResponse>;
}
