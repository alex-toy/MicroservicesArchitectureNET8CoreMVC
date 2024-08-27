using Transactions.Web.Dtos.Incentives;

namespace Transactions.Web.Models;

public class GetAllIncentivesVM
{
    public List<IncentiveDto> Incentives { get; set; }
    public FilterIncentiveDto Filter { get; set; }
}
