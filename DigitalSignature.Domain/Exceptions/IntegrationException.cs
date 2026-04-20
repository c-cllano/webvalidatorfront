namespace DigitalSignature.Domain.Exceptions
{
    public class IntegrationException(
        string message
    ) : Exception(message)
    {
    }
}
