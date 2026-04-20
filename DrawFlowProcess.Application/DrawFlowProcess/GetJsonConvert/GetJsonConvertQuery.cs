using DrawFlowProcess.Domain.Domain;
using MediatR;
using System.Text.Json;

namespace DrawFlowProcess.Application.DrawFlowProcess.GetJsonConvert
{
    public class GetJsonConvertQuery : IRequest<ExportJson>
    {
        public Guid ProcesoConvenioGuid { get; set; }
        public JsonDocument JsonDocument { get; set; } = default!;
    }
}
