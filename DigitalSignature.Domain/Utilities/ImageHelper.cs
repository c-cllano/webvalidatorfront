namespace DigitalSignature.Domain.Utilities
{
    public static class ImageHelper
    {
        public static string GetImageExtension(byte[] fileBytes)
        {
            if (fileBytes.Length < 4) return ".jpg";

            if (fileBytes[0] == 0xFF && fileBytes[1] == 0xD8 && fileBytes[2] == 0xFF)
                return ".jpg";

            if (fileBytes[0] == 0x89 && fileBytes[1] == 0x50 && fileBytes[2] == 0x4E && fileBytes[3] == 0x47)
                return ".png";

            return ".jpg";
        }

        public static string GetMimeType(string extension)
        {
            return extension switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };
        }
    }
}
