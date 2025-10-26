using Microsoft.EntityFrameworkCore;
using SimajaAPI.EntityDbContext;
using SimajaAPI.EntitySimaja;

namespace SimajaAPI;

public static class ExtensionMiddleware
{
    public static bool Login(this SimajaDbContext simajaDb , string username , string password)
    {
        var findUser = simajaDb.users.Where(e => e.username == username && e.password == password).FirstOrDefault();
        return findUser != null;
    }
    public static bool Login(this SimajaDbContext simajaDb , string username , string password , out Users user)
    {
        var findUser = simajaDb.users.Where(e => e.username == username && e.password == password)
        .Include(role => role.roles)
        .FirstOrDefault();
        user = findUser != null ? findUser : null;
        return user != null;
    }
}
