using Microsoft.EntityFrameworkCore;
using Transports.Api.Models;

namespace Transports.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Transport> Transports { get; set; }
}
