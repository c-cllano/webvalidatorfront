using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;

namespace Process.Infrastructure.Utility
{
    public static class GetCertificadoSigningCredentials
    {
        public static X509Certificate2 GetCertificateInfo(string _thumbprintCertifacte)
        {
            X509Store store = new(StoreName.My, StoreLocation.LocalMachine);
            X509Store Root = new(StoreName.Root, StoreLocation.LocalMachine);
            X509Store CertificateAuthority = new(StoreName.CertificateAuthority, StoreLocation.LocalMachine);
            X509Store AddressBook = new(StoreName.AddressBook, StoreLocation.LocalMachine);
            X509Store TrustedPublisher = new(StoreName.TrustedPublisher, StoreLocation.LocalMachine);
            X509Store TrustedPeople = new(StoreName.TrustedPeople, StoreLocation.LocalMachine);

            store.Open(OpenFlags.ReadOnly);
            Root.Open(OpenFlags.ReadOnly);
            CertificateAuthority.Open(OpenFlags.ReadOnly);
            AddressBook.Open(OpenFlags.ReadOnly);
            TrustedPublisher.Open(OpenFlags.ReadOnly);
            TrustedPeople.Open(OpenFlags.ReadOnly);

            var certs = store.Certificates.Find(X509FindType.FindByThumbprint, _thumbprintCertifacte, false).OfType<X509Certificate>().FirstOrDefault();

            certs ??= Root.Certificates.Find(X509FindType.FindByThumbprint, _thumbprintCertifacte, false).OfType<X509Certificate>().FirstOrDefault();

            certs ??= CertificateAuthority.Certificates.Find(X509FindType.FindByThumbprint, _thumbprintCertifacte, false).OfType<X509Certificate>().FirstOrDefault();

            certs ??= AddressBook.Certificates.Find(X509FindType.FindByThumbprint, _thumbprintCertifacte, false).OfType<X509Certificate>().FirstOrDefault();

            certs ??= TrustedPublisher.Certificates.Find(X509FindType.FindByThumbprint, _thumbprintCertifacte, false).OfType<X509Certificate>().FirstOrDefault();

            certs ??= TrustedPeople.Certificates.Find(X509FindType.FindByThumbprint, _thumbprintCertifacte, false).OfType<X509Certificate>().FirstOrDefault();

            store.Close();
            Root.Close();
            CertificateAuthority.Close();
            AddressBook.Close();
            TrustedPublisher.Close();
            TrustedPeople.Close();

            X509Certificate2 certificate = new(certs!);

            return certificate;
        }


        public static X509Certificate2 GetCertificateInfo(string certificate_Path, string certificate_Password)
        {

            string certPath = Environment.GetEnvironmentVariable("CERTIFICATE_PATH") ?? Path.Combine(Directory.GetCurrentDirectory(), certificate_Path);
            string certPassword = Environment.GetEnvironmentVariable("CERTIFICATE_PASSWORD") ?? certificate_Password;

            if (string.IsNullOrWhiteSpace(certPath))
                throw new InvalidOperationException("CERTIFICATE_PATH no está definido.");

            if (string.IsNullOrWhiteSpace(certPassword))
                throw new InvalidOperationException("CERTIFICATE_PASSWORD no está definido.");

            // Cargar directamente desde archivo .pfx
            return new X509Certificate2(certPath, certPassword, X509KeyStorageFlags.MachineKeySet);
        }




        /// <summary>
        /// divide el token encriptado del algoritmo usado para encriptarlo 
        /// </summary>
        /// <param name="token"> token encriptado</param>
        /// <returns>token depurado</returns>
        public static Task<string> GetProcess(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("El token no puede ser nulo o vacío.", nameof(token));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenAux = tokenHandler.ReadJwtToken(token.Trim());

            var processClaim = tokenAux.Claims.FirstOrDefault(c => c.Type.Contains("process", StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(processClaim?.Value ?? string.Empty);
        }

    }
}
