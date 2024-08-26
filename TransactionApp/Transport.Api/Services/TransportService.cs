using AutoMapper;
using Transports.Api.Data;
using Transports.Api.Dtos;
using Transports.Api.Models;

namespace Transports.Api.Services;

public class TransportService : ITransportService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public TransportService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public int Upsert(TransportDto TransportDto)
    {
        Transport Transport = _mapper.Map<Transport>(TransportDto);

        if (Transport.TransportId > 0)
        {
            _db.Transports.Update(Transport);
        }
        else
        {
            _db.Transports.Add(Transport);
        }
        _db.SaveChanges();

        return Transport.TransportId;
    }

    public List<TransportDto> GetAll()
    {
        IEnumerable<Transport> Transports = _db.Transports.ToList();
        return _mapper.Map<List<TransportDto>>(Transports);
    }

    //public List<TransportDto> GetAll(FilterIncentiveDto filterIncentive)
    //{
    //    Func<Transport, bool>? transportPredicate = Transport =>
    //    {
    //        if (filterIncentive.TransportComparator == ">") return Transport.MinTransportCount > filterIncentive.TransportCount;
    //        if (filterIncentive.TransportComparator == ">=") return Transport.MinTransportCount >= filterIncentive.TransportCount;
    //        if (filterIncentive.TransportComparator == "<") return Transport.MinTransportCount < filterIncentive.TransportCount;
    //        if (filterIncentive.TransportComparator == "<=") return Transport.MinTransportCount <= filterIncentive.TransportCount;
    //        return true;
    //    };

    //    Func<Transport, bool>? kilometerPredicate = Transport =>
    //    {
    //        if (filterIncentive.KilometerComparator == ">") return Transport.MinKilometersCount > filterIncentive.KilometersCount;
    //        if (filterIncentive.KilometerComparator == ">=") return Transport.MinKilometersCount >= filterIncentive.KilometersCount;
    //        if (filterIncentive.KilometerComparator == "<") return Transport.MinKilometersCount < filterIncentive.KilometersCount;
    //        if (filterIncentive.KilometerComparator == "<=") return Transport.MinKilometersCount <= filterIncentive.KilometersCount;
    //        return true;
    //    };

    //    Func<Transport, bool>? bonusPredicate = Transport =>
    //    {
    //        if (filterIncentive.BonusComparator == ">") return Transport.Bonus > filterIncentive.Bonus;
    //        if (filterIncentive.BonusComparator == ">=") return Transport.Bonus >= filterIncentive.Bonus;
    //        if (filterIncentive.BonusComparator == "<") return Transport.Bonus < filterIncentive.Bonus;
    //        if (filterIncentive.BonusComparator == "<=") return Transport.Bonus <= filterIncentive.Bonus;
    //        return true;
    //    };

    //    Func<Transport, bool>? predicate = i => kilometerPredicate(i) && bonusPredicate(i) && transportPredicate(i);

    //    IEnumerable<Transport> Transports = _db.Transports.Where(predicate);

    //    List<TransportDto> incentiveDtos = _mapper.Map<List<TransportDto>>(Transports);
    //    return incentiveDtos;
    //}

    public TransportDto? Get(Func<Transport, bool> predicate)
    {
        Transport? Transport = _db.Transports.FirstOrDefault(predicate);
        return _mapper.Map<TransportDto>(Transport);
    }

    public bool Delete(int TransportId)
    {
        Transport? Transport = _db.Transports.FirstOrDefault(i => i.TransportId == TransportId);
        if (Transport is null) return false;
        _db.Transports.Remove(Transport);
        _db.SaveChanges();
        return true;
    }

    //public bool DeleteMany(DeleteIncentiveDto Transport)
    //{
    //    List<Transport> Transports = _db.Transports.Where(i =>
    //        i.MinKilometersCount > Transport.MinKilometersCount &&
    //        i.Bonus > Transport.Bonus &&
    //        i.MinTransportCount > Transport.MinTransportCount
    //    ).ToList();
    //    _db.Transports.RemoveRange(Transports);
    //    _db.SaveChanges();
    //    return true;
    //}
}
