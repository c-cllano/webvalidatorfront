using MediatR;

namespace UIConfiguration.Application.UIConfiguration.PutConfiguration
{ 
        public record AddIfNotExistsQuery(
        Guid ProcesoConvenioGuid,
        int WorkflowID,
        Dictionary<string, object> FieldsToAdd
    ) : IRequest<object?>;
}
