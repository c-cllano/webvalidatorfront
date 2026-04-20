using MediatR;

namespace Process.Application.UploadFileBlob
{
    public record UploadFileBlobCommand(
        long AgreementId,
        string FileBase64
    ) : IRequest<UploadFileBlobResponse>;
}
