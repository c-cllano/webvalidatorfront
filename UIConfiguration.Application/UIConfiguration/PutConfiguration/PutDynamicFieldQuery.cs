using MediatR;

namespace UIConfiguration.Application.UIConfiguration.PutConfiguration
{
    public record PutDynamicFieldQuery(
        Guid ProcesoConvenioGuid,
        int WorkflowID,
        Dictionary<string, object> FieldsToUpdate
    ) : IRequest<object?>;
}
