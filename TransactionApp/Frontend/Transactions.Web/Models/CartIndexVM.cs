using Transactions.Core.Dtos.TransportCarts;

namespace Transactions.Web.Models;

public class CartIndexVM
{
    public CartDto CartDto { get; set; }
    public int TotalDistanceKm { get; set; }
    public int TotalCount{ get; set; }
    public double TotalPrice { get; set; }
}
