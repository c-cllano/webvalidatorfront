using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.GetNationalitiesFromItem
{

    public class GetNationalitiesFromItemQuery : IRequest<object>
    {
        public int WorkFlowID { get; set; }
    }
}
