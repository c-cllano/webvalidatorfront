using MediatR;

namespace UIConfiguration.Application.UIConfiguration.PutConfiguration
{
    public class AddIfNotExistsToAllQuery : IRequest<object>
    {
        public Dictionary<string, object> FieldsToAdd { get; }

        public AddIfNotExistsToAllQuery(Dictionary<string, object> fieldsToAdd)
        {
            FieldsToAdd = fieldsToAdd ?? throw new ArgumentNullException(nameof(fieldsToAdd));
        }
    }
}
