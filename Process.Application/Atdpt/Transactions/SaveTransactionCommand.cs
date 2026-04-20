using MediatR;
using Process.Domain.Parameters.Atdpt;
using Process.Domain.ValueObjects;

namespace Process.Application.Atdpt.Transactions
{
    public record SaveTransactionCommand(SaveTransactionRequest Request,Guid AgreementGuid) : IRequest<AtdpTransactionSave>;

}
