namespace Transactions.Core.Dtos.Transports;

public class FilterTransportDto
{
	private string _to = string.Empty;
	public string To
	{
		get { return _to; }
		set { _to = value ?? string.Empty; }
	}

	private string _from = string.Empty;
	public string From
	{
		get { return _from; }
		set { _from = value ?? string.Empty; }
	}

	private string _priceComparator = string.Empty;
	public string PriceComparator
	{
		get { return _priceComparator; }
		set { _priceComparator = value ?? string.Empty; }
	}

	public double Price { get; set; } = 0;


	private string _distanceComparator = string.Empty;
	public string DistanceComparator
	{
		get { return _distanceComparator; }
		set { _distanceComparator = value ?? string.Empty; }
	}

	public int DistanceKm { get; set; } = 0;
}
