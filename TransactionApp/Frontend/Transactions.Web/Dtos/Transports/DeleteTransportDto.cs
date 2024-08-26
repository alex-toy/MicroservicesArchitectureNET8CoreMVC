namespace Transactions.Web.Dtos.Transports;

public class DeleteTransportDto
{
    public string To { get; set; }
    public string From { get; set; }
    public string PriceComparator { get; set; }
    public double Price { get; set; }
}
