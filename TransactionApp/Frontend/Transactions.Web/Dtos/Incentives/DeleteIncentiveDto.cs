namespace Transactions.Web.Dtos.Incentives;

public class DeleteIncentiveDto
{
	public int MinTransportCount { get; set; }
	public int MinKilometersCount { get; set; }
	public double Bonus { get; set; }
}
