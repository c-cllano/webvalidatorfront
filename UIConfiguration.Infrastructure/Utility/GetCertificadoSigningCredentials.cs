using System.Security.Cryptography.X509Certificates;

namespace UIConfiguration.Infrastructure.Utility
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
    }
}
