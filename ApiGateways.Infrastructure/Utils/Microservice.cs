using System.ComponentModel;

namespace ApiGateways.Infrastructure.Utils
{
    public static class Microservice
    {
        [Description("Api De Proceso")]
        public const string ProcessAPI = "Process.API";
        [Description("Api De Configuración De Interfaz De Usuario")]
        public const string UIConfigurationAPI = "UIConfiguration.API";
        [Description("Api De Dactyloscopy")]
        public const string DactyloscopyAPI = "Dactyloscopy.API";
        [Description("Api de Proceso de flujo de dibujo")]
        public const string DrawFlowProcessAPI = "DrawFlowProcess.API";
        [Description("Api de Configuración de flujo de dibujo")]
        public const string DrawFlowConfigurationAPI = "DrawFlowConfiguration.API";
        [Description("Api de plantilla de interfaz de usuario")]
        public const string UITemplateAPI = "UITemplate.API";
        [Description("Api de firma digital")]
        public const string DigitalSignatureAPI = "DigitalSignature.API";
    }
}
