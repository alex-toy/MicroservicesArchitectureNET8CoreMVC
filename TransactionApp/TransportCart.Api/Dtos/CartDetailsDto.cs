using Transactions.Core.Dtos.Transports;

namespace TransportCart.Api.Dtos;

public class CartDetailsDto
{
	public int CartDetailsId { get; set; }

	public int CartHeaderId { get; set; }

	public CartHeaderDto CartHeader { get; set; }

	public int TransportId { get; set; }

	public TransportDto Transport { get; set; }

	public int Count { get; set; }
}
