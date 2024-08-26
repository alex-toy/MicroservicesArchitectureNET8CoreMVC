namespace Transports.Api.Dtos;

public class TransportDto
{
    public int TransportId { get; set; }

    public string From { get; set; }

    public string To { get; set; }

    public double Price { get; set; }

    public string Category { get; set; }
}
