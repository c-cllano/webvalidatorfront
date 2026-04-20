using DrawFlowProcess.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using UAParser;

namespace DrawFlowProcess.Infrastructure.Repositories
{
    public sealed class ClientInfoRepository : IClientInfoRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Parser _uaParser;

        public ClientInfoRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _uaParser = Parser.GetDefault();
        }

        public string GetBrowser()
        {
            var userAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString() ?? "";
            var ua = userAgent.ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(ua))
                return "Desconocido";

            if (ua.Contains("edg"))
                return "Edge";
            if (ua.Contains("chrome"))
                return "Chrome";
            if (ua.Contains("firefox"))
                return "Firefox";
            if (ua.Contains("safari") && !ua.Contains("chrome"))
                return "Safari";
            if (ua.Contains("opr") || ua.Contains("opera"))
                return "Opera";

            return "Desconocido";
        }


        public string GetOS()
        {
            var userAgent = _httpContextAccessor.HttpContext?.Items["UserAgent"]?.ToString();
            if (string.IsNullOrEmpty(userAgent)) return "Desconocido";

            var clientInfo = _uaParser.Parse(userAgent);
            return $"{clientInfo.OS.Family} {clientInfo.OS.Major}";
        }

        public string GetDevice()
        {
            var userAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString() ?? "";

            var ua = userAgent.ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(ua))
                return "Desconocido";

            if (ua.Contains("mobi") || ua.Contains("iphone") || ua.Contains("android"))
                return "Móvil";

            if (ua.Contains("tablet") || ua.Contains("ipad"))
                return "Tablet";

            if (ua.Contains("windows") || ua.Contains("macintosh") || ua.Contains("linux"))
                return "Escritorio";

            return "Desconocido";
        }

    }
}
