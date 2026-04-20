using MediatR;

namespace Dactyloscopy.Application.LogServices.CreateError
{
    public class CreateErrorCommand : IRequest<bool>
    {
        public int SeverityID { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? UserID { get; set; }
        public string? TransactionID { get; set; }
        public string? Code { get; set; }
        public string? Component { get; set; }
        public string? Machine { get; set; }
        public DateTime Date { get; set; }
    }
}
