namespace Transactions.Web.Dtos.Incentives;

public class FilterIncentiveDto
{
	public int TransportCount { get; set; } = 0;

	private string _transportComparator = string.Empty;
	public string TransportComparator
	{
		get { return _transportComparator; }
		set { _transportComparator = value ?? string.Empty; }
	}

	public int KilometersCount { get; set; } = 0;

	private string _kilometerComparator = string.Empty;
	public string KilometerComparator
	{
		get { return _kilometerComparator; }
		set { _kilometerComparator = value ?? string.Empty; }
	}

	public double Bonus { get; set; } = 0;

	private string _bonusComparator = string.Empty;
	public string BonusComparator
	{
		get { return _bonusComparator; }
		set { _bonusComparator = value ?? string.Empty; }
	}
}
