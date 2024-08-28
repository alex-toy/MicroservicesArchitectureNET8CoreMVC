using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Transactions.Core.Dtos;
using Transactions.Core.Dtos.Incentives;
using Transactions.Core.Dtos.Transports;
using Transactions.Core.Services.Incentives;
using Transactions.Core.Services.Transports;
using TransportCart.Api.Data;
using TransportCart.Api.Dtos;

namespace TransportCart.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransportCartController
{
	private IMapper _mapper;
	private readonly AppDbContext _db;
	private ITransportsService _transportsService;
	private IIncentiveService _incentiveService;
	private IConfiguration _configuration;
	//private readonly IMessageBus _messageBus;

	public TransportCartController(
		IMapper mapper, ITransportsService transportsService, IConfiguration configuration, AppDbContext db, IIncentiveService incentiveService)
	{
		_mapper = mapper;
		_transportsService = transportsService;
		_configuration = configuration;
		_db = db;
		_incentiveService = incentiveService;
	}

	[HttpGet("GetCart/{userId}")]
	public async Task<ResponseDto<CartDto>> GetCart(string userId)
	{
		try
		{
			CartDto cart = new()
			{
				CartHeader = _mapper.Map<CartHeaderDto>(_db.CartHeaders.First(cartHeader => cartHeader.UserId == userId))
			};

			cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(_db.CartDetails.Where(u => u.CartHeaderId == cart.CartHeader.CartHeaderId));

			ResponseDto<List<TransportDto>> response = await _transportsService.GetAllAsync(null);
			List<TransportDto> transportDtos = response.Result;

			foreach (CartDetailsDto item in cart.CartDetails)
			{
				item.Transport = transportDtos.FirstOrDefault(u => u.TransportId == item.TransportId);
				cart.CartHeader.TotalPrice += (item.Count * item.Transport.Price);
			}

			if (!string.IsNullOrEmpty(cart.CartHeader.IncentiveCode))
			{
				ResponseDto<IncentiveDto> response_incentive = await _incentiveService.GetByCodeAsync(cart.CartHeader.IncentiveCode);
				IncentiveDto incentive = response_incentive.Result;
				if (cart.CartHeader.TransportCount > incentive.MinTransportCount && cart.CartHeader.TotalDistance > incentive.MinKilometersCount)
				{
					cart.CartHeader.TotalPrice -= incentive.Bonus;
					cart.CartHeader.Bonus = incentive.Bonus;
				}
			}

			return new ResponseDto<CartDto> { Result = cart, IsSuccess = true };
		}
		catch (Exception ex)
		{
			return new ResponseDto<CartDto> { IsSuccess = false, ErrorMessage = ex.Message };
		}
	}

	//[HttpPost("ApplyCoupon")]
	//public async Task<object> ApplyCoupon([FromBody] CartDto cartDto)
	//{
	//	try
	//	{
	//		var cartFromDb = await _db.CartHeaders.FirstAsync(u => u.UserId == cartDto.CartHeader.UserId);
	//		cartFromDb.CouponCode = cartDto.CartHeader.CouponCode;
	//		_db.CartHeaders.Update(cartFromDb);
	//		await _db.SaveChangesAsync();
	//		_response.Result = true;
	//	}
	//	catch (Exception ex)
	//	{
	//		_response.IsSuccess = false;
	//		_response.Message = ex.ToString();
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

	//[HttpPost("CartUpsert")]
	//public async Task<ResponseDto> CartUpsert(CartDto cartDto)
	//{
	//	try
	//	{
	//		var cartHeaderFromDb = await _db.CartHeaders.AsNoTracking()
	//			.FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);
	//		if (cartHeaderFromDb == null)
	//		{
	//			//create header and details
	//			CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
	//			_db.CartHeaders.Add(cartHeader);
	//			await _db.SaveChangesAsync();
	//			cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
	//			_db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
	//			await _db.SaveChangesAsync();
	//		}
	//		else
	//		{
	//			//if header is not null
	//			//check if details has same product
	//			var cartDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
	//				u => u.ProductId == cartDto.CartDetails.First().ProductId &&
	//				u.CartHeaderId == cartHeaderFromDb.CartHeaderId);
	//			if (cartDetailsFromDb == null)
	//			{
	//				//create cartdetails
	//				cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
	//				_db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
	//				await _db.SaveChangesAsync();
	//			}
	//			else
	//			{
	//				//update count in cart details
	//				cartDto.CartDetails.First().Count += cartDetailsFromDb.Count;
	//				cartDto.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
	//				cartDto.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;
	//				_db.CartDetails.Update(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
	//				await _db.SaveChangesAsync();
	//			}
	//		}
	//		_response.Result = cartDto;
	//	}
	//	catch (Exception ex)
	//	{
	//		_response.Message = ex.Message.ToString();
	//		_response.IsSuccess = false;
	//	}
	//	return _response;
	//}

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
}
