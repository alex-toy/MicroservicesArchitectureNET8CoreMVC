namespace Transactions.Web.Dtos.Requests;

public class DeleteIncentiveDto
{
	public int MinTransportCount { get; set; }
	public int MinKilometersCount { get; set; }
	public double Bonus { get; set; }
}
