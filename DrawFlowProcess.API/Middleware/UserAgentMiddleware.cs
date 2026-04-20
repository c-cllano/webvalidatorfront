namespace DrawFlowProcess.API.Middleware
{
    public class UserAgentMiddleware
    {
        private readonly RequestDelegate _next;

        public UserAgentMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userAgent = context.Request.Headers["User-Agent"].ToString();

            context.Items["UserAgent"] = userAgent;

            await _next(context);
        }
    }
}
