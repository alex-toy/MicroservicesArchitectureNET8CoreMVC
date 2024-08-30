namespace Transactions.Core.Dtos.TransportCarts;

public class CartHeaderDto
{
	public int CartHeaderId { get; set; } = 0;

	public string? UserId { get; set; } = string.Empty;

    public string? IncentiveCode { get; set; } = string.Empty;

    public double TotalPrice { get; set; } = 0;

	public double Bonus { get; set; } = 0;

	public int TotalDistance { get; set; } = 0;

	public int TransportCount { get; set; } = 0;

	public string? Name { get; set; } = string.Empty;

	public string? Phone { get; set; } = string.Empty;

	public string? Email { get; set; } = string.Empty;
}
