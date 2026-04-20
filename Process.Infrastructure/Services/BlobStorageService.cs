using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Process.Domain.Exceptions;
using Process.Domain.Services;
using Process.Domain.Utilities;
using System.Security.Cryptography;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Services
{
    public class BlobStorageService(
        IConfiguration configuration,
        BlobContainerClient container,
        IEncryptImageBlobService encryptImageBlobService
    ) : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient = new(configuration.GetConnectionString("AzureBlobStorage"));
        private readonly BlobContainerClient _container = container;
        private readonly IEncryptImageBlobService _encryptImageBlobService = encryptImageBlobService;

        public async Task<string> UploadFilePathAsync(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            string blobName = $"{DateTime.UtcNow:yyyy-MM-dd}/{fileName}";

            var blobClient = _container.GetBlobClient(blobName);

            var options = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = "application/json" }
            };

            await blobClient.UploadAsync(filePath, options);

            return blobName;
        }

        public async Task<(string, string)> DownloadFileAsBase64ByUrlAsync(string blobUrl)
        {
            var blobUri = new Uri(blobUrl);
            var blobUriBuilder = new BlobUriBuilder(blobUri);

            var containerClient = _blobServiceClient.GetBlobContainerClient(blobUriBuilder.BlobContainerName);
            var blobClient = containerClient.GetBlobClient(blobUriBuilder.BlobName);

            using var ms = new MemoryStream();
            await blobClient.DownloadToAsync(ms);

            string extension = Path.GetExtension(blobUriBuilder.BlobName)?.ToLowerInvariant() ?? ".jpg";
            string mimeType = ImageHelper.GetMimeType(extension);

            string base64 = Convert.ToBase64String(ms.ToArray());
            base64 = await _encryptImageBlobService.DecryptImageAsync(base64);

            return (base64, mimeType);
        }

        // Inicio código generado por GitHub Copilot
        public async Task<string> UploadMediaAsync(
            string containerGuid, 
            AzureBlobStorageFolders folder, 
            Stream fileStream, 
            string fileName
        )
        {
            var parts = containerGuid.Split('/', StringSplitOptions.RemoveEmptyEntries);
            string containerName = parts[0];
            string subFolder = parts.Length > 1 ? parts[1] : "";

            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            string extension = Path.GetExtension(fileName).ToLowerInvariant();
            string mimeType = ImageHelper.GetMimeType(extension);

            string basePath = !string.IsNullOrEmpty(subFolder)
                ? $"{subFolder}/{folder}/{Guid.NewGuid()}{extension}"
                : $"{folder}/{Guid.NewGuid()}{extension}";

            BlobClient blobClient = containerClient.GetBlobClient(basePath);

            await blobClient.UploadAsync(
                fileStream,
                new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders { ContentType = mimeType }
                });

            return blobClient.Uri.ToString();
        }
        // Fin código generado por GitHub Copilot

        public async Task<string> UploadFileAsync(
            string containerGuid,
            AzureBlobStorageFolders folder,
            string fileBase64,
            bool convertSvgToPng = false
        )
        {
            var parts = containerGuid.Split('/', StringSplitOptions.RemoveEmptyEntries);
            string containerName = parts[0];
            string subFolder = parts.Length > 1 ? parts[1] : "";

            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            string processedBase64 = await _encryptImageBlobService.EncryptImageAsync(fileBase64);

            var base64InputStream = new MemoryStream();
            try
            {
                await using (var writer = new StreamWriter(base64InputStream, leaveOpen: true))
                {
                    await writer.WriteAsync(processedBase64);
                    await writer.FlushAsync();
                }

                base64InputStream.Position = 0;

                byte[] headerBuffer = DecodeBase64Header(fileBase64, 32);
                string originalExtension = ImageHelper.GetImageExtension(headerBuffer);

                string basePath = !string.IsNullOrEmpty(subFolder)
                    ? $"{subFolder}/{folder}/{Guid.NewGuid()}"
                    : $"{folder}/{Guid.NewGuid()}";

                base64InputStream.Position = 0;

                await using var decodedStream = new CryptoStream(
                    base64InputStream,
                    new FromBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces),
                    CryptoStreamMode.Read);

                string originalFileName = $"{basePath}{originalExtension}";
                BlobClient originalBlob = containerClient.GetBlobClient(originalFileName);

                await originalBlob.UploadAsync(
                    decodedStream,
                    new BlobUploadOptions
                    {
                        HttpHeaders = new BlobHttpHeaders
                        {
                            ContentType = ImageHelper.GetMimeType(originalExtension)
                        }
                    });

                if (convertSvgToPng && originalExtension.Equals(".svg", StringComparison.OrdinalIgnoreCase))
                {
                    base64InputStream.Position = 0;
                    await using var svgDecodedStream = new CryptoStream(
                        base64InputStream,
                        new FromBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces),
                        CryptoStreamMode.Read);
                    await using var pngStream = await SvgConverter.ConvertSvgToPngAsync(svgDecodedStream);
                    string pngFileName = $"{basePath}.png";
                    BlobClient pngBlob = containerClient.GetBlobClient(pngFileName);
                    await pngBlob.UploadAsync(
                        pngStream,
                        new BlobUploadOptions
                        {
                            HttpHeaders = new BlobHttpHeaders
                            {
                                ContentType = "image/png"
                            }
                        });
                }

                return originalBlob.Uri.ToString();
            }
            catch (FormatException)
            {
                throw new GenericException("No fue posible procesar la imagen enviada. Por favor, intenta nuevamente o contacta a soporte si el problema persiste.");
            }
            finally
            {
                await base64InputStream.DisposeAsync();
            }
        }

        private static byte[] DecodeBase64Header(string base64, int maxBytes)
        {
            int maxBase64Chars = (int)Math.Ceiling(maxBytes / 3.0) * 4;

            if (base64.Length < maxBase64Chars)
                maxBase64Chars = base64.Length;

            string headerBase64 = base64[..maxBase64Chars];

            return Convert.FromBase64String(headerBase64);
        }
    }
}
