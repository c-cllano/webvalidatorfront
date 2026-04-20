namespace Process.Application.UseCases.InterfacesUseCases
{
    public interface IExtractImageService
    {
        Task<string> ExtractImageAsync(Guid agreementGuid, Guid validationProcessGuid, string image);
    }
}
