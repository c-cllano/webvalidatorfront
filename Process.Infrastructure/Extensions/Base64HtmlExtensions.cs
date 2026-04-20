using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process.Infrastructure.Extensions
{
    public static class Base64HtmlExtensions
    {
        /// <summary>
        /// Convierte un string Base64 en un archivo HTML guardado en el disco.
        /// </summary>
        /// <param name="base64Html">Contenido HTML codificado en Base64.</param>
        /// <param name="filePath">Ruta completa donde se guardará el archivo .html.</param>
        public static void ToHtmlFile(this string base64Html, string filePath)
        {
            if (string.IsNullOrWhiteSpace(base64Html))
                throw new ArgumentException("El contenido Base64 no puede estar vacío.", nameof(base64Html));

            byte[] htmlBytes;

            try
            {
                htmlBytes = Convert.FromBase64String(base64Html);
            }
            catch (FormatException)
            {
                throw new FormatException("El contenido proporcionado no es un Base64 válido.");
            }

            File.WriteAllBytes(filePath, htmlBytes);
        }

        /// <summary>
        /// Convierte un string Base64 en el contenido HTML como texto plano.
        /// </summary>
        /// <param name="base64Html">Contenido HTML codificado en Base64.</param>
        /// <returns>Contenido HTML como texto decodificado.</returns>
        public static string ToHtmlString(this string base64Html)
        {
            if (string.IsNullOrWhiteSpace(base64Html))
                throw new ArgumentException("El contenido Base64 no puede estar vacío.", nameof(base64Html));

            try
            {
                byte[] htmlBytes = Convert.FromBase64String(base64Html);
                return Encoding.UTF8.GetString(htmlBytes);
            }
            catch (FormatException)
            {
                throw new FormatException("El contenido proporcionado no es un Base64 válido.");
            }
        }
    }
}
