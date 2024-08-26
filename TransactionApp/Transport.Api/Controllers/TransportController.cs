using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Transactions.Core.Dtos;
using Transports.Api.Data;
using Transports.Api.Dtos;
using Transports.Api.Services;

namespace Transports.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class TransportController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    private readonly ITransportService _transportService;

    public TransportController(AppDbContext db, IMapper mapper, ITransportService incentiveService)
    {
        _db = db;
        _mapper = mapper;
        _transportService = incentiveService;
    }

    [HttpPost("GetAll")]
    public ResponseDto<List<TransportDto>> GetAll([FromBody] FilterTransportDto filter)
    {
        try
        {
            List<TransportDto> incentiveDtos = _transportService.GetAll(filter);
            return new ResponseDto<List<TransportDto>> { Result = incentiveDtos, IsSuccess = true };
        }
        catch (Exception ex)
        {
            return new ResponseDto<List<TransportDto>> { IsSuccess = false, ErrorMessage = ex.Message };
        }
    }
}
