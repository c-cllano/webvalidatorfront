using static Domain.Enums.Enumerations;

namespace Process.Domain.Services
{
    public interface IBlobStorageService
    {
        Task<(string, string)> DownloadFileAsBase64ByUrlAsync(string blobUrl);
        Task<string> UploadFileAsync(string containerGuid, AzureBlobStorageFolders folder, string fileBase64, bool convertSvgToPng = false);
        Task<string> UploadFilePathAsync(string filePath);
        Task<string> UploadMediaAsync(string containerGuid, AzureBlobStorageFolders folder, Stream fileStream, string fileName);
    }
}
