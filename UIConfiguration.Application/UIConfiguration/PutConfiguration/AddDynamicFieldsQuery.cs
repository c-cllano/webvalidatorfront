using MediatR;

namespace UIConfiguration.Application.UIConfiguration.PutConfiguration
{
    public record AddDynamicFieldsQuery(
    Guid ProcesoConvenioGuid,
    int WorkflowID,
    Dictionary<string, object> FieldsToAdd
) : IRequest<object?>;
}
