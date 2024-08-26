using System.ComponentModel.DataAnnotations;

namespace Transports.Api.Models;

public class Transport
{
    [Key]
    public int TransportId { get; set; }

    [Required]
    public string From { get; set; }

    [Required]
    public string To { get; set; }

    [Range(1, 1000)]
    public double Price { get; set; }

    public string Category { get; set; }
}
