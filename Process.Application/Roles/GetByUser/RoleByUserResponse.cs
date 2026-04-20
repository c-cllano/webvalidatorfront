using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process.Application.Roles.GetByUser
{

    public record RoleByUserResponse(long UserId, string NameRol, long RolId);
}
