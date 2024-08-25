using AutoMapper;
using Bonus.API.Data;
using Bonus.API.Dtos;
using Bonus.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bonus.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IncentiveController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    private readonly IIncentiveService _incentiveService;

    public IncentiveController(AppDbContext db, IMapper mapper, IIncentiveService incentiveService)
    {
        _db = db;
        _mapper = mapper;
        _incentiveService = incentiveService;
    }

    [HttpPost("GetAll")]
    public ResponseDto<List<IncentiveDto>> GetAll([FromBody] FilterIncentiveDto filterIncentive)
    {
        try
        {
            List<IncentiveDto> incentiveDtos = _incentiveService.GetAll(filterIncentive);
            return new ResponseDto<List<IncentiveDto>> { Result = incentiveDtos, IsSuccess = true };
        }
        catch (Exception ex)
        {
            return new ResponseDto<List<IncentiveDto>> { IsSuccess = false, ErrorMessage = ex.Message };
        }
    }

    [HttpGet("GetById/{id:int}")]
    public ResponseDto<IncentiveDto> GetById(int id)
    {
        try
        {
            IncentiveDto? incentiveDto = _incentiveService.Get(i => i.IncentiveId == id);
            if (incentiveDto is null) return new ResponseDto<IncentiveDto> { IsSuccess = false, ErrorMessage = "not found" };
            return new ResponseDto<IncentiveDto> { Result = incentiveDto, IsSuccess = true };

        }
        catch (Exception ex)
        {
            return new ResponseDto<IncentiveDto> { IsSuccess = false, ErrorMessage = ex.Message };
        }
    }

    [HttpGet("GetByCode/{code}")]
    public ResponseDto<IncentiveDto> GetByCode(string code)
    {
        try
        {
            IncentiveDto? incentiveDto = _incentiveService.Get(i => i.IncentiveCode == code);
            if (incentiveDto is null) return new ResponseDto<IncentiveDto> { IsSuccess = false, ErrorMessage = "not found" };
            return new ResponseDto<IncentiveDto> { Result = incentiveDto, IsSuccess = true };

        }
        catch (Exception ex)
        {
            return new ResponseDto<IncentiveDto> { IsSuccess = false, ErrorMessage = ex.Message };
        }
    }

    [HttpPost("Create")]
    public ResponseDto<int> Create([FromBody] IncentiveDto incentive)
    {
        try
        {
            int incentiveId = _incentiveService.Upsert(incentive);
            return new ResponseDto<int> { Result = incentiveId, IsSuccess = true };
        }
        catch (Exception ex)
        {
            return new ResponseDto<int> { IsSuccess = false, ErrorMessage = ex.Message, Result = 0 };
        }
    }

    [HttpPut("Update")]
    public ResponseDto<int> Update([FromBody] IncentiveDto incentive)
    {
        try
        {
            int incentiveId = _incentiveService.Upsert(incentive);
            return new ResponseDto<int> { Result = incentiveId, IsSuccess = true };
        }
        catch (Exception ex)
        {
            return new ResponseDto<int> { IsSuccess = false, ErrorMessage = ex.Message, Result = 0 };
        }
    }

    [HttpDelete("Delete/{id:int}")]
    public ResponseDto<bool> Delete(int id)
    {
        try
        {
            bool isSuccess = _incentiveService.Delete(id);
            if (!isSuccess) return new ResponseDto<bool> { IsSuccess = false, ErrorMessage = "error deleting" };
            return new ResponseDto<bool> { IsSuccess = true };

        }
        catch (Exception ex)
        {
            return new ResponseDto<bool> { IsSuccess = false, ErrorMessage = ex.Message };
        }
    }

    [HttpDelete("DeleteMany")]
    public ResponseDto<bool> DeleteMany([FromBody] DeleteIncentiveDto incentive)
    {
        try
        {
            bool isSuccess = _incentiveService.DeleteMany(incentive);
            if (!isSuccess) return new ResponseDto<bool> { IsSuccess = false, ErrorMessage = "error deleting" };
            return new ResponseDto<bool> { IsSuccess = true };

        }
        catch (Exception ex)
        {
            return new ResponseDto<bool> { IsSuccess = false, ErrorMessage = ex.Message };
        }
    }
}
