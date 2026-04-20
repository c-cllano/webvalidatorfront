using MediatR;
using Process.Application.Roles.GetByUser;
using Process.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process.Application.Menus.GetActualRoleBySideMenu
{


    public class GetActualRoleBySideMenuHandler(IMenuRepository repository) : IRequestHandler<GetActualRoleBySideMenuQuery, List<Domain.Entities.RoleActual>>

    {
        private readonly IMenuRepository _repository = repository;

        public async Task<List<Domain.Entities.RoleActual>> Handle(GetActualRoleBySideMenuQuery request, CancellationToken cancellationToken)
        {
            var usersRole = await _repository.GetActualRoleBySideMenu(request.UserId , request.AgreementId);
            return (List<Domain.Entities.RoleActual>)usersRole;
        }

    }
}
