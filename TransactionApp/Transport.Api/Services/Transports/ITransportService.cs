using Transports.Api.Dtos;
using Transports.Api.Models;

namespace Transports.Api.Services.Transports;

public interface ITransportService
{
    int Upsert(TransportDto incentiveDto, string baseUrl = "");
    List<TransportDto> GetAll();
    List<TransportDto> GetAll(FilterTransportDto filter);
    TransportDto? Get(Func<Transport, bool> predicate);
    bool Delete(int incentiveId);
	List<TransportDto> GetByIds(IEnumerable<int> transportIds);
	//bool DeleteMany(DeleteIncentiveDto incentive);
}