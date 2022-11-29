
using Microsoft.EntityFrameworkCore;

using CheckerApi.Models;

namespace CheckerApi.Data
{
    public class ApiContext : DbContext
    {
        public DbSet <Student> Students { get; set; } 
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {

        }
    }
}
