using MediatR;
using Process.Domain.ValueObjects;

namespace Process.Application.Atdpt.Transactions.Querys
{
  public record GetFileTransactionByIdQuery(int AtdpTransactionId, Guid agreementGuid) : IRequest<AtdpTransactionFile>;
  
}
