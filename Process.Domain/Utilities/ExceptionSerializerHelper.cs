// Inicio código generado por GitHub Copilot
using System.Collections;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Process.Domain.Utilities
{
    public static class ExceptionSerializerHelper
    {
        // Inicio optimización por GitHub Copilot
        // Campo estático para cachear las opciones de serialización JSON
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        // Fin optimización por GitHub Copilot

        // Método generado por GitHub Copilot
        public static string ToJson(this Exception ex)
        {
            var obj = SerializeException(ex);

            return JsonSerializer.Serialize(obj, JsonOptions);
        }

        // Método generado por GitHub Copilot
        private static object SerializeException(Exception ex)
        {
            if (ex == null) return null!;

            return new
            {
                ex.Message,
                ExceptionType = ex.GetType().FullName,
                ex.StackTrace,
                ex.Source,
                ex.HResult,
                TargetSite = ex.TargetSite?.ToString(),
                Data = GetExceptionData(ex),
                InnerException = ex.InnerException != null
                    ? SerializeException(ex.InnerException)
                    : null,
                Properties = GetCustomProperties(ex)
            };
        }

        // Método generado por GitHub Copilot
        private static Dictionary<string, object> GetExceptionData(Exception ex)
        {
            var dict = new Dictionary<string, object>();

            if (ex.Data != null)
            {
                foreach (DictionaryEntry item in ex.Data)
                {
                    dict[item.Key?.ToString() ?? "null"] = item.Value?.ToString() ?? "";
                }
            }

            return dict;
        }

        // Método generado por GitHub Copilot
        private static Dictionary<string, object> GetCustomProperties(Exception ex)
        {
            var result = new Dictionary<string, object>();

            var baseProps = typeof(Exception)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => p.Name)
                .ToHashSet();

            var props = ex.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => !baseProps.Contains(p.Name));

            foreach (var prop in props)
            {
                try
                {
                    var value = prop.GetValue(ex);
                    result[prop.Name] = value?.ToString() ?? "";
                }
                catch
                {
                    result[prop.Name] = "No disponible";
                }
            }

            return result;
        }
    }
}
// Fin código generado por GitHub Copilot
