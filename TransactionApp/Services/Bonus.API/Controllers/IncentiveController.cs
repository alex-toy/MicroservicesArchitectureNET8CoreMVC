using AutoMapper;
using Azure;
using Bonus.API.Dtos;
using Bonus.API.Models;
using Microsoft.AspNetCore.Mvc;
using Products.API.DbContexts;

namespace Bonus.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IncentiveController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public IncentiveController(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    [HttpGet("GetAll")]
    public ResponseDto<List<IncentiveDto>> GetAll()
    {
        try
        {
            IEnumerable<Incentive> incentives = _db.Incentives.ToList();
            List<IncentiveDto> incentiveDtos =  _mapper.Map<List<IncentiveDto>>(incentives);
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
            Incentive incentive = _db.Incentives.First(i => i.IncentiveId == id);
            IncentiveDto incentiveDto = _mapper.Map<IncentiveDto>(incentive);
            return new ResponseDto<IncentiveDto> { Result = incentiveDto , IsSuccess = true };

        }
        catch (Exception ex)
        {
            return new ResponseDto<IncentiveDto> { IsSuccess = false, ErrorMessage = ex.Message };
        }
    }
}
