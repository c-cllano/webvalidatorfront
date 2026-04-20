using Process.Application.UseCases.InterfacesUseCases;
using Process.Application.ValidationProcesses.SaveDocumentBothSides;
using Process.Domain.Parameters.ExternalApiClientParameters;
using Process.Domain.Services;

namespace Process.Application.UseCases.ServicesUseCases
{
    public class JumioUserCaseService(
        IJumioService jumioService
    ) : IJumioUserCaseService
    {
        private readonly IJumioService _jumioService = jumioService;

        public async Task<GetProcessJumioResponse> JumioAsync(SaveDocumentBothSidesCommand request)
        {
            TokenJumioResponse tokenJumio = await _jumioService.GetTokenJumioAsync()
                ?? throw new KeyNotFoundException("Error al obtener el token del servicio de JUMIO");

            ClientAccountJumioResponse clientJumio = await _jumioService.CreateClientJumioAsync(tokenJumio.AccessToken)
                ?? throw new KeyNotFoundException("Error al crear el cliente del servicio de JUMIO");

            string urlFrontal = clientJumio.WorkflowExecution.Credentials[0].Api.Parts.Front;
            string urlReverse = clientJumio.WorkflowExecution.Credentials[0].Api.Parts.Back;
            string urlFinalizedProcess = clientJumio.WorkflowExecution.Credentials[0].Api.WorkflowExecution;
            Guid guidWorkFlow = clientJumio.WorkflowExecution.Id;

            await _jumioService.ExecuteUrlsFrontBackAsync(tokenJumio.AccessToken, urlFrontal, request.Frontal.Value);

            if (request.Reverse != null)
            {
                await _jumioService.ExecuteUrlsFrontBackAsync(tokenJumio.AccessToken, urlReverse, request.Reverse.Value);
            }
            
            Guid workflowExecutionId = await _jumioService.ExecuteUrlFinalizedProcessAsync(tokenJumio.AccessToken, urlFinalizedProcess);

            GetProcessJumioResponse resultJumio = await _jumioService.GetResultJumioProcessAsync(tokenJumio.AccessToken, workflowExecutionId);

            await _jumioService.ExecuteDeleteUserInfoOnJumio(tokenJumio.AccessToken, guidWorkFlow);

            return resultJumio;
        }
    }
}
