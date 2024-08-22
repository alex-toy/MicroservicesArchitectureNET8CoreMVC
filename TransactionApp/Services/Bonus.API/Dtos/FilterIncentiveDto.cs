namespace Bonus.API.Dtos;

public class FilterIncentiveDto
{
	public string TransportComparator { get; set; }
	public int TransportCount { get; set; }
	public string KilometerComparator { get; set; }
	public int KilometersCount { get; set; }
	public string BonusComparator { get; set; }
	public double Bonus { get; set; }
}
