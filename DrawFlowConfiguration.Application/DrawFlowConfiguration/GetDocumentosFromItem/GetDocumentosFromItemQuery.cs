using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.GetDocumentosFromItem
{
    public class GetDocumentosFromItemQuery : IRequest<object>
    {
        public int WorkFlowID { get; set; }
    }
}
