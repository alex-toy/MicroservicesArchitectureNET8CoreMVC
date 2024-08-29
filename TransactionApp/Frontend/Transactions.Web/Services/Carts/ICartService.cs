using Transactions.Core.Dtos;
using Transactions.Core.Dtos.TransportCarts;

namespace Transactions.Web.Services.Carts
{
	public interface ICartService
	{
		Task<ResponseDto<bool>> ApplyCouponAsync(CartDto cartDto);
		Task<ResponseDto<bool>> EmailCartAsync(CartDto cartDto);
		Task<ResponseDto<bool>> GetCartByUserIdAsnyc(string userId);
		Task<ResponseDto<bool>> RemoveFromCartAsync(int cartDetailsId);
		Task<ResponseDto<bool>> UpsertCartAsync(CartDto cartDto);
	}
}