using shopapp.Domain;
using Microsoft.EntityFrameworkCore;

namespace shopapp.Infrastructure
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        // Hisse senetlerini temsil eden DbSet özelliği.
        public DbSet<Stock> Stocks { get; set; }

        // Yorumları temsil eden DbSet özelliği.
        public DbSet<Comment> Comments { get; set; }
    }
}
