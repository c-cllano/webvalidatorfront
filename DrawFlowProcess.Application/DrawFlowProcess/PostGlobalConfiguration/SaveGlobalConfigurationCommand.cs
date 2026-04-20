using System.Text.Json;
using MediatR;

namespace DrawFlowProcess.Application.DrawFlowProcess.PostGlobalConfiguration
{
    public class SaveGlobalConfigurationCommand : IRequest<bool>
    {
        public JsonDocument? Document { get; set; }
    }
}
