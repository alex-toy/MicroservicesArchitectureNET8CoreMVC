using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transactions.Core.Dtos.Transports;

namespace TransportCart.Api.Models;

public class CartDetails
{
	[Key]
	public int CartDetailsId { get; set; }


	public int CartHeaderId { get; set; }

	[ForeignKey("CartHeaderId")]
	public CartHeader CartHeader { get; set; }


	public int TransportId { get; set; }

	[NotMapped]
	public TransportDto Transport { get; set; }


	public int Count { get; set; }
}
