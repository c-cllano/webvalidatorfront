namespace Process.Domain.Services
{
    public interface ISigningCredentialService
    {
       Task<string> ConsultAgreementProcess(string process);
    }
}
