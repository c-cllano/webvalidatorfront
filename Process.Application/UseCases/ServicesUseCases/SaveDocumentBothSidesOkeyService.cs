using F23.StringSimilarity;
using Process.Application.UseCases.Attributes;
using Process.Application.UseCases.InterfacesUseCases;
using Process.Application.UseCases.Responses;
using Process.Application.ValidationProcesses.SaveDocumentBothSides;
using Process.Domain.Entities;
using Process.Domain.Parameters.ExternalApiClientParameters;
using Process.Domain.Parameters.ProcessDocuments;
using Process.Domain.Repositories;
using Process.Domain.Services;
using Process.Domain.Utilities;
using static Domain.Enums.Enumerations;

namespace Process.Application.UseCases.ServicesUseCases
{
    [PlatformConnection(Constants.OkeyStudio)]
    public class SaveDocumentBothSidesOkeyService(
        IBlobStorageService blobStorageService,
        ICitizenRepository citizenRepository,
        IVerIdService verIdService,
        IDocumentTypeRepository documentTypeRepository,
        ICitizenBiometricsDocumentsRepository biometricsRepository,
        IJumioUserCaseService jumioUserCaseService,
        IConfigurationService configurationService,
        IValidationProcessDocumentsRepository validationProcessDocumentsRepository,
        IValidationProcessScoresRepository scoreRepository
    ) : ISaveDocumentBothSidesService
    {
        private readonly IBlobStorageService _blobStorageService = blobStorageService;
        private readonly ICitizenRepository _citizenRepository = citizenRepository;
        private readonly IVerIdService _verIdService = verIdService;
        private readonly IDocumentTypeRepository _documentTypeRepository = documentTypeRepository;
        private readonly ICitizenBiometricsDocumentsRepository _biometricsRepository = biometricsRepository;
        private readonly IJumioUserCaseService _jumioUserCaseService = jumioUserCaseService;
        private readonly IConfigurationService _configurationService = configurationService;
        private readonly IValidationProcessDocumentsRepository _validationProcessDocumentsRepository = validationProcessDocumentsRepository;
        private readonly IValidationProcessScoresRepository _scoreRepository = scoreRepository;

        public async Task<SaveDocumentBothSidesResponse> SaveDocumentBothSidesAsync(
            SaveDocumentBothSidesCommand request,
            ValidationProcess validationProcess
        )
        {
            Citizen? citizen = await _citizenRepository.GetCitizenByGuidAsync(request.CitizenGUID)
                ?? throw new KeyNotFoundException($"No existe ciudadano con guid: {request.CitizenGUID}");

            DocumentType? documentType = await _documentTypeRepository
                .GetDocumentTypeById(citizen.DocumentTypeId!.Value)
                    ?? throw new KeyNotFoundException($"No existe tipo de documento con id: {citizen.DocumentTypeId}");

            string urlFrontal = await _blobStorageService
                .UploadFileAsync(validationProcess.AgreementGUID.ToString()!, AzureBlobStorageFolders.ValidationFiles, request.Frontal.Value);

            await CreateValidationProcessDocuments(request, urlFrontal, (int)ServicioSubTipoEnum.ANVERSO);

            string urlReverse = string.Empty;

            if (request.Reverse != null)
            {
                urlReverse = await _blobStorageService
                    .UploadFileAsync(validationProcess.AgreementGUID.ToString()!, AzureBlobStorageFolders.ValidationFiles, request.Reverse.Value);

                await CreateValidationProcessDocuments(request, urlReverse, (int)ServicioSubTipoEnum.REVERSO);
            }

            SaveDocumentBothSidesResponse resultDocumentValidation = await ValidationDocumentAsync(citizen, documentType, request, urlFrontal, urlReverse);

            if (resultDocumentValidation != null)
            {
                return resultDocumentValidation;
            }

            (Guid frontalGuid, Guid reverseGuid) = await SaveDocumentCitizenAsync(citizen.CitizenId, urlFrontal, urlReverse);

            return new()
            {
                FrontalGUID = frontalGuid,
                ReverseGUID = reverseGuid,
                FrontalSuccessful = true,
                ReverseSuccessful = true,
                FrontalMessage = "Ok",
                ReverseMessage = "Ok",
                DocumentTypeDescription = documentType.Name,
                DocumentType = documentType.Code,
                DocumentNumber = citizen.DocumentNumber,
                FirstName = citizen.FirstName,
                SecondName = citizen.SecondName,
                LastName = citizen.LastName,
                SecondLastName = citizen.SecondLastName,
                Sex = null,
                Rh = null,
                PlaceBirth = citizen.Location,
                DateBirth = citizen.BirthDate,
                IsHomologation = true,
                IsSuccessful = true,
                PlaceOfIssue = citizen.Location,
                DateOfIssue = citizen.DocumentIssueDate,
                TransactionError = []
            };
        }

        private async Task CreateValidationProcessDocuments(
            SaveDocumentBothSidesCommand request,
            string urlFace,
            int subType
        )
        {
            ProcessDocumentsRequest processDocumentsRequest = new()
            {
                UrlFile = urlFace,
                ProcessName = nameof(SaveDocumentBothSidesOkeyService),
                ServiceType = (int)ServiciosEnum.BIOMETRIA_DOCUMENTO,
                ServiceSubType = subType,
                Date = DateTime.UtcNow.AddHours(-5)
            };

            await _validationProcessDocumentsRepository
                .SaveValidationProcessDocumentsAsync(request.ValidationProcessId, processDocumentsRequest);
        }

        private async Task<SaveDocumentBothSidesResponse> ValidationDocumentAsync(
            Citizen citizen,
            DocumentType documentType,
            SaveDocumentBothSidesCommand request,
            string urlFrontal,
            string urlReverse
        )
        {
            DocumentValidationResponse resultProvider = documentType.Code!.Equals(Constants.CC)
                ? await ExecuteVerIdAsync(request, urlFrontal, urlReverse)
                : await ExecuteJumioAsync(request);

            await _scoreRepository.SaveValidationProcessScoresAsync(request.ValidationProcessId, (decimal)resultProvider.Score, ScoresCode.VerIdScore);

            if (resultProvider.SaveDocumentBothSidesResponse != null)
            {
                return resultProvider.SaveDocumentBothSidesResponse;
            }

            float score = documentType.Code!.Equals(Constants.CC)
                ? float.Parse(_configurationService.GetConfiguration("ScoresValidation:ScoreVerId"))
                : float.Parse(_configurationService.GetConfiguration("ScoresValidation:ScoreJumio"));

            if (resultProvider.Score > score)
            {
                string messageError = ConstantsOkey.GetMessageError("6000");

                return new()
                {
                    FrontalSuccessful = false,
                    ReverseSuccessful = false,
                    FrontalMessage = messageError,
                    ReverseMessage = messageError,
                    IsHomologation = false,
                    IsSuccessful = false,
                    TransactionError = [
                        new ErrorTransactionResponse(){
                            Code = "6000",
                            Description = messageError
                        }
                    ]
                };
            }

            if (!citizen.DocumentNumber!.Equals(resultProvider.DocumentNumber))
            {
                string messageError = ConstantsOkey.GetMessageError("7000");

                return new()
                {
                    FrontalSuccessful = false,
                    ReverseSuccessful = false,
                    FrontalMessage = messageError,
                    ReverseMessage = messageError,
                    IsHomologation = false,
                    IsSuccessful = false,
                    TransactionError = [
                        new ErrorTransactionResponse(){
                            Code = "7000",
                            Description = messageError
                        }
                    ]
                };
            }

            double percentageSimilarityNames = double.Parse(_configurationService.GetConfiguration("ScoresValidation:PercentageSimilarityNames"));
            string nameCitizen = $"{citizen.FirstName} {citizen.SecondName} {citizen.LastName} {citizen.SecondLastName}";
            nameCitizen = nameCitizen.ToLower().Trim();
            string nameServiceProvider = resultProvider.Name.ToLower().Trim();

            var jaroWinkler = new JaroWinkler();
            double similarityScore = jaroWinkler.Similarity(nameCitizen, nameServiceProvider) * 100;

            await _scoreRepository.SaveValidationProcessScoresAsync(request.ValidationProcessId, (decimal)similarityScore, ScoresCode.DatosDocumentoVsDatosOficiales);

            if (similarityScore < percentageSimilarityNames)
            {
                string messageError = ConstantsOkey.GetMessageError("8000");

                return new()
                {
                    FrontalSuccessful = false,
                    ReverseSuccessful = false,
                    FrontalMessage = messageError,
                    ReverseMessage = messageError,
                    IsHomologation = false,
                    IsSuccessful = false,
                    TransactionError = [
                        new ErrorTransactionResponse(){
                            Code = "8000",
                            Description = messageError
                        }
                    ]
                };
            }

            return null!;
        }

        private async Task<DocumentValidationResponse> ExecuteJumioAsync(
            SaveDocumentBothSidesCommand request
        )
        {
            GetProcessJumioResponse response = await _jumioUserCaseService.JumioAsync(request);

            return new()
            {
                Score = response!.Decision.Risk.Score,
                DocumentNumber = response.Capabilities.Extraction[0].Data.DocumentNumber ?? string.Empty,
                Name = $"{response.Capabilities.Extraction[0].Data.FirstName} {response.Capabilities.Extraction[0].Data.LastName}"
            };
        }

        private async Task<DocumentValidationResponse> ExecuteVerIdAsync(
            SaveDocumentBothSidesCommand request,
            string urlFrontal,
            string urlReverse
        )
        {
            object contentBodyExternalRequest = CreateObject(urlFrontal, urlReverse);

            if (request.Reverse == null)
            {
                throw new KeyNotFoundException($"Se debe enviar el reverso del documento");
            }

            VerIdResponse response = await _verIdService
                .VerIdAsync(request.Frontal.Value, request.Reverse!.Value, contentBodyExternalRequest);

            if (response != null && !ConstantsVerId.ValidCodesOk.Contains(response.Summary.Code))
            {
                string messageError = ConstantsVerId.GetMessage(response.Summary.Code);

                SaveDocumentBothSidesResponse entity = new()
                {
                    FrontalSuccessful = response.Summary.From == "front_img" ? false : null,
                    ReverseSuccessful = response.Summary.From == "back_img" ? false : null,
                    FrontalMessage = response.Summary.From == "front_img" ? messageError : null,
                    ReverseMessage = response.Summary.From == "back_img" ? messageError : null,
                    IsHomologation = false,
                    IsSuccessful = false,
                    TransactionError = [
                        new ErrorTransactionResponse(){
                            Code = response.Summary.Code,
                            Description = messageError
                        }
                    ]
                };

                return new()
                {
                    SaveDocumentBothSidesResponse = entity
                };
            }

            return new()
            {
                Score = response!.Summary.RiskScore,
                DocumentNumber = response.GlobalResult.ExtractedData.ExtractedOcr.FrontImg.Numero,
                Name = $"{response.GlobalResult.ExtractedData.ExtractedOcr.FrontImg.PrimerNombre} " +
                       $"{response.GlobalResult.ExtractedData.ExtractedOcr.FrontImg.SegundoNombre} " +
                       $"{response.GlobalResult.ExtractedData.ExtractedOcr.FrontImg.PrimerApellido} " +
                       $"{response.GlobalResult.ExtractedData.ExtractedOcr.FrontImg.SegundoApellido}"
            };
        }

        private async Task<(Guid, Guid)> SaveDocumentCitizenAsync(
            long citizenId,
            string urlFrontal,
            string urlReverse
        )
        {
            CitizenBiometricsDocuments? biometricFrontal = await _biometricsRepository
                .GetBiometricsByCitizenIdAndServiceTypeAsync(citizenId, (int)ServiciosEnum.BIOMETRIA_DOCUMENTO, (int)ServicioSubTipoEnum.FRONTAL);

            biometricFrontal = biometricFrontal is null
                ? await SaveBiometricAsync(citizenId, urlFrontal, (int)ServicioSubTipoEnum.FRONTAL)
                : await UpdateBiometricAsync(biometricFrontal.CitizenBiometricsDocumentsId, urlFrontal);

            CitizenBiometricsDocuments? biometricReverse = await _biometricsRepository
                .GetBiometricsByCitizenIdAndServiceTypeAsync(citizenId, (int)ServiciosEnum.BIOMETRIA_DOCUMENTO, (int)ServicioSubTipoEnum.REVERSO);

            biometricReverse = biometricReverse is null
                ? await SaveBiometricAsync(citizenId, urlReverse, (int)ServicioSubTipoEnum.REVERSO)
                : await UpdateBiometricAsync(biometricReverse.CitizenBiometricsDocumentsId, urlReverse);

            return (biometricFrontal.CitizenBiometricsDocumentsGuid, biometricReverse.CitizenBiometricsDocumentsGuid);
        }

        private async Task<CitizenBiometricsDocuments> SaveBiometricAsync(
            long citizenId,
            string urlFile,
            int serviceSubType
        )
        {
            CitizenBiometricsDocuments biometrics = new()
            {
                CitizenBiometricsDocumentsGuid = Guid.NewGuid(),
                CitizenId = citizenId,
                UrlFile = urlFile,
                ServiceType = (int)ServiciosEnum.BIOMETRIA_DOCUMENTO,
                ServiceSubType = serviceSubType,
                CreatedDate = DateTime.UtcNow.AddHours(-5),
                Active = true,
                IsDeleted = false
            };

            return await _biometricsRepository.SaveBiometricsAsync(biometrics);
        }

        private async Task<CitizenBiometricsDocuments> UpdateBiometricAsync(
            long citizenBiometricsDocumentsId,
            string urlFile
        )
        {
            return await _biometricsRepository.UpdateFileAsync(citizenBiometricsDocumentsId, urlFile);
        }

        private static object CreateObject(
            string urlBiometric,
            string urlBiometricGesture
        )
        {
            object contentBodyExternalRequest = new
            {
                multipartContentFile1 = urlBiometric,
                multipartContentFile2 = urlBiometricGesture
            };

            return contentBodyExternalRequest;
        }
    }
}
