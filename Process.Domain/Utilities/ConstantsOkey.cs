namespace Process.Domain.Utilities
{
    public static class ConstantsOkey
    {
        private static readonly string UnknownError = "Código de error desconocido";

        private static readonly Dictionary<string, string> _messagesError = new()
        {
            { "6000", "Superó riesgo validación documento" },
            { "7000", "El número de documento del ciudadano, no coincide con el del documento enviado" },
            { "8000", "La coincidencia del nombre es insuficiente. El porcentaje de similitud debe ser del 80% o superior" }
        };

        public static string GetMessageError(string code)
        => _messagesError.TryGetValue(code, out var message)
            ? message
            : UnknownError;
    }
}
