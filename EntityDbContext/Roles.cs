using System.ComponentModel.DataAnnotations;

namespace SimajaAPI;

public class Roles
{
    [Key]
    public int id { get; set; }
    [MaxLength(10)]
    public string? roleName { get; set; }
    public string? description { get; set; }
}
