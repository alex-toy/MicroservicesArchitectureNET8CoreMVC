using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Transactions.Core.Dtos;
using Transactions.Core.Dtos.TransportCarts;
using TransportCart.Api.Services.Carts;

namespace TransportCart.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransportCartController
{
	private ICartService _cartService;
	private IConfiguration _configuration;
	//private readonly IMessageBus _messageBus;

	public TransportCartController(
		IMapper mapper, IConfiguration configuration, ICartService cartService)
	{
		_configuration = configuration;
		_cartService = cartService;
	}

	[HttpGet("GetCart/{userId}")]
	public async Task<ResponseDto<CartDto>> GetCart(string userId)
	{
		try
		{
			CartDto cart = await _cartService.GetByUserId(userId);

			return new ResponseDto<CartDto> { Result = cart, IsSuccess = true };
		}
		catch (Exception ex)
		{
			return new ResponseDto<CartDto> { IsSuccess = false, ErrorMessage = ex.Message };
		}
	}

	[HttpPost("ApplyIncentive")]
	public async Task<ResponseDto<bool>> ApplyIncentive([FromBody] CartDto cartDto)
	{
		try
		{
			await _cartService.ApplyIncentive(cartDto);
			return new ResponseDto<bool> { Result = true, IsSuccess = true };
		}
		catch (Exception ex)
		{
			return new ResponseDto<bool> { IsSuccess = false, Result = false, ErrorMessage = ex.Message };
		}
	}

	[HttpPost("CartUpsert")]
	public async Task<ResponseDto<bool>> CartUpsert(CartDto cartDto)
	{
		try
		{
			await _cartService.CartUpsert(cartDto);
			return new ResponseDto<bool>() { IsSuccess = true};  
		}
		catch (Exception ex)
		{
			return new ResponseDto<bool> { IsSuccess = false, Result = false, ErrorMessage = ex.Message };
		}
	}

	[HttpPost("RemoveCart")]
	public async Task<ResponseDto<bool>> RemoveCart([FromBody] int cartDetailsId)
	{
		try
		{
			await _cartService.RemoveCart(cartDetailsId);

			return new ResponseDto<bool>() { IsSuccess = true };
		}
		catch (Exception ex)
		{
			return new ResponseDto<bool> { IsSuccess = false, Result = false, ErrorMessage = ex.Message };
		}
	}

	[HttpPost("EmailCartRequest")]
	public async Task<ResponseDto<bool>> EmailCartRequest([FromBody] CartDto cartDto)
	{
		try
		{
			//await _messageBus.PublishMessage(cartDto, _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue"));

			return new ResponseDto<bool>() { IsSuccess = true };
		}
		catch (Exception ex)
		{
			return new ResponseDto<bool> { IsSuccess = false, Result = false, ErrorMessage = ex.Message };
		}
	}
}
