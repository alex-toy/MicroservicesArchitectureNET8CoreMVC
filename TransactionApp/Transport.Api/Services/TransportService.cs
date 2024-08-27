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

    public int Upsert(TransportDto transportDto, string baseUrl = "")
    {
        Transport transport = _mapper.Map<Transport>(transportDto);
        //HandleImage(transportDto, baseUrl, transport);

        if (transport.TransportId > 0)
        {
            _db.Transports.Update(transport);
        }
        else
        {
            _db.Transports.Add(transport);
        }

        _db.SaveChanges();

        return transport.TransportId;
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

		Func<Transport, bool>? distancePredicate = transport =>
		{
			if (filter.DistanceComparator == ">") return transport.DistanceKm > filter.DistanceKm;
			if (filter.DistanceComparator == ">=") return transport.DistanceKm >= filter.DistanceKm;
			if (filter.DistanceComparator == "<") return transport.DistanceKm < filter.DistanceKm;
			if (filter.DistanceComparator == "<=") return transport.DistanceKm <= filter.DistanceKm;
			return true;
		};

		Func<Transport, bool>? toPredicate = transport => transport.To.Contains(filter.To) || string.IsNullOrEmpty(filter.To);
        Func<Transport, bool>? fromPredicate = transport => transport.From.Contains(filter.From) || string.IsNullOrEmpty(filter.From);

        Func<Transport, bool>? predicate = i => transportPredicate(i) && distancePredicate(i) && toPredicate(i) && fromPredicate(i);

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

    private static void HandleImage(TransportDto transportDto, string baseUrl, Transport transport)
    {
        if (transportDto.ImageUrl is not null)
        {

            string fileName = transport.TransportId + Path.GetExtension(transportDto.ImageUrl);
            string filePath = @"wwwroot\ProductImages\" + fileName;

            string directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);
            FileInfo file = new FileInfo(directoryLocation);
            if (file.Exists) file.Delete();

            string filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
            using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
            {
                //transportDto.Image.CopyTo(fileStream);
            }
            transport.ImageUrl = baseUrl + "/ProductImages/" + fileName;
            transport.ImageLocalPath = filePath;
        }
        else
        {
            transport.ImageUrl = "https://placehold.co/600x400";
        }
    }
}
