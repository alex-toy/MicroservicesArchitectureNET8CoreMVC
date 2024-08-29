﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Transactions.Core.Dtos;
using Transactions.Core.Dtos.Incentives;
using Transactions.Core.Dtos.TransportCarts;
using Transactions.Core.Dtos.Transports;
using Transactions.Core.Services.Incentives;
using Transactions.Core.Services.Transports;
using TransportCart.Api.Data;
using TransportCart.Api.Models;

namespace TransportCart.Api.Services.Carts;

public class CartService : ICartService
{
	private readonly AppDbContext _db;
	private ITransportsService _transportsService;
	private IIncentiveService _incentiveService;
	private readonly IMapper _mapper;

	public CartService(AppDbContext db, IMapper mapper, ITransportsService transportsService, IIncentiveService incentiveService)
	{
		_db = db;
		_mapper = mapper;
		_transportsService = transportsService;
		_incentiveService = incentiveService;
	}

	public async Task<CartDto> GetByUserId(string userId)
	{
		CartHeader? cartHeader = _db.CartHeaders.FirstOrDefault(cartHeader => cartHeader.UserId == userId);
		CartHeaderDto cartHeaderDto = _mapper.Map<CartHeaderDto>(cartHeader);
		CartDto cart = new() { CartHeader = cartHeaderDto };

		if (cartHeaderDto is not null)
		{
			List<CartDetails> cartDetails = _db.CartDetails.Where(u => u.CartHeaderId == cart.CartHeader.CartHeaderId).ToList();
			cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(cartDetails);
		}

		List<int> transportIds = cart.CartDetails.Select(x => x.TransportId).ToList();
		ResponseDto<List<TransportDto>> response = await _transportsService.GetByIds(transportIds);
		List<TransportDto> transportDtos = response.Result;

		foreach (CartDetailsDto cartDetail in cart.CartDetails)
		{
			cartDetail.Transport = transportDtos.FirstOrDefault(u => u.TransportId == cartDetail.TransportId);
			cart.CartHeader.TotalPrice += cartDetail.Count * cartDetail.Transport.Price;
			cart.CartHeader.TotalDistance += cartDetail.Count * cartDetail.Transport.DistanceKm;
			cart.CartHeader.TransportCount += cartDetail.Count;
		}

		if (string.IsNullOrEmpty(cart.CartHeader.IncentiveCode)) return cart;

		ResponseDto<IncentiveDto> response_incentive = await _incentiveService.GetByCodeAsync(cart.CartHeader.IncentiveCode);
		IncentiveDto incentive = response_incentive.Result;
		if (incentive is not null && cart.CartHeader.TransportCount > incentive.MinTransportCount && cart.CartHeader.TotalDistance > incentive.MinKilometersCount)
		{
			cart.CartHeader.TotalPrice += incentive.Bonus;
			cart.CartHeader.Bonus = incentive.Bonus;
		}

		return cart;
	}

	public async Task ApplyIncentive(CartDto cartDto)
	{
		CartHeader cartFromDb = await _db.CartHeaders.FirstAsync(u => u.UserId == cartDto.CartHeader.UserId);
		cartFromDb.IncentiveCode = cartDto.CartHeader.IncentiveCode;
		_db.CartHeaders.Update(cartFromDb);
		await _db.SaveChangesAsync();
	}

	public async Task CartUpsert(CartDto cartDto)
	{
		CartHeader? cartHeaderFromDb = await _db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);
		if (cartHeaderFromDb is null)
		{
			await CreateCart(cartDto);
		}
		else
		{
			CartDetails? cartDetailsFromDb = await GetCartDetails(cartDto, cartHeaderFromDb);

			if (cartDetailsFromDb is null)
			{
				await CreateCartDetails(cartDto, cartHeaderFromDb);
			}
			else
			{
				await UpdateCartDetails(cartDto, cartDetailsFromDb);
			}
		}
	}

	public async Task RemoveCart(int cartDetailsId)
	{
		CartDetails cartDetails = _db.CartDetails.First(u => u.CartDetailsId == cartDetailsId);

		int totalCountofCartItem = _db.CartDetails.Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();
		_db.CartDetails.Remove(cartDetails);
		if (totalCountofCartItem == 1)
		{
			CartHeader? cartHeaderToRemove = await _db.CartHeaders.FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);

			if (cartHeaderToRemove is not null) _db.CartHeaders.Remove(cartHeaderToRemove);
		}
		await _db.SaveChangesAsync();
	}

	private async Task UpdateCartDetails(CartDto cartDto, CartDetails? cartDetailsFromDb)
	{
		cartDto.CartDetails.First().Count += cartDetailsFromDb.Count;
		cartDto.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
		cartDto.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;
		_db.CartDetails.Update(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
		await _db.SaveChangesAsync();
	}

	private async Task CreateCartDetails(CartDto cartDto, CartHeader? cartHeaderFromDb)
	{
		cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
		_db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
		await _db.SaveChangesAsync();
	}

	private async Task<CartDetails?> GetCartDetails(CartDto cartDto, CartHeader? cartHeaderFromDb)
	{
		return await _db.CartDetails
			.AsNoTracking()
			.FirstOrDefaultAsync(u => u.TransportId == cartDto.CartDetails.First().TransportId && u.CartHeaderId == cartHeaderFromDb.CartHeaderId);
	}

	private async Task CreateCart(CartDto cartDto)
	{
		CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
		_db.CartHeaders.Add(cartHeader);
		await _db.SaveChangesAsync();
		cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
		_db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
		await _db.SaveChangesAsync();
	}
}
