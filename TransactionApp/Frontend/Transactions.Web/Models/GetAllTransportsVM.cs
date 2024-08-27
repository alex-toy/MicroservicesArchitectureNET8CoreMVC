using Transactions.Core.Dtos.Transports;

namespace Transactions.Web.Models;

public class GetAllTransportsVM
{
    public List<TransportDto> Transports { get; set; }
    public FilterTransportDto Filter { get; set; }
}
