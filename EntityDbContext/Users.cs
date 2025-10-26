using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace SimajaAPI.EntityDbContext;

public class Users
{
    [Key]
    public int id { get; set; }
    public string? username { get; set; }
    public string? password { get; set; }
    public int role { get; set; }

    [ForeignKey("role")]
    public Roles? roles { get; set; }
}
