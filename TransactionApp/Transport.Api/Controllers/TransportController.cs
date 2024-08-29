using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transactions.Core.Dtos;
using Transports.Api.Data;
using Transports.Api.Dtos;
using Transports.Api.Services.Transports;

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
			List<TransportDto> transportDtos = _transportService.GetAll(filter);
			return new ResponseDto<List<TransportDto>> { Result = transportDtos, IsSuccess = true };
		}
		catch (Exception ex)
		{
			return new ResponseDto<List<TransportDto>> { IsSuccess = false, ErrorMessage = ex.Message };
		}
	}

	[HttpPost("GetByIds")]
	public ResponseDto<List<TransportDto>> GetByIds([FromBody] IEnumerable<int> transportIds)
	{
		try
		{
			List<TransportDto> transportDtos = _transportService.GetByIds(transportIds);
			return new ResponseDto<List<TransportDto>> { Result = transportDtos, IsSuccess = true };
		}
		catch (Exception ex)
		{
			return new ResponseDto<List<TransportDto>> { IsSuccess = false, ErrorMessage = ex.Message };
		}
	}

	[HttpGet("GetById/{id:int}")]
    public ResponseDto<TransportDto> GetById(int id)
    {
        try
        {
            TransportDto? TransportDto = _transportService.Get(i => i.TransportId == id);
            if (TransportDto is null) return new ResponseDto<TransportDto> { IsSuccess = false, ErrorMessage = "not found" };
            return new ResponseDto<TransportDto> { Result = TransportDto, IsSuccess = true };
        }
        catch (Exception ex)
        {
            return new ResponseDto<TransportDto> { IsSuccess = false, ErrorMessage = ex.Message };
        }
    }

    [HttpPost("Create")]
    [Authorize(Roles = "ADMIN")]
    public ResponseDto<int> Create([FromBody] TransportDto transportDto)
    {
        string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";

        try
        {
            int incentiveId = _transportService.Upsert(transportDto, baseUrl);
            return new ResponseDto<int> { Result = incentiveId, IsSuccess = true };
        }
        catch (Exception ex)
        {
            return new ResponseDto<int> { IsSuccess = false, ErrorMessage = ex.Message, Result = 0 };
        }
    }

    [HttpPut("Update")]
	[Authorize(Roles = "ADMIN")]
	public ResponseDto<int> Update([FromBody] TransportDto transport)
    {
        string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";

        try
        {
            int incentiveId = _transportService.Upsert(transport, baseUrl);
            return new ResponseDto<int> { Result = incentiveId, IsSuccess = true };
        }
        catch (Exception ex)
        {
            return new ResponseDto<int> { IsSuccess = false, ErrorMessage = ex.Message, Result = 0 };
        }
    }

    [HttpDelete("Delete/{id:int}")]
	[Authorize(Roles = "ADMIN")]
	public ResponseDto<bool> Delete(int id)
    {
        try
        {
            bool isSuccess = _transportService.Delete(id);
            if (!isSuccess) return new ResponseDto<bool> { IsSuccess = false, ErrorMessage = "error deleting" };
            return new ResponseDto<bool> { IsSuccess = true };

        }
        catch (Exception ex)
        {
            return new ResponseDto<bool> { IsSuccess = false, ErrorMessage = ex.Message };
        }
    }
}
