using DrawFlowConfiguration.Domain.Parameters.DrawFlow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawFlowConfiguration.Domain.Repositories
{
    public interface IDocumentTypeByCountryRepository
    {
        Task<IEnumerable<DocumentTypeByCountry>> GetDocumentTypeByCountry();
    }

}
