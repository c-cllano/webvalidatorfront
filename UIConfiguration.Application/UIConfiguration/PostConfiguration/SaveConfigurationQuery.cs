using MediatR;

namespace UIConfiguration.Application.UIConfiguration.PostConfiguration
{
    public class SaveConfigurationQuery: IRequest<object>
    {
        public object UIConfigurationJson { get; set; } = default!;
    }
}
