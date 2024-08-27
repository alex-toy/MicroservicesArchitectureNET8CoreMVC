using Transports.Api.Dtos;
using Transports.Api.Models;

namespace Transports.Api.Services;

public interface ITransportService
{
    int Upsert(TransportDto incentiveDto, string baseUrl = "");
    List<TransportDto> GetAll();
    List<TransportDto> GetAll(FilterTransportDto filter);
    TransportDto? Get(Func<Transport, bool> predicate);
    bool Delete(int incentiveId);
    //bool DeleteMany(DeleteIncentiveDto incentive);
}