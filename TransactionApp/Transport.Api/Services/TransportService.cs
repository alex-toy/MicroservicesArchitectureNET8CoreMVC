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

    public List<TransportDto> GetAll(FilterTransportDto filter)
    {
        Func<Transport, bool>? transportPredicate = transport =>
        {
            if (filter.PriceComparator == ">") return transport.Price > filter.Price;
            if (filter.PriceComparator == ">=") return transport.Price >= filter.Price;
            if (filter.PriceComparator == "<") return transport.Price < filter.Price;
            if (filter.PriceComparator == "<=") return transport.Price <= filter.Price;
            return true;
        };

        Func<Transport, bool>? toPredicate = transport => transport.To == filter.To || string.IsNullOrEmpty(filter.To);
        Func<Transport, bool>? fromPredicate = transport => transport.From == filter.From || string.IsNullOrEmpty(filter.From);

        Func<Transport, bool>? predicate = i =>  transportPredicate(i) && toPredicate(i) && fromPredicate(i);

        IEnumerable<Transport> transports = _db.Transports.Where(predicate);

        return _mapper.Map<List<TransportDto>>(transports);
    }

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
