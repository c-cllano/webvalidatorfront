using SkiaSharp;
using Svg.Skia;
using System.Text;

namespace Process.Domain.Utilities
{
    public static class SvgConverter
    {
        public static byte[] ConvertSvgToPng(byte[] svgBytes)
        {
            string svgContent = Encoding.UTF8.GetString(svgBytes);
            var svg = new SKSvg();
            svg.FromSvg(svgContent);
            var picture = svg.Picture ?? throw new InvalidDataException("El contenido SVG no es válido o no se pudo analizar.");
            var info = new SKImageInfo((int)picture.CullRect.Width, (int)picture.CullRect.Height);
            using var surface = SKSurface.Create(info);
            var canvas = surface.Canvas;
            canvas.Clear(SKColors.Transparent);
            canvas.DrawPicture(picture);
            canvas.Flush();
            using SKImage image = surface.Snapshot();
            using SKData data = image.Encode(SKEncodedImageFormat.Png, 100);
            return data.ToArray();
        }

        public static async Task<Stream> ConvertSvgToPngAsync(Stream svgStream)
        {
            if (svgStream == null)
                throw new ArgumentNullException(nameof(svgStream));
            using var reader = new StreamReader(svgStream, Encoding.UTF8, leaveOpen: true);
            string svgContent = await reader.ReadToEndAsync();
            var svg = new SKSvg();
            svg.FromSvg(svgContent);
            var picture = svg.Picture
                ?? throw new InvalidDataException("El contenido SVG no es válido o no se pudo analizar.");
            var width = (int)Math.Ceiling(picture.CullRect.Width);
            var height = (int)Math.Ceiling(picture.CullRect.Height);
            var info = new SKImageInfo(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
            using var surface = SKSurface.Create(info);
            var canvas = surface.Canvas;
            canvas.Clear(SKColors.Transparent);
            canvas.DrawPicture(picture);
            canvas.Flush();
            using var image = surface.Snapshot();
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            return new MemoryStream(data.ToArray());
        }

    }
}
