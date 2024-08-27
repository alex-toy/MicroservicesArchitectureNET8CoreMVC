using System.ComponentModel.DataAnnotations;

namespace Transactions.Web.Dtos.Transports;

public class TransportDto
{
    public int TransportId { get; set; }

    public string From { get; set; }

    public string To { get; set; }

    public double Price { get; set; }

    public string Category { get; set; }

    public string? ImageUrl { get; set; }

    public string? ImageLocalPath { get; set; }

    [Range(1, 100)]
    public int Count { get; set; } = 1;

    //[MaxFileSize(1)]
    //[AllowedExtensions(new string[] { ".jpg", ".png" })]
    //public IFormFile? Image { get; set; }
}
