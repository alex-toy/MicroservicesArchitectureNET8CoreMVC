using Transactions.Core.Dtos.TransportCarts;

namespace TransportCart.Api.Services.Carts;

public interface ICartService
{
	Task ApplyIncentive(CartDto cartDto);
	Task CartUpsert(CartDto cartDto);
	Task<CartDto> GetByUserId(string userId);
}