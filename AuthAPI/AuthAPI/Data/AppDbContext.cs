using Microsoft.EntityFrameworkCore;
using AuthAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AuthAPI.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
