using Microsoft.AspNetCore.Http;

namespace Process.Infrastructure.Utility
{
    public static class FormFileExtensions
    {
        public static async Task<byte[]> ToBytesAsync(this IFormFile file)
        {
            await using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}
