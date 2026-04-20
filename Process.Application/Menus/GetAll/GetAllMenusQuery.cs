using MediatR;
using Process.Domain.Entities;

namespace Process.Application.Menus.GetAll
{
    public record GetAllMenusQuery() : IRequest<SsoServiceResult<List<MenuResponse>>>;
}
