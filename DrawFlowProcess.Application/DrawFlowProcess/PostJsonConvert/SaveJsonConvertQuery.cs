using System.Text.Json;
using MediatR;

namespace DrawFlowProcess.Application.DrawFlowProcess.PostJsonConvert
{
    public class SaveJsonConvertQuery : IRequest<bool>
    {
        public JsonDocument? Document { get; set; }
    }
}
