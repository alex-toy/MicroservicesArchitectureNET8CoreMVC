using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Transactions.Core.Dtos;
using Transactions.Core.Dtos.TransportCarts;
using TransportCart.Api.Data;
using TransportCart.Api.Services.Carts;

namespace TransportCart.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransportCartController
{
	private IMapper _mapper;
	private readonly AppDbContext _db;
	private ICartService _cartService;
	private IConfiguration _configuration;
	//private readonly IMessageBus _messageBus;

	public TransportCartController(
		IMapper mapper, IConfiguration configuration, AppDbContext db, ICartService cartService)
	{
		_mapper = mapper;
		_configuration = configuration;
		_db = db;
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

	//[HttpPost("RemoveCart")]
	//public async Task<ResponseDto> RemoveCart([FromBody] int cartDetailsId)
	//{
	//	try
	//	{
	//		CartDetails cartDetails = _db.CartDetails
	//		   .First(u => u.CartDetailsId == cartDetailsId);

	//		int totalCountofCartItem = _db.CartDetails.Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();
	//		_db.CartDetails.Remove(cartDetails);
	//		if (totalCountofCartItem == 1)
	//		{
	//			var cartHeaderToRemove = await _db.CartHeaders
	//			   .FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);

	//			_db.CartHeaders.Remove(cartHeaderToRemove);
	//		}
	//		await _db.SaveChangesAsync();

	//		_response.Result = true;
	//	}
	//	catch (Exception ex)
	//	{
	//		_response.Message = ex.Message.ToString();
	//		_response.IsSuccess = false;
	//	}
	//	return _response;
	//}

	//[HttpPost("EmailCartRequest")]
	//public async Task<object> EmailCartRequest([FromBody] CartDto cartDto)
	//{
	//	try
	//	{
	//		await _messageBus.PublishMessage(cartDto, _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue"));
	//		_response.Result = true;
	//	}
	//	catch (Exception ex)
	//	{
	//		_response.IsSuccess = false;
	//		_response.Message = ex.ToString();
	//	}
	//	return _response;
	//}
}
