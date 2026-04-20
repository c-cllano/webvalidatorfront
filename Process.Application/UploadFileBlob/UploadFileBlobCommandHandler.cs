using MediatR;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Domain.Services;
using static Domain.Enums.Enumerations;

namespace Process.Application.UploadFileBlob
{
    public class UploadFileBlobCommandHandler(
        IAgreementOkeyStudioRepository agreementOkeyStudioRepository,
        IBlobStorageService blobStorageService
    ) : IRequestHandler<UploadFileBlobCommand, UploadFileBlobResponse>
    {
        private readonly IAgreementOkeyStudioRepository _agreementOkeyStudioRepository = agreementOkeyStudioRepository;
        private readonly IBlobStorageService _blobStorageService = blobStorageService;

        public async Task<UploadFileBlobResponse> Handle(UploadFileBlobCommand request, CancellationToken cancellationToken)
        {
            AgreementOkeyStudio? agreementOkeyStudio = await _agreementOkeyStudioRepository.GetAgreementById(request.AgreementId)
                ?? throw new KeyNotFoundException("El convenio no fue encontrado.");

            string urlBobStorage = await _blobStorageService
                .UploadFileAsync($"{AzureBlobStorageFolders.UIFiles.ToString().ToLower()}/{agreementOkeyStudio.AgreementGUID}", AzureBlobStorageFolders.UIFiles, request.FileBase64,true);

            return new()
            {
                UrlBlobStorage = urlBobStorage
            };
        }
    }
}
