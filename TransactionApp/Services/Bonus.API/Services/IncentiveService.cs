using AutoMapper;
using Bonus.API.Data;
using Bonus.API.Dtos;
using Bonus.API.Models;

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

    public List<IncentiveDto> GetAll(FilterIncentiveDto filterIncentive)
	{
		Func<Incentive, bool>? transportPredicate = incentive =>
		{
			if (filterIncentive.TransportComparator == ">") return incentive.MinTransportCount > filterIncentive.TransportCount;
			if (filterIncentive.TransportComparator == ">=") return incentive.MinTransportCount >= filterIncentive.TransportCount;
			if (filterIncentive.TransportComparator == "<") return incentive.MinTransportCount < filterIncentive.TransportCount;
			if (filterIncentive.TransportComparator == "<=") return incentive.MinTransportCount <= filterIncentive.TransportCount;
			return true;
		};

		Func<Incentive, bool>? kilometerPredicate = incentive =>
		{
			if (filterIncentive.KilometerComparator == ">") return incentive.MinKilometersCount > filterIncentive.KilometersCount;
			if (filterIncentive.KilometerComparator == ">=") return incentive.MinKilometersCount >= filterIncentive.KilometersCount;
			if (filterIncentive.KilometerComparator == "<") return incentive.MinKilometersCount < filterIncentive.KilometersCount;
			if (filterIncentive.KilometerComparator == "<=") return incentive.MinKilometersCount <= filterIncentive.KilometersCount;
			return true;
		};

		Func<Incentive, bool>? bonusPredicate = incentive =>
		{
			if (filterIncentive.BonusComparator == ">") return incentive.Bonus > filterIncentive.Bonus;
			if (filterIncentive.BonusComparator == ">=") return incentive.Bonus >= filterIncentive.Bonus;
			if (filterIncentive.BonusComparator == "<") return incentive.Bonus < filterIncentive.Bonus;
			if (filterIncentive.BonusComparator == "<=") return incentive.Bonus <= filterIncentive.Bonus;
			return true;
		};

		Func<Incentive, bool>? predicate = i => kilometerPredicate(i) && bonusPredicate(i) && transportPredicate(i);

		IEnumerable<Incentive> incentives = _db.Incentives.Where(predicate);

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

    public bool DeleteMany(DeleteIncentiveDto incentive)
    {
        List<Incentive> incentives = _db.Incentives.Where(i => 
            i.MinKilometersCount > incentive.MinKilometersCount && 
            i.Bonus > incentive.Bonus &&
            i.MinTransportCount > incentive.MinTransportCount
        ).ToList();
        _db.Incentives.RemoveRange(incentives);
        _db.SaveChanges();
        return true;
    }
}
