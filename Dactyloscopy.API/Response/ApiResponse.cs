namespace Dactyloscopy.API.Response
{
    public class ApiResponse<T>(T data, int code, string codeName)
    {
        public T Data { get; set; } = data;
        public int Code { get; set; } = code;
        public string CodeName { get; set; } = codeName;

        public static ApiResponse<T> Success(T data, int code = 200, string codeName = "OK")
        {
            return new ApiResponse<T>(data, code, codeName);
        }

        public static ApiResponse<T> Error(T data, int code, string codeName)
        {
            return new ApiResponse<T>(data, code, codeName);
        }
    }

    public static class ApiResponse
    {
        public static ApiResponse<object> Success(int code = 200, string codeName = "OK")
            => ApiResponse<object>.Success(null!, code, codeName);

        public static ApiResponse<object> Error(int code, string codeName)
            => ApiResponse<object>.Error(null!, code, codeName);
    }
}
