namespace Transactions.Web.Dtos.Incentives;

public class IncentiveDto
{
	public int IncentiveId { get; set; }
	public string IncentiveCode { get; set; }
	public int MinTransportCount { get; set; }
	public int MinKilometersCount { get; set; }
	public double Bonus { get; set; }
}
