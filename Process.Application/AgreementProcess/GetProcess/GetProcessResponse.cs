
namespace Process.Application.AgreementProcess.GetProcess
{
    public class GetProcessResponse
    {

        public string? Error { get; set; }
        public  string? Token { get; set; }
        public Guid Id { get; set; }
        public bool IsValid { get; set; }

    }
}
