﻿using Microsoft.EntityFrameworkCore;
using TransportCart.Api.Models;

namespace TransportCart.Api.Data;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
	}

	public DbSet<CartHeader> CartHeaders { get; set; }
	public DbSet<CartDetails> CartDetails { get; set; }
}
