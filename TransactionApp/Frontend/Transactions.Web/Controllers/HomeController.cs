using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Transactions.Core.Dtos;
using Transactions.Core.Dtos.TransportCarts;
using Transactions.Core.Dtos.Transports;
using Transactions.Core.Services.Transports;
using Transactions.Web.Models;
using Transactions.Web.Services.Carts;

namespace Transactions.Web.Controllers
{
	public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly ITransportsService _productService;
        private readonly ICartService _cartService;

        public HomeController(ILogger<HomeController> logger, ITransportsService productService, ICartService cartService)
        {
            _logger = logger;
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
		{
			List<TransportDto>? transports = new();

			ResponseDto<List<TransportDto>> response = await _productService.GetAllAsync(new FilterTransportDto());

			if (response.IsSuccess)
			{
				transports = response.Result;
			}
			else
			{
				TempData["error"] = response?.ErrorMessage;
			}

			return View(transports);
		}

        [Authorize]
        public async Task<IActionResult> TransportDetails(int transportId)
        {
            TransportDto model = new();

            ResponseDto<TransportDto> response = await _productService.GetByIdAsync(transportId);

            if (response.IsSuccess)
            {
                model = response.Result;
            }
            else
            {
                TempData["error"] = response?.ErrorMessage;
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ActionName("TransportDetails")]
        public async Task<IActionResult> TransportDetails(TransportDto productDto)
        {
            CartDto cartDto = new CartDto()
            {
                CartHeader = new CartHeaderDto
                {
                    UserId = User.Claims.Where(u => u.Type == JwtClaimTypes.Subject)?.FirstOrDefault()?.Value
                }
            };

            CartDetailsDto cartDetails = new CartDetailsDto()
            {
                Count = productDto.Count,
                TransportId = productDto.TransportId,
            };

            cartDto.CartDetails = new List<CartDetailsDto>() { cartDetails };

            ResponseDto<bool>? response = await _cartService.UpsertCartAsync(cartDto);

            if (response.IsSuccess)
            {
                TempData["success"] = "Item has been added to the Shopping Cart";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.ErrorMessage;
            }

            return View(productDto);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}