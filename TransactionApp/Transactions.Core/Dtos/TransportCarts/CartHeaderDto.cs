namespace Transactions.Core.Dtos.TransportCarts;

public class CartHeaderDto
{
	public int CartHeaderId { get; set; }

	public string? UserId { get; set; }

	public string? IncentiveCode { get; set; }

	public double TotalPrice { get; set; }

	public double Bonus { get; set; }

	public int TotalDistance { get; set; }

	public int TransportCount { get; set; }

	public string? Name { get; set; }

	public string? Phone { get; set; }

	public string? Email { get; set; }
}
