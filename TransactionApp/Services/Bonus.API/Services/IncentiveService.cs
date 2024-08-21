using AutoMapper;
using Bonus.API.Dtos;
using Bonus.API.Models;
using Products.API.DbContexts;

namespace Bonus.API.Services;

public class IncentiveService : IIncentiveService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public IncentiveService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public int Upsert(IncentiveDto incentiveDto)
    {
        Incentive incentive = _mapper.Map<Incentive>(incentiveDto);

        if (incentive.IncentiveId > 0) 
        {
            _db.Incentives.Update(incentive);
        }
        else
        {
            _db.Incentives.Add(incentive);
        }
        _db.SaveChanges();

        return incentive.IncentiveId;
    }

    public List<IncentiveDto> GetAll()
    {
        IEnumerable<Incentive> incentives = _db.Incentives.ToList();
        List<IncentiveDto> incentiveDtos = _mapper.Map<List<IncentiveDto>>(incentives);
        return incentiveDtos;
    }

    public IncentiveDto? Get(Func<Incentive, bool> predicate)
    {
        Incentive? incentive = _db.Incentives.FirstOrDefault(predicate);
        return _mapper.Map<IncentiveDto>(incentive);
    }

    public bool Delete(int incentiveId)
    {
        Incentive? incentive = _db.Incentives.FirstOrDefault(i => i.IncentiveId == incentiveId);
        if (incentive is null) return false;
        _db.Incentives.Remove(incentive);
        _db.SaveChanges();
        return true;
    }
}
