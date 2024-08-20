using System.ComponentModel.DataAnnotations;

namespace Bonus.API.Models;

public class Incentive
{
    [Key]
    public int IncentiveId { get; set; }

    [Required]
    public string IncentiveCode { get; set; }

    [Required]
    public int MinTransportCount { get; set; }

    [Required]
    public int MinKilometersCount { get; set; }

    [Required]
    public double Bonus { get; set; }
}
