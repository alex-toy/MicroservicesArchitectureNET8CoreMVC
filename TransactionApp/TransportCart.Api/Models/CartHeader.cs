using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TransportCart.Api.Models;

public class CartHeader
{
	[Key]
	public int CartHeaderId { get; set; }
	public string? UserId { get; set; }
	public string? IncentiveCode { get; set; }

	[NotMapped]
	public double Bonus { get; set; }

	[NotMapped]
	public double CartTotal { get; set; }
}
