using MediatR;

namespace Process.Application.VeriFace
{
    public record GetTokenVeriFaceQuery : IRequest<GetTokenVeriFaceResponse>;
}
