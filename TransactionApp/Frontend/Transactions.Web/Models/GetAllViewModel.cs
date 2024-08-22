using Transactions.Web.Dtos.Data;
using Transactions.Web.Dtos.Requests;

namespace Transactions.Web.Models;

public class GetAllViewModel
{
    public List<IncentiveDto> Incentives { get; set; }
    public FilterIncentiveDto Filter { get; set; }
}
