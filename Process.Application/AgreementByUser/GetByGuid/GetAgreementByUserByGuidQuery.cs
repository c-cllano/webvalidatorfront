using MediatR;
using Process.Application.AgreementByUser.GetByUserId;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process.Application.AgreementByUser.GetByGuid
{

    public class GetAgreementByUserByGuidQuery : IRequest<IEnumerable<GetByGuidQueryResponse>>
    {
        public Guid Guid { get; set; }
    }
}
