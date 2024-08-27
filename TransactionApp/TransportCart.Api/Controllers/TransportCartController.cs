using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Transactions.Core.Services.Transports;
using TransportCart.Api.Data;

namespace TransportCart.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransportCartController
{
	private IMapper _mapper;
	private readonly AppDbContext _db;
	private ITransportsService _transportsService;
	//private IIncentiveService _incentiveService;
	private IConfiguration _configuration;
	//private readonly IMessageBus _messageBus;
}
