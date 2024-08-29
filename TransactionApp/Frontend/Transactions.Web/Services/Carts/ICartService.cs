using Transactions.Core.Dtos;
using Transactions.Core.Dtos.TransportCarts;

namespace Transactions.Web.Services.Carts
{
	public interface ICartService
	{
		Task<ResponseDto<CartDto>> GetCartByUserIdAsnyc(string userId);
		Task<ResponseDto<bool>> UpsertCartAsync(CartDto cartDto);
		Task<ResponseDto<bool>> RemoveFromCartAsync(int cartDetailsId);
		Task<ResponseDto<bool>> ApplyCouponAsync(CartDto cartDto);
		Task<ResponseDto<bool>> EmailCartAsync(CartDto cartDto);
	}
}