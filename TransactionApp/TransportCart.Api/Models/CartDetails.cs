using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transactions.Core.Dtos.Transports;
using TransportCart.Api.Models;

namespace Bonus.API.Models;

public class CartDetails
{
	[Key]
	public int CartDetailsId { get; set; }

	public int CartHeaderId { get; set; }

	[ForeignKey("CartHeaderId")]
	public CartHeader CartHeader { get; set; }

	public int ProductId { get; set; }

	[NotMapped]
	public TransportDto Transport { get; set; }

	public int Count { get; set; }
}
