namespace Dactyloscopy.Domain.Exceptions
{
    public class IntegrationException(
        string message
    ) : Exception(message)
    {
    }
}
