namespace ApiGateways.API.Core.Handlers
{
    public class IgnoreSslHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    (msg, cert, chain, errors) => true
            };

            var client = new HttpClient(handler);
            return client.SendAsync(request, cancellationToken);
        }
    }
}
