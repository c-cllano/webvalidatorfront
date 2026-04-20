using MediatR;

namespace DrawFlowConfiguration.Application.ScreenFlow.GetScreenFlowByFilter
{


    public class GetScreenFlowByFilterQuery : IRequest<object>
    {
        public int? ScreenFlowID { get; set; }

        public Guid? AgreementId { get; set; }

        public int? SelectedIdWorkflow { get; set; }


    }
}
