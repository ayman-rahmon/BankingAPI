namespace BankingAPI.DataAccess;

using BankingAPI.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDBContext : DbContext
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
        : base(options) { }

    // Uncomment if you have DbSet properties
    // public DbSet<User> Users { get; set; }
}
