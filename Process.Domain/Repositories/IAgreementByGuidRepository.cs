using Process.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process.Domain.Repositories
{
    public interface IAgreementByGuidRepository
    {
        Task<IEnumerable<AgreementByGuid>> GetByGuidAsync(Guid guid);
    }
 }
