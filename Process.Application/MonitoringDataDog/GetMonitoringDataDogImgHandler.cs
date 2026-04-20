using MediatR;
using Process.Domain.Parameters.MonitoringDataDog;
using Process.Domain.Repositories;

namespace Process.Application.MonitoringDataDog
{
    public class GetMonitoringDataDogImgHandler(IMonitoringDataDogImg monitoringDataDogImg) : IRequestHandler<GetMonitoringDataDogImgQuery, GetMonitoringDataDogImgResponse>
    {
        private readonly IMonitoringDataDogImg _monitoringDataDogImg = monitoringDataDogImg;
        public async Task<GetMonitoringDataDogImgResponse> Handle(GetMonitoringDataDogImgQuery request, CancellationToken cancellationToken)
        {
            List<string> parametros = ObtenerParametros(request.Type);
            IEnumerable<MonitoringDataDogResponse> response = await _monitoringDataDogImg.GetMonitoringDataDogImg(parametros);
            return new GetMonitoringDataDogImgResponse
            {
                Frontal = response.FirstOrDefault(x => x.ParameterName.Equals(parametros[0], StringComparison.OrdinalIgnoreCase))?.ParameterValue ?? "",
                Reverse = response.FirstOrDefault(x => x.ParameterName.Equals(parametros[1], StringComparison.OrdinalIgnoreCase))?.ParameterValue ?? ""
            };
        }

        public static List<string> ObtenerParametros(string Type)
        {
            List<string> parametros = [];
            switch (Type)
            {
                case "dataBiometria":
                    parametros.Add("dataBiometriaValue");
                    parametros.Add("dataBiometriaBiometricGesture");
                    break;
                case "dataValidacionBiometria":
                    parametros.Add("ValidacionBiometriaBiometric");
                    parametros.Add("ValidacionBiometriaBiometricGesture");
                    break;
                case "dataGuardarDocumento":
                    parametros.Add("GuardarDocumentofrontal");
                    parametros.Add("GuardarDocumentoreverse");
                    break;

            }
            return parametros;
        }

    }
}
