using MediatR;
using Process.Domain.Parameters.PersonalizationAgreement;
using Process.Domain.Repositories;
using Process.Domain.Services;
using static Domain.Enums.Enumerations;

namespace Process.Application.AgreementProcess.GetProcess
{
    public class GetProcessHandler(
        IAgreementProcessRepository repository,
        IPersonalizationAgreementRepository personalizationAgreementRepository,
        IAuthService authService) : IRequestHandler<GetProcessQuery, GetProcessResponse>
    {
        private readonly IAgreementProcessRepository _repository = repository;
        private readonly IPersonalizationAgreementRepository _personalizationAgreementRepository = personalizationAgreementRepository;
        private readonly IAuthService _authService = authService;

        private const string ErrorProcesoNoEncontrado = "Proceso no encontrado, por favor valida con su proveedor de servicio.";
        private const string ErrorRevisionForence = "Por favor espere unos minutos.";
        private const string ErrorProcesoNoDisponible = "Proceso no disponible.";
        private const string ErrorTiempoCaducado = "El tiempo límite para realizar la validación de identidad ha caducado, por favor solicita un nuevo proceso de validación de identidad con su proveedor de servicio.";
        private const string ErrorIdentityServer = "Identity Server no disponible.";
        private const string ErrorProcesoCancelado = "Su proceso ha sido cancelado, por favor solicita un nuevo proceso de validación de identidad con su proveedor de servicio.";
        private const string MensajeProcesoFinalizado = "Su proceso ya ha finalizado. El resultado de la validación de identidad ya fue notificado a su proveedor de servicio.";

        public async Task<GetProcessResponse> Handle(GetProcessQuery command, CancellationToken cancellationToken)
        {
            var response = new GetProcessResponse();
            var aggregamentProcess = await _repository.GetProcess(command.ProcesoConvenioGuid);

            if (aggregamentProcess is null)
                return CreateErrorResponse(response, ErrorProcesoNoEncontrado);


            if (aggregamentProcess.RevisionForence == 1)
                return CreateErrorResponse(response, ErrorRevisionForence);

            if (aggregamentProcess.AppFrontGuid != null && aggregamentProcess.AppFrontGuid != command.AppFrontGuid)
                return CreateErrorResponse(response, ErrorProcesoNoDisponible);


            var personalizacion = await _personalizationAgreementRepository.GetFrontConfiguration(aggregamentProcess.ConvenioId!.Value);

            if (personalizacion == null)
                return CreateErrorResponse(response, ErrorProcesoNoEncontrado);

            if (IsLinkExpired(personalizacion, aggregamentProcess.CreatedAt))
                return CreateErrorResponse(response, ErrorTiempoCaducado);

            if (!aggregamentProcess.Finalizado.GetValueOrDefault() && !aggregamentProcess.Cancelado.GetValueOrDefault())
                return await HandleActiveProcess(response, command);

            return HandleFinalizedOrCancelledProcess(response, aggregamentProcess, personalizacion);
        }

        private async Task<GetProcessResponse> HandleActiveProcess(GetProcessResponse response, GetProcessQuery command)
        {
            var token = await _authService.GetToken("Angular", "5UebhcVqdiSGeMqW431DF");

            if (token.AccessToken != null)
            {
                string newToken = await _authService.GetNewToken(token.AccessToken, command.ProcesoConvenioGuid);
                response.IsValid = true;
                response.Token = newToken;
                return response;
            }

            return CreateErrorResponse(response, ErrorIdentityServer);
        }

        private static GetProcessResponse HandleFinalizedOrCancelledProcess(GetProcessResponse response,
            Domain.Entities.AgreementProcess process,
            List<GetFrontConfigurationOut> personalizacion)
        {
            return process switch
            {
                { Finalizado: true, Cancelado: false } => CreateErrorResponse(response, GetFinalizedMessage(personalizacion)),
                { Finalizado: true, Cancelado: true } => CreateErrorResponse(response, ErrorProcesoCancelado),
                _ => response
            };
        }

        private static bool IsLinkExpired(List<GetFrontConfigurationOut> personalizacion, DateTime? createdAt)
        {
            var linkExp = personalizacion.Find(p => p.PersonalizacionId == (long)PersonalizacionesEnum.LinkExpiration);
            if (linkExp == null || !createdAt.HasValue) return false;

            var diffMinutes = DateTime.Now.Subtract(createdAt.Value).TotalMinutes;
            return diffMinutes > Convert.ToInt32(linkExp.Valor) * 60;
        }

        private static string GetFinalizedMessage(List<GetFrontConfigurationOut> personalizacion)
        {
            var textoFinal = personalizacion.Find(p => p.PersonalizacionId == (long)PersonalizacionesEnum.TextoFinalValidador);
            return textoFinal?.Valor ?? MensajeProcesoFinalizado;
        }

        private static GetProcessResponse CreateErrorResponse(GetProcessResponse response, string errorMessage)
        {
            response.IsValid = false;
            response.Error = errorMessage;
            return response;
        }
    }
}
