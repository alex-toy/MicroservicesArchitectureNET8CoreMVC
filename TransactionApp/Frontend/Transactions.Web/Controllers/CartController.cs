using Transactions.Core.Services.Transports;
using Transactions.Web.Services.Carts;

namespace Transactions.Web.Controllers;

public class CartController
{
	private readonly ICartService _cartService;
	//private readonly IOrderService _orderService;

	public CartController(ICartService cartService)
	{
		_cartService = cartService;
	}
}
