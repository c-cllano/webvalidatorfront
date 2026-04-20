using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawFlowConfiguration.Domain.Parameters.DrawFlow.Request;
using DrawFlowConfiguration.Domain.Parameters.DrawFlow;
using DrawFlowConfiguration.Domain.Parameters.ScreenFlow.Request;
using DrawFlowConfiguration.Domain.Parameters.ScreenFlow.Response;

namespace DrawFlowConfiguration.Domain.Repositories
{
    public interface IScreenFlowRepository
    {

        public Task<bool> SaveScreenFlow(ScreenFlowRequest request);

        public Task<IEnumerable<ScreenFlowResponse>> GetAllScreenFlow();

        public Task<bool> UpdateScreenFlow(ScreenFlowResponse request);

        public Task<IEnumerable<ScreenFlowResponse>> GetScreenFlowByFilter(int? screenFlowId = null, Guid? agreementId = null, int? selectedIdWorkflow = null);


    }
}
