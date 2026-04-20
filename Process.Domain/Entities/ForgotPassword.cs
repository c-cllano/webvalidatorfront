using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process.Domain.Entities
{
    public class ForgotPassword
    {
        public required string Email { get; init; }
        public required Guid Token { get; init; }
    }
}
