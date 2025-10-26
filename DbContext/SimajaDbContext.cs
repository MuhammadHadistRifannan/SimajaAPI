using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using MyApp.Namespace;
using SimajaAPI.EntityDbContext;
namespace SimajaAPI.EntitySimaja;

public class SimajaDbContext : DbContext
{
    public SimajaDbContext(DbContextOptions<SimajaDbContext> options) : base(options)
    {

    }
    public DbSet<Users> users { get; set; }
    public DbSet<Roles> roles { get; set; }
}
