namespace DigitalSignature.Domain.Enums
{
    public class Enumerations
    {
        public enum ApiName
        {
            ExternalApi,
            BaseUrlEmissionDocument,
            BaseUrlSignature,
            LoginEmission,
            LoginSignature,
            CreateTemplate,
            UpdateTemplate,
            GetTemplate,
            GetTemplateFields,
            GenerateDocument,
            Sign
        }

        public enum ConnectionsName
        {
            OKeyConnection,
            DefaultConnection
        }
    }
}
