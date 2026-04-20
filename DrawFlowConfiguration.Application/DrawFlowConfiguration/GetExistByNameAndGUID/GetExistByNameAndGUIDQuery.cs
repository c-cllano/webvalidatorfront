using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.GetExistByNameAndGUID
{

    public class GetExistByNameAndGUIDQuery : IRequest<object>
    {
        public Guid? AgreementID { get; set; }
        public string? Name { get; set; }
        public int? WorkflowID { get; set; }
    }

}
