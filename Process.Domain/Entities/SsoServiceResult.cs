namespace Process.Domain.Entities
{
    /// <summary>
    /// Representa el resultado de una operación de servicio SSO.
    /// </summary>
    /// <typeparam name="T">Tipo de datos devueltos.</typeparam>
    public class SsoServiceResult<T>
    {
        /// <summary>
        /// Indica si la operación fue exitosa.
        /// </summary>
        public bool Success { get; init; }

        /// <summary>
        /// Datos devueltos por la operación.
        /// </summary>
        public T? Data { get; init; }

        /// <summary>
        /// Mensaje de error principal.
        /// </summary>
        public string? Error { get; init; }

        /// <summary>
        /// Lista de errores adicionales.
        /// </summary>
        public List<string>? Errors { get; init; }

        /// <summary>
        /// Código de estado HTTP o de negocio.
        /// </summary>
        public int? StatusCode { get; init; }

        private SsoServiceResult() { }

        /// <summary>
        /// Crea un resultado exitoso con datos.
        /// </summary>
        public static SsoServiceResult<T> Ok(T data) =>
            new() { Success = true, Data = data };

        /// <summary>
        /// Crea un resultado exitoso sin datos.
        /// </summary>
        public static SsoServiceResult<T> Ok() =>
            new() { Success = true };

        /// <summary>
        /// Crea un resultado fallido con mensaje de error y código.
        /// </summary>
        public static SsoServiceResult<T> Fail(string error, int? statusCode = null) =>
            new() { Success = false, Error = error, StatusCode = statusCode };

        /// <summary>
        /// Crea un resultado fallido con múltiples errores.
        /// </summary>
        public static SsoServiceResult<T> Fail(IEnumerable<string> errors, int? statusCode = null) =>
            new() { Success = false, Errors = errors?.ToList(), StatusCode = statusCode, Error = errors?.FirstOrDefault() };

        /// <summary>
        /// Crea un resultado fallido a partir de una excepción.
        /// </summary>
        public static SsoServiceResult<T> FromException(Exception ex, int? statusCode = null) =>
            new() { Success = false, Error = ex.Message, StatusCode = statusCode, Errors = [ex.Message] };
    }
}
