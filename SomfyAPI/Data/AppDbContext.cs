
using Microsoft.EntityFrameworkCore;

namespace SomfyAPI.Data
{
    internal class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
