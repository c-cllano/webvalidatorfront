using Process.Domain.Exceptions;
using System.Text;

namespace Process.Domain.Utilities
{
    public static class ImageHelper
    {
        public static string GetImageExtension(byte[] fileBytes)
        {
            if (fileBytes == null || fileBytes.Length < 4)
                return ".jpg";

            ReadOnlySpan<byte> bytes = fileBytes;

            if (bytes.StartsWith(new byte[] { 0xFF, 0xD8, 0xFF }))
                return ".jpg";

            if (bytes.StartsWith(new byte[] { 0x89, 0x50, 0x4E, 0x47 }))
                return ".png";

            if (bytes.StartsWith("GIF8"u8))
                return ".gif";

            if (bytes.StartsWith("BM"u8))
                return ".bmp";

            if (bytes.StartsWith(new byte[] { 0x49, 0x49, 0x2A, 0x00 }) ||
                bytes.StartsWith(new byte[] { 0x4D, 0x4D, 0x00, 0x2A }))
                return ".tiff";

            if (bytes.Length > 12 &&
                bytes.StartsWith("RIFF"u8) &&
                bytes.Slice(8, 4).SequenceEqual("WEBP"u8))
                return ".webp";

            if (bytes.Length > 12)
            {
                string ftype = Encoding.ASCII.GetString(fileBytes, 4, 8);
                if (ftype.Contains("ftypavif", StringComparison.OrdinalIgnoreCase))
                    return ".avif";
            }

            string text = Encoding.UTF8.GetString(fileBytes).TrimStart();
            if (text.StartsWith("<svg", StringComparison.OrdinalIgnoreCase))
                return ".svg";

            return ".jpg";
        }

        // Inicio código generado por GitHub Copilot
        // Método refactorizado por GitHub Copilot
        public static string GetMimeType(string extension)
        {
            return extension.ToLower() switch
            {
                // Imágenes
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".svg" => "image/svg+xml",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                ".bmp" => "image/bmp",
                ".tiff" => "image/tiff",
                ".tif" => "image/tiff",
                ".avif" => "image/avif",

                // Videos
                ".mp4" => "video/mp4",
                ".mov" => "video/quicktime",
                ".m4v" => "video/x-m4v",
                ".webm" => "video/webm",
                ".mkv" => "video/x-matroska",
                ".avi" => "video/x-msvideo",
                ".flv" => "video/x-flv",

                _ => "application/octet-stream"
            };
        }
        // Fin código generado por GitHub Copilot

        public static string[] GetImage(string image)
        {
            try
            {
                var publicKey = image[^130..];
                var hash = image.Substring((image.Length - (130 + 64)), 64);
                var tag = image.Substring((image.Length - (130 + 64 + 24)), 24);
                var iv = image.Substring((image.Length - (130 + 64 + 24 + 16)), 16);
                var ciphertext = image[..^234];

                string[] response =
                [
                    publicKey,
                    hash,
                    tag,
                    iv,
                    ciphertext
                ];

                return response;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"No se pudo obtener la imagen: {ex.InnerException?.Message ?? ex.Message}", 500);
            }
        }
    }
}
